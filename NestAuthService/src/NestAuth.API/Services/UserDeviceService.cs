namespace NestAuth.API.Services;

public class UserDeviceService : IUserDeviceService
{
    public Task<IEnumerable<UserDevice>> GetUserDevicesAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> GetUserDeviceByIdAsync(string userId, string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> AddUserDeviceAsync(UserDevice userDevice)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> UpdateUserDeviceAsync(string deviceId, UserDevice updatedDevice)
    {
        throw new NotImplementedException();
    }

    public Task<UserDevice> BlockUserDeviceAsync(string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUserDeviceAsync(string userId, string deviceId)
    {
        throw new NotImplementedException();
    }
}