namespace NestAuth.API.Services;

public class UserDeviceService : IUserDeviceService
{
    private readonly IUserDeviceRepository _repository;

    public UserDeviceService(IUserDeviceRepository repository)
    {
        _repository = repository;
    }

    public IQueryable<UserDevice> GetUserDevicesAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> GetUserDeviceByIdAsync(string userId, string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> RegisterDeviceAsync(RegisterDeviceDto device)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> UpdateUserDeviceAsync(string deviceId, UserDevice updatedDevice)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateLastLoginAsync(string deviceId, string ipAddress)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> BlockDeviceAsync(string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnblockDeviceAsync(string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveDeviceAsync(string userId, string deviceId)
    {
        throw new NotImplementedException();
    }
}