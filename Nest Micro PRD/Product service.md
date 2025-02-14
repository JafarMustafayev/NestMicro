
```
namespace NestProduct.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IVendorWriteRepository _vendorWriteRepository;
    private readonly IVendorReadRepository _vendorReadRepository;
    private readonly IMapper _mapper;
    private IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly HttpClient _httpClient;

    public ProductService(IProductReadRepository productReadRepository,
                           IProductWriteRepository productWriteRepository,
                           IVendorWriteRepository vendorWriteRepository,
                           IVendorReadRepository vendorReadRepository,
                           IMapper mapper,
                           IPublishEndpoint publishEndpoint,
                           ISendEndpointProvider sendEndpointProvider,
                           HttpClient httpClient

        )
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _vendorWriteRepository = vendorWriteRepository;
        _vendorReadRepository = vendorReadRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
        _httpClient = httpClient;
    }

    public ResponseDTO GetAllProducts()
    {
        Expression<Func<Product, object>> orderBy = x => x.Name;

        List<Expression<Func<Product, object>>> includes = new()
        {
            x => x.Vendor,
        };

        //TODO: include product images

        var res = _productReadRepository.GetAll(
            false,
            orderBy, false,
            includes);

        var products = _mapper.Map<List<GetSingleProductForGrid>>(res);

        return new()
        {
            Data = products,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = $"{products.Count} Products fetched successfully"
        };
    }

    public ResponseDTO GetAllProducts(int page = 1, int take = 20)
    {
        Expression<Func<Product, object>> orderBy = x => x.Name;

        List<Expression<Func<Product, object>>> includes = new()
        {
            x => x.Vendor,
        };

        Expression<Func<Product, bool>> filter = x => x.IsActive == true && x.Vendor.IsActive == true;

        //TODO: include product images

        var res = _productReadRepository.GetAllByExpression(
            filter,
            page,
            take,
            false,
            orderBy,
            false,
            includes);

        var products = _mapper.Map<List<GetSingleProductForGrid>>(res.items);

        return new()
        {
            Data = products,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = $"{res.count} Products fetched successfully"
        };
    }

    public async Task<ResponseDTO> GetProductByIdAsync(string id)
    {
        List<Expression<Func<Product, object>>> includes = new()
        {
            x => x.Vendor,
        };

        var res = await _productReadRepository.GetByIdAsync(id, false, includes);

        var product = _mapper.Map<GetSingleProductDTO>(res);

        if (product == null)
        {
            return new()
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Product not found"
            };
        }

        return new()
        {
            Data = product,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = "Product fetched successfully"
        };
    }

    public ResponseDTO SearchProducts(string query, int page)
    {
        Expression<Func<Product, object>> orderBy = x => x.Name;

        List<Expression<Func<Product, object>>> includes = new()
        {
            x => x.Vendor,
        };

        Expression<Func<Product, bool>> filter = x => x.IsActive && x.Vendor.IsActive && x.Name.ToLower().Contains(query.ToLower());

        var res = _productReadRepository.GetAllByExpression(
            filter,
            false,
            orderBy,
            false,
            includes);

        if (res.Count == 0)
        {
            return new()
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Product not found"
            };
        }

        var products = _mapper.Map<List<GetSingleProductForGrid>>(res.Items);

        return new()
        {
            Data = products,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = $"{res.Count} Products fetched successfully"
        };
    }

    public ResponseDTO SearchByVendor(string query, int page)
    {
        Expression<Func<Product, object>> orderBy = x => x.Name;

        List<Expression<Func<Product, object>>> includes = new()
        {
            x => x.Vendor,
        };

        Expression<Func<Product, bool>> expression = x => x.IsActive && x.Vendor.IsActive && x.Vendor.Name.Contains(query);

        var res = _productReadRepository.GetAllByExpression(
            expression,
            false,
            orderBy,
            false,
            includes);

        if (res.Count == 0)
        {
            return new()
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Product not found"
            };
        }

        var products = _mapper.Map<List<GetSingleProductForGrid>>(res.Items);

        return new()
        {
            Data = products,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = $"{res.Count} Products fetched successfully"
        };
    }

    public async Task<ResponseDTO> CreateProductAsync(ProductCreateDTO productCreateDTO)
    {
        var vendor = await _vendorReadRepository.GetByIdAsync(productCreateDTO.VendorId);
        if (vendor == null || vendor.IsDelete)
        {
            throw new NotFoundCustomException("Vendor not found");
        }

        Expression<Func<Product, bool>> includes = x => x.IsDelete == false && x.Name == productCreateDTO.Title;

        var productExist = await _productReadRepository.GetSingleByExpressionAsync(includes, false);

        if (productExist != null)
        {
            throw new DuplicateCustomException("Product already exist");
        }

        var product = _mapper.Map<Product>(productCreateDTO);

        await _productWriteRepository.AddAsync(product);

        await _productWriteRepository.SaveChangesAsync();

        ProductCreatedEvent productCreatedEvent = new()
        {
            CreatedUserId = product.WhoCreated,
            ProductId = product.Id,
            StockCount = 0,
            CreatedDate = product.CreateDate,
            //MainImage = productCreateDTO.MainImage,
        };

        await _publishEndpoint.Publish(productCreatedEvent);

        return new()
        {
            Errors = null,
            Message = "Product created successfully",
            Data = null,
            StatusCode = StatusCodes.Status201Created,
            IsSuccess = true
        };
    }

    public async Task<ResponseDTO> DeleteProductAsync(string id, string WhoDeleted)
    {
        var exsistProduct = await _productReadRepository.GetByIdAsync(id);

        if (exsistProduct == null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Product is not found",
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        var uri = $"{ServicesURL.StockServices_GetInStock}{id}";
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Stock service is unreachable",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var content = await response.Content.ReadAsAsync<ResponseDTO>();

        if (!content.IsSuccess)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Failed to retrieve stock data",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        var stockData = JsonConvert.DeserializeObject<StockData>(content.Data.ToString());
        if (stockData.StockCount > 0)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Product cannot be deleted because it is in stock",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        // Product exists in stock, proceed with deletion
        exsistProduct.IsDelete = true;
        exsistProduct.WhoDeleted = WhoDeleted;
        exsistProduct.DeleteDate = DateTime.UtcNow;

        _productWriteRepository.Update(exsistProduct);
        await _productWriteRepository.SaveChangesAsync();

        var deletedEvent = new ProductDeletedEvent()
        {
            DeletedUserId = WhoDeleted,
            ProductId = exsistProduct.Id,
            DeletedDate = DateTime.UtcNow
        };

        // Send delete event to RabbitMQ
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Stock_DeletedNewProductItemEventQueue}"));
        await sendEndpoint.Send(deletedEvent);

        return new()
        {
            IsSuccess = true,
            Message = "Product deleted successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDTO> UpdateProductAsync(ProductUpdateDTO product)
    {
        var exsistProduct = await _productReadRepository.GetByIdAsync(product.Id);

        if (exsistProduct == null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Product is not found",
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        _mapper.Map(product, exsistProduct);

        _productWriteRepository.Update(exsistProduct);
        //TODO: add product image update

        await _productWriteRepository.SaveChangesAsync();

        return new()
        {
            Data = null,
            IsSuccess = true,
            Message = "Product updated successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }
}
```


