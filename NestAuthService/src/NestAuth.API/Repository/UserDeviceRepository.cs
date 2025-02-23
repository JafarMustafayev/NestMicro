namespace NestAuth.API.Repository;

public class UserDeviceRepository : Repository<UserDevice>, IUserDeviceRepository
{
    public UserDeviceRepository(AppDbContext context) : base(context)
    {
    }
}