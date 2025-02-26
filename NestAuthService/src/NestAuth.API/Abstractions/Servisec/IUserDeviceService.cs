namespace NestAuth.API.Abstractions.Servisec;

public interface IUserDeviceService
{
    IQueryable<UserDevice> GetUserDevicesAsync(string userId);

    Task<UserDevice> GetUserDeviceByIdAsync(string userId, string deviceId);

    Task<UserDevice> RegisterDeviceAsync(RegisterDeviceDto device);

    Task<UserDevice> UpdateUserDeviceAsync(string deviceId, UserDevice updatedDevice);

    Task<bool> UpdateLastLoginAsync(string deviceId, string ipAddress);

    Task<UserDevice> BlockDeviceAsync(string deviceId);

    Task<bool> UnblockDeviceAsync(string deviceId);

    Task<bool> RemoveDeviceAsync(string userId, string deviceId);
}