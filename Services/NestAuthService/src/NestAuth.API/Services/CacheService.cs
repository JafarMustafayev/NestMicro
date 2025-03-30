namespace NestAuth.API.Services;

public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _redis;
    private readonly string _redisInstanceName = Configurations.GetConfiguratinValue<string>("RedisServer", "RedisInstanceName");

    public CacheService(
        ILogger<CacheService> logger,
        IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var prefixedKey = GetPrefixedKey(key);
        try
        {
            var json = JsonConvert.SerializeObject(value);
            return await _database.StringSetAsync(prefixedKey, json, expiry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache write error: {Key}", prefixedKey);
            return false;
        }
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var prefixedKey = GetPrefixedKey(key);
        try
        {
            var value = await _database.StringGetAsync(prefixedKey);
            return value.HasValue
                ? JsonConvert.DeserializeObject<T>(value)
                : default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache read error: {Key}", prefixedKey);
            return default;
        }
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        var prefixedKey = GetPrefixedKey(key);
        try
        {
            return await _database.KeyExistsAsync(prefixedKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in key verification: {Key}", prefixedKey);
            return false;
        }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        var prefixedKey = GetPrefixedKey(key);
        try
        {
            return await _database.KeyDeleteAsync(prefixedKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting key: {Key}", prefixedKey);
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern = "*")
    {
        var prefixedPattern = GetPrefixedKey(pattern);
        var keys = new List<string>();

        try
        {
            foreach (var endpoint in _redis.GetEndPoints())
            {
                var server = _redis.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: prefixedPattern))
                {
                    var tes = key.ToString().Replace(_redisInstanceName + "_", "");
                    keys.Add(key.ToString().Replace(_redisInstanceName + "_", ""));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for key: {Pattern}", prefixedPattern);
        }

        return keys;
    }

    public async Task<IEnumerable<T>> GetAllDatasAsync<T>(string pattern = "*")
    {
        var prefixedPattern = GetPrefixedKey(pattern);
        var datas = new List<T>();

        try
        {
            var redisKeys = new List<RedisKey>();

            foreach (var endpoint in _redis.GetEndPoints())
            {
                var server = _redis.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: prefixedPattern))
                {
                    redisKeys.Add(key);
                }
            }

            if (redisKeys.Count > 0)
            {
                var values = await _database.StringGetAsync(redisKeys.ToArray());
                foreach (var value in values)
                {
                    if (value.HasValue)
                    {
                        try
                        {
                            var json = value.ToString();
                            if (json.StartsWith("["))
                            {
                                var items = JsonConvert.DeserializeObject<List<T>>(json);
                                datas.AddRange(items);
                            }
                            else
                            {
                                var item = JsonConvert.DeserializeObject<T>(json);
                                datas.Add(item);
                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogWarning(ex, "Deserialization error for JSON: {Json}", value);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading data: {Pattern}", prefixedPattern);
        }

        return datas;
    }

    public async Task<IDictionary<string, T>> GetAllByKeysAsync<T>(IEnumerable<string> keys)
    {
        var prefixedKeys = keys.Select(k => GetPrefixedKey(k)).ToArray();
        var result = new Dictionary<string, T>();

        try
        {
            var values = await _database.StringGetAsync(prefixedKeys.Select(k => (RedisKey)k).ToArray());

            for (int i = 0; i < prefixedKeys.Length; i++)
            {
                if (values[i].HasValue)
                {
                    var originalKey = keys.ElementAt(i);
                    result[originalKey] = JsonConvert.DeserializeObject<T>(values[i]);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving multiple data");
        }

        return result;
    }

    public async Task SetAllAsync<T>(IDictionary<string, T> keyValuePairs, TimeSpan? expiry = null)
    {
        var prefixedPairs = keyValuePairs
            .Select(kv => new KeyValuePair<RedisKey, RedisValue>(
                GetPrefixedKey(kv.Key),
                JsonConvert.SerializeObject(kv.Value))
            ).ToArray();

        try
        {
            await _database.StringSetAsync(prefixedPairs);

            if (expiry.HasValue)
            {
                foreach (var prefixedKey in prefixedPairs.Select(p => p.Key))
                {
                    await _database.KeyExpireAsync(prefixedKey, expiry.Value);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing too much data");
        }
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<T> factory, TimeSpan? expiry = null)
    {
        var prefixedKey = GetPrefixedKey(key);
        try
        {
            var cachedValue = await GetAsync<T>(prefixedKey);
            if (cachedValue != null) return cachedValue;

            var newValue = factory();
            await SetAsync(prefixedKey, newValue, expiry);
            return newValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetOrAdd error: {Key}", prefixedKey);
            return factory();
        }
    }

    public async Task<bool> SetExpirationAsync(string key, TimeSpan expiry)
    {
        var prefixedKey = GetPrefixedKey(key);
        try
        {
            return await _database.KeyExpireAsync(prefixedKey, expiry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating TTL: {Key}", key);
            return false;
        }
    }

    public async Task ClearAsync()
    {
        var keys = await GetAllKeysAsync();
        foreach (var key in keys)
        {
            await RemoveAsync(key);
        }
    }

    private string GetPrefixedKey(string? key) => $"{_redisInstanceName}__{key}";
}