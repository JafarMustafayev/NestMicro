namespace NestStorage.API.Abstractions.Services;

public interface IStorageConfigurationService
{
    public void GetStorageSettingsAsync();
    public void GetCloudProviderSettingsAsync();

    public void GetSyncSettingsAsync();
    //public void UpdateStorageSettingsAsync(StorageSettings settings);
}