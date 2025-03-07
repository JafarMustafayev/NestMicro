namespace NestNotification.API.Mapping;

public class EmailSendingProfile : Profile
{
    public EmailSendingProfile()
    {
        CreateMap<MailRequest, EmailLog>()
            .ForMember(dest => dest.IsHtml, opt => opt.MapFrom(src => src.Body.Contains("<html>"))).ReverseMap();

        CreateMap<SendEmailDto, MailRequest>();

        CreateMap<SendEmailDto, EmailQueue>();

        CreateMap<SendTemplatedEmailDto, EmailQueue>();

        CreateMap<SendBulkEmailDto, EmailQueue>();

        CreateMap<SendScheduledEmailDto, EmailQueue>()
            .ForMember(dest => dest.ScheduledAt, opt => opt.MapFrom(src => EmailPriority.Scheduled));

        CreateMap<SendScheduledTemplateEmailDto, EmailQueue>()
            .ForMember(dest => dest.ScheduledAt, opt => opt.MapFrom(src => EmailPriority.Scheduled));
    }
}