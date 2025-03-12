namespace NestNotification.API.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IEmailTemplateAttributeRepository _emailTemplateAttributeRepository;
    private readonly ILogger<EmailTemplateService> _logger;
    private readonly IMapper _mapper;

    public EmailTemplateService(
        IEmailTemplateRepository emailTemplateRepository,
        ILogger<EmailTemplateService> logger,
        IMapper mapper, IEmailTemplateAttributeRepository emailTemplateAttributeRepository)
    {
        _emailTemplateRepository = emailTemplateRepository;
        _logger = logger;
        _mapper = mapper;
        _emailTemplateAttributeRepository = emailTemplateAttributeRepository;
    }

    public ResponseDto GetEmailTemplates()
    {
        var res = _emailTemplateRepository.GetAll(false).Where(x => x != null).Cast<EmailTemplate>().ToList();

        var map = _mapper.Map<List<GetEmailTemplateDto>>(res);

        return new()
        {
            Data = map,
            Message = "Email templates fetched successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> GetEmailTemplateByIdAsync(string templateId)
    {
        var template = await _emailTemplateRepository.GetByExpressionAsync(x => x.Id == templateId);
        if (template == null)
        {
            throw new EntityNotFoundException($"Template with ID '{templateId}' not found");
        }

        return new()
        {
            Data = _mapper.Map<GetEmailTemplateDto>(template),
            Message = "Email template fetched successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> GetEmailTemplateByNameAsync(string templateName)
    {
        var template = await _emailTemplateRepository.GetByExpressionAsync(x => x.TemplateName == templateName);
        if (template == null)
        {
            throw new EntityNotFoundException($"Template with name '{templateName}' not found");
        }

        return new()
        {
            Data = _mapper.Map<GetEmailTemplateDto>(template),
            Message = "Email template fetched successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> CreateEmailTemplateAsync(CreateEmailTemplateDto templateDto)
    {
        var existingTemplate =
            await _emailTemplateRepository.GetByExpressionAsync(x => x.TemplateName == templateDto.TemplateName);
        if (existingTemplate != null)
        {
            throw new DuplicateEntityException($"Template with name '{templateDto.TemplateName}' already exists");
        }


        var map = _mapper.Map<EmailTemplate>(templateDto);

        await _emailTemplateRepository.AddAsync(map);
        await _emailTemplateRepository.SaveChangesAsync();

        if (templateDto.Attributes != null && templateDto.Attributes.Count > 0)
        {
            if (!templateDto.Body.Contains("{{{$"))
            {
                throw new InvalidStateException("Body must contain template attributes");
            }

            var newAttributes = new List<EmailTemplateAttribute>();
            foreach (var attribute in templateDto.Attributes)
            {
                var attributeMap = _mapper.Map<EmailTemplateAttribute>(attribute);
                attributeMap.TemplateId = map.Id;
                newAttributes.Add(attributeMap);
            }

            await _emailTemplateAttributeRepository.AddRangeAsync(newAttributes);
            await _emailTemplateAttributeRepository.SaveChangesAsync();
        }

        _logger.LogInformation("New email template '{TemplateName}' created", templateDto.TemplateName);

        return new()
        {
            Message = "Email template created successfully",
            StatusCode = StatusCodes.Status201Created,
            IsSuccess = true,
            Data = new
            {
                TemplateId = map.Id
            }
        };
    }

    public async Task<ResponseDto> UpdateEmailTemplateAsync(UpdateEmailTemplateDto templateDto)
    {
        var template = await _emailTemplateRepository.GetByIdAsync(templateDto.TemplateId);
        if (template == null)
        {
            throw new Exception($"Template with ID '{templateDto.TemplateId}' not found");
        }

        var map = _mapper.Map<EmailTemplate>(templateDto);

        _emailTemplateRepository.Update(map);
        await _emailTemplateRepository.SaveChangesAsync();
        _logger.LogInformation("Email template '{TemplateName}' updated", templateDto.TemplateName);

        return new()
        {
            Message = "Email template updated successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> DeleteEmailTemplateAsync(DeleteEmailTemplateDto templateDto)
    {
        var template = await _emailTemplateRepository.GetByIdAsync(templateDto.TemplateId);
        if (template == null)
        {
            throw new EntityNotFoundException($"Template with ID '{templateDto.TemplateId}' not found");
        }

        _mapper.Map(templateDto, template);

        _emailTemplateRepository.Update(template);
        await _emailTemplateRepository.SaveChangesAsync();
        _logger.LogInformation("Email template '{TemplateName}' deleted", template.TemplateName);

        return new()
        {
            Message = "Email template deleted successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }


    public ResponseDto GetTemplateAttributes(string templateId)
    {
        var attributes =
            _emailTemplateAttributeRepository.GetAllByExpression(
                x => x.TemplateId == templateId,
                false,
                x => x.AttributeName);

        if (attributes.Count == 0)
        {
            throw new EntityNotFoundException($"No attributes found for template with ID '{templateId}'");
        }

        var map = _mapper.Map<List<GetTemplateAttributeDto>>(attributes.Items);

        return new()
        {
            Data = map,
            Message = "Template attributes fetched successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> GetTemplateAttributeByIdAsync(string attributeId)
    {
        var attribute = await _emailTemplateAttributeRepository.GetByIdAsync(attributeId);

        if (attribute == null)
        {
            throw new EntityNotFoundException($"Attribute with ID '{attributeId}' not found");
        }

        var map = _mapper.Map<GetTemplateAttributeDto>(attribute);

        return new()
        {
            Data = map,
            Message = "Template attribute fetched successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> CreateTemplateAttributeAsync(CreateTemplateAttributeDto attributeDto)
    {
        if (attributeDto.TemplateId == null)
        {
            throw new InvalidStateException("Template ID is required");
        }

        var template = await _emailTemplateRepository.GetByIdAsync(attributeDto.TemplateId);

        if (template == null)
        {
            throw new EntityNotFoundException($"Template with ID '{attributeDto.TemplateId}' not found");
        }

        var attribute = _mapper.Map<EmailTemplateAttribute>(attributeDto);
        attribute.TemplateId = template.Id;
        await _emailTemplateAttributeRepository.AddAsync(attribute);
        await _emailTemplateAttributeRepository.SaveChangesAsync();

        return new()
        {
            StatusCode = StatusCodes.Status201Created,
            IsSuccess = true,
            Message = "Template attribute created successfully",
            Data = new
            {
                AttributeId = attribute.Id
            }
        };
    }

    public async Task<ResponseDto> UpdateTemplateAttributeAsync(UpdateTemplateAttributeDto attributeDto)
    {
        var attribute = await _emailTemplateAttributeRepository.GetByIdAsync(attributeDto.AttributeId);

        if (attribute == null)
        {
            throw new EntityNotFoundException($"Attribute with ID '{attributeDto.AttributeId}' not found");
        }

        _mapper.Map(attributeDto, attribute);
        _emailTemplateAttributeRepository.Update(attribute);
        await _emailTemplateAttributeRepository.SaveChangesAsync();

        return new()
        {
            Message = "Template attribute updated successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }

    public async Task<ResponseDto> DeleteTemplateAttributeAsync(DeleteTemplateAttributeDto attributeDto)
    {
        var attribute = await _emailTemplateAttributeRepository.GetByIdAsync(attributeDto.AttributeId);

        if (attribute == null)
        {
            throw new EntityNotFoundException($"Attribute with ID '{attributeDto.AttributeId}' not found");
        }

        _mapper.Map(attributeDto, attribute);
        _emailTemplateAttributeRepository.Update(attribute);
        await _emailTemplateAttributeRepository.SaveChangesAsync();

        return new()
        {
            Message = "Template attribute deleted successfully",
            StatusCode = StatusCodes.Status200OK,
            IsSuccess = true
        };
    }
}