namespace NestNotification.API.Mapping;

public class EmailManagementProfile : Profile
{
    public EmailManagementProfile()
    {
        CreateMap<EmailLog, GetEmailLogDto>();
    }
}