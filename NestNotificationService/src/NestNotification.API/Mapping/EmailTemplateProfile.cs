namespace NestNotification.API.Mapping;

public class EmailTemplateProfile : Profile
{
    public EmailTemplateProfile()
    {
        CreateMap<CreateEmailTemplateDto, EmailTemplate>();

        CreateMap<UpdateEmailTemplateDto, EmailTemplate>()
            .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<EmailTemplate, GetEmailTemplateDto>();
    }
}