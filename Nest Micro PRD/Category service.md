```
namespace NestProduct.Persistence.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ISubCategoryReadRepository _subCategoryReadRepository;
    private readonly ICategoryWriteRepository _categoryWriteRepository;
    private readonly ISubCategoryWriteRepository _subCategoryWriteRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryReadRepository categoryReadRepository,
        ISubCategoryReadRepository subCategoryReadRepository,
        ICategoryWriteRepository categoryWriteRepository,
        ISubCategoryWriteRepository subCategoryWriteRepository,
        IMapper mapper)
    {
        _categoryReadRepository = categoryReadRepository;
        _subCategoryReadRepository = subCategoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
        _subCategoryWriteRepository = subCategoryWriteRepository;
        _mapper = mapper;
    }

    public async Task<ResponseDTO> GetAllCategories(int page)
    {
        Expression<Func<Category, object>> orderBy = x => x.Name;
        Expression<Func<Category, bool>> expression = x => x.IsActive;

        var res = _categoryReadRepository.GetAllByExpression(
            expression,
            page, 20,
            false,
            orderBy, false);

        var categories = _mapper.Map<List<GetCategoriesDTO>>(res);

        var count = await _categoryReadRepository.CountAsync(expression);
        return new()
        {
            IsSuccess = true,
            Data = categories,
            Message = $"{count} Categories fetched",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public ResponseDTO GetAllCategories()
    {
        Expression<Func<Category, object>> orderBy = x => x.Name;

        var res = _categoryReadRepository.GetAll(
            false,
            orderBy, false);

        var categories = _mapper.Map<List<GetCategoriesDTO>>(res);

        return new()
        {
            IsSuccess = true,
            Data = categories,
            Message = $"{categories.Count} Categories fetched",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> GetCategoryByIdAsync(string id)
    {
        var includes = new List<Expression<Func<Category, object>>>
        {
            x => x.SubCategories
        };

        var res = await _categoryReadRepository.GetByIdAsync(id, false, includes);
        if (res == null)
        {
            return new()
            {
                StatusCode = 404,
                Message = "Category not found"
            };
        }

        var category = _mapper.Map<GetSingleCategoryDTO>(res);

        return new()
        {
            Data = category,
            Errors = null,
            IsSuccess = true,
            Message = "Category found",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public ResponseDTO SearchCategories(string query)
    {
        Expression<Func<Category, bool>> expression = x => x.Name.ToLower().Contains(query.ToLower());
        Expression<Func<Category, object>> orderBy = x => x.Name;

        var res = _categoryReadRepository.GetAllByExpression(
            expression,
            false,
            orderBy, false);

        var categories = _mapper.Map<List<GetCategoriesDTO>>(res);

        return new()
        {
            IsSuccess = true,
            Data = categories,
            Message = $"{categories.Count} Categories fetched",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> CreateCategoryAsync(CreateCategoryDTO categoryDTO)
    {
        Expression<Func<Category, bool>> expression = x => x.Name.ToLower().Contains(categoryDTO.Name.ToLower());

        var categoryDb = await _categoryReadRepository.AnyAsync(expression);

        if (categoryDb)
        {
            return new()
            {
                StatusCode = 400,
                Message = "Category already exists",
                IsSuccess = false,
            };
        }

        var category = _mapper.Map<Category>(categoryDTO);

        category.CreateDate = DateTime.UtcNow;

        await _categoryWriteRepository.AddAsync(category);
        await _categoryWriteRepository.SaveChangesAsync();

        return new()
        {
            Data = null,
            Errors = null,
            IsSuccess = true,
            Message = "Category created",
            StatusCode = StatusCodes.Status201Created,
        };
    }

    public async Task<ResponseDTO> UpdateCategoryAsync(UpdateCategoryDTO categoryDTO)
    {
        var categoryDb = await _categoryReadRepository.GetByIdAsync(categoryDTO.Id);

        if (categoryDb == null)
        {
            return new()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Category not found",
                IsSuccess = false,
            };
        }

        Expression<Func<Category, bool>> expression = x => x.Name.ToLower().Contains(categoryDTO.Name.ToLower());

        var any = await _categoryReadRepository.AnyAsync(expression);

        if (any)
        {
            return new()
            {
                StatusCode = 400,
                Message = "Category already exists",
                IsSuccess = false,
            };
        }

        _mapper.Map(categoryDTO, categoryDb);
        _categoryWriteRepository.Update(categoryDb);
        await _categoryWriteRepository.SaveChangesAsync();

        return new()
        {
            Data = null,
            Errors = null,
            IsSuccess = true,
            Message = "Category updated",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> DeleteCategoryAsync(string Id, string whoDeleted)
    {
        var categoryDb = await _categoryReadRepository.GetByIdAsync(Id);

        if (categoryDb == null)
        {
            return new()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Category not found",
                IsSuccess = false,
            };
        }

        categoryDb.IsDelete = true;
        categoryDb.WhoDeleted = whoDeleted;
        categoryDb.DeleteDate = DateTime.UtcNow;

        _categoryWriteRepository.Update(categoryDb);
        await _categoryWriteRepository.SaveChangesAsync();

        return new()
        {
            IsSuccess = true,
            Message = "Category deleted",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public ResponseDTO GetAllSubCategories()
    {
        var res = _subCategoryReadRepository.GetAll(
            false,
            x => x.Name, true);

        var categories = _mapper.Map<List<GetSubCategoriesDTO>>(res);

        return new()
        {
            IsSuccess = true,
            Data = categories,
            Message = $"{categories.Count} Categories fetched",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> GetAllSubCategories(int page)
    {
        Expression<Func<SubCategory, object>> orderBy = x => x.Name;
        Expression<Func<SubCategory, bool>> expression = x => x.IsActive;

        var res = _subCategoryReadRepository.GetAllByExpression(
            expression,
            page, 20,
            false,
            orderBy, false);

        var count = await _subCategoryReadRepository.CountAsync(expression);

        var categories = _mapper.Map<List<GetSubCategoriesDTO>>(res);

        return new()
        {
            IsSuccess = true,
            Data = categories,
            Message = $"{count} Categories fetched",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> GetSubCategoryByIdAsync(string id)
    {
        var includes = new List<Expression<Func<SubCategory, object>>>
        {
            x => x.Category
        };

        var res = await _subCategoryReadRepository.GetByIdAsync(id, false, includes);
        if (res == null)
        {
            return new()
            {
                StatusCode = 404,
                Message = "SubCategory not found"
            };
        }

        var category = _mapper.Map<GetSingleSubCategoryDTO>(res);

        return new()
        {
            Data = category,
            Errors = null,
            IsSuccess = true,
            Message = "SubCategory found",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public ResponseDTO GetSubCategoriesByCategoryId(string categoryId)
    {
        var includes = new List<Expression<Func<SubCategory, object>>>
        {
            x => x.Category
        };

        var res = _subCategoryReadRepository.GetAllByExpression(
            x => x.CategoryId == categoryId,
            false,
            x => x.Name,
            false,
            includes);
        if (res.Item2 == 0)
        {
            return new()
            {
                StatusCode = 404,
                Message = "SubCategory not found"
            };
        }

        var category = _mapper.Map<List<GetSingleSubCategoryDTO>>(res.Item1);

        return new()
        {
            Data = category,
            Errors = null,
            IsSuccess = true,
            Message = $"{res.Item2} subcategory founded",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public ResponseDTO SearchSubCategories(string query)
    {
        List<Expression<Func<SubCategory, object>>> includes = new()
        {
            x => x.Category
        };
        var res = _subCategoryReadRepository.GetAllByExpression(
             x => x.Name.Contains(query),
             false,
             x => x.Name, false,
             includes);

        var categories = _mapper.Map<List<GetSubCategoriesDTO>>(res);

        return new()
        {
            IsSuccess = true,
            Data = categories,
            Message = $"{categories.Count} Categories fetched",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> CreateSubCategoryAsync(CreateSubCategoryDTO subCategoryDTO)
    {
        Expression<Func<SubCategory, bool>> expression = x => x.Name.ToLower().Contains(subCategoryDTO.Name.ToLower());

        var any = await _subCategoryReadRepository.AnyAsync(expression);

        if (any)
        {
            return new()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Subcategory already exists",
                IsSuccess = false,
            };
        }

        any = await _categoryReadRepository.AnyAsync(x => x.Id == subCategoryDTO.CategoryId);

        if (!any)
        {
            return new()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Category not found",
                IsSuccess = false,
            };
        }

        var subCategory = _mapper.Map<SubCategory>(subCategoryDTO);
        await _subCategoryWriteRepository.AddAsync(subCategory);
        await _subCategoryWriteRepository.SaveChangesAsync();

        return new()
        {
            IsSuccess = true,
            Message = "Subcategory created",
            StatusCode = StatusCodes.Status201Created,
        };
    }

    public async Task<ResponseDTO> UpdateSubCategoryAsync(UpdateSubCategoryDto subCategoryDTO)
    {
        var categoryDb = await _subCategoryReadRepository.GetByIdAsync(subCategoryDTO.Id);

        if (categoryDb == null)
        {
            return new()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Subcategory not found",
                IsSuccess = false,
            };
        }

        Expression<Func<SubCategory, bool>> expression = x => x.Name.ToLower().Contains(subCategoryDTO.Name.ToLower());

        var any = await _subCategoryReadRepository.AnyAsync(expression);

        if (any)
        {
            return new()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Subcategory already exists",
                IsSuccess = false,
            };
        }

        any = await _categoryReadRepository.AnyAsync(x => x.Id == subCategoryDTO.CategoryId);

        if (!any)
        {
            return new()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Category not found",
                IsSuccess = false,
            };
        }

        _mapper.Map(subCategoryDTO, categoryDb);
        _subCategoryWriteRepository.Update(categoryDb);
        await _subCategoryWriteRepository.SaveChangesAsync();

        return new()
        {
            IsSuccess = true,
            Message = "Subcategory updated",
            StatusCode = StatusCodes.Status200OK,
        };
    }

    public async Task<ResponseDTO> DeleteSubCategoryAsync(string Id, string whoDeleted)
    {
        var categoryDb = await _subCategoryReadRepository.GetByIdAsync(Id);

        if (categoryDb == null)
        {
            return new()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Category not found",
                IsSuccess = false,
            };
        }

        categoryDb.IsDelete = true;
        categoryDb.WhoDeleted = whoDeleted;
        categoryDb.DeleteDate = DateTime.UtcNow;

        _subCategoryWriteRepository.Update(categoryDb);
        await _subCategoryWriteRepository.SaveChangesAsync();

        return new()
        {
            IsSuccess = true,
            Message = "Category deleted",
            StatusCode = StatusCodes.Status200OK,
        };
    }
}
```