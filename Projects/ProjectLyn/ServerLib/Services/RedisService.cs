using StackExchange.Redis;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace ServerLib
{
    public class RedisService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = connectionMultiplexer.GetDatabase();
        }

        // 키밸류
        public async Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _database.StringSetAsync(key, value, expiry);
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        // JSON 직렬화,역직렬화
        public async Task<bool> SetObjectAsync<T>(string key, T obj, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(obj);
            return await SetAsync(key, json, expiry);
        }

        public async Task<T?> GetObjectAsync<T>(string key)
        {
            var json = await GetAsync(key);
            if (string.IsNullOrEmpty(json))
                return default(T);

            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (JsonException)
            {
                return default(T);
            }
        }

        // Hash
        public async Task<bool> HashSetAsync(string key, string field, string value)
        {
            return await _database.HashSetAsync(key, field, value);
        }

        public async Task<string?> HashGetAsync(string key, string field)
        {
            var value = await _database.HashGetAsync(key, field);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<HashEntry[]> HashGetAllAsync(string key)
        {
            return await _database.HashGetAllAsync(key);
        }

        // List
        public async Task<long> ListPushAsync(string key, string value)
        {
            return await _database.ListLeftPushAsync(key, value);
        }

        public async Task<string?> ListPopAsync(string key)
        {
            var value = await _database.ListRightPopAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<string[]> ListRangeAsync(string key, long start = 0, long stop = -1)
        {
            var values = await _database.ListRangeAsync(key, start, stop);
            return values.Select(v => v.ToString()).ToArray();
        }

        // Set
        public async Task<bool> SetAddAsync(string key, string value)
        {
            return await _database.SetAddAsync(key, value);
        }

        public async Task<bool> SetContainsAsync(string key, string value)
        {
            return await _database.SetContainsAsync(key, value);
        }

        public async Task<string[]> SetMembersAsync(string key)
        {
            var values = await _database.SetMembersAsync(key);
            return values.Select(v => v.ToString()).ToArray();
        }

        // 연결
        public bool IsConnected()
        {
            return _connectionMultiplexer.IsConnected;
        }

        public string GetConnectionStatus()
        {
            return _connectionMultiplexer.GetStatus();
        }
    }
}
