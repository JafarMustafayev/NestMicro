namespace NestNotification.API.Mapping;

public class EmailTemplateProfile : Profile
{
    public EmailTemplateProfile()
    {
        CreateMap<CreateEmailTemplateDto, EmailTemplate>();

        CreateMap<UpdateEmailTemplateDto, EmailTemplate>()
            .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<DeleteEmailTemplateDto, EmailTemplate>()
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<EmailTemplate, GetEmailTemplateDto>();
    }
}