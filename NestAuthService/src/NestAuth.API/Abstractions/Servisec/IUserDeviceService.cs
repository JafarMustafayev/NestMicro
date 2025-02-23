namespace NestAuth.API.Abstractions.Servisec;

public interface IUserDeviceService
{
    Task<IEnumerable<UserDevice>> GetUserDevicesAsync(string userId);

    Task<UserDevice> GetUserDeviceByIdAsync(string userId, string deviceId);

    Task<UserDevice> AddUserDeviceAsync(UserDevice userDevice);

    Task<UserDevice> UpdateUserDeviceAsync(string deviceId, UserDevice updatedDevice);

    Task<UserDevice> BlockUserDeviceAsync(string deviceId);

    Task<bool> DeleteUserDeviceAsync(string userId, string deviceId);
}