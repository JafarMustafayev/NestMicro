namespace NestAuth.API.Abstractions.Servisec;

public interface ICacheService
{
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    // Redis-ə məlumat yazır. expiry: cache müddəti (optional)

    Task<T> GetAsync<T>(string key);
    // Key-ə uyğun value qaytarır (null olarsa default)

    Task<bool> ContainsKeyAsync(string key);
    // Verilən key'in cache-də olub-olmadığını yoxlayır

    Task<bool> RemoveAsync(string key);
    // Cache-dən spesifik data silmək

    Task<IEnumerable<string>> GetAllKeysAsync(string pattern = "*");
    // Regex pattern-a uyğun bütün key'leri listləyir

    Task<IDictionary<string, T>> GetAllByKeysAsync<T>(IEnumerable<string> keys);
    // Çoxlu key üzrə eyni anda data çəkərkən istifadə (mget kimi)

    Task<IEnumerable<T>> GetAllDatasAsync<T>(string pattern = "*");
    //Butun datalari getirir

    Task SetAllAsync<T>(IDictionary<string, T> keyValuePairs, TimeSpan? expiry = null);
    // Çoxlu data yazarkən istifadə (mset kimi)

    Task<T> GetOrAddAsync<T>(string key, Func<T> factory, TimeSpan? expiry = null);
    // Cache stampede problemi üçün atomic əməliyyat (thread-safe)

    Task<bool> SetExpirationAsync(string key, TimeSpan expiry);
    // Artıq mövcud olan key'in TTL-ini dəyişmək

    Task ClearAsync();
    // Redis-də bütün key'leri silir (dikkatlə!)
}