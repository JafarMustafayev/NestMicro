namespace NestNotification.API.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly ILogger<EmailTemplateService> _logger;
    private readonly IMapper _mapper;

    public EmailTemplateService(
        IEmailTemplateRepository emailTemplateRepository,
        ILogger<EmailTemplateService> logger,
        IMapper mapper)
    {
        _emailTemplateRepository = emailTemplateRepository;
        _logger = logger;
        _mapper = mapper;
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

    public async Task<ResponseDto> AddEmailTemplateAsync(CreateEmailTemplateDto templateDto)
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
}