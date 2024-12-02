using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace ClaudeSearch.Infrastructure.Caching
{
    public interface ICacheService
    {
        T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        void Remove(string key);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
            _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        public T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null)
        {
            if (_cache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var value = factory();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
            };

            _cache.Set(key, value, cacheOptions);
            return value;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_cache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var lockObj = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
            await lockObj.WaitAsync();

            try
            {
                if (_cache.TryGetValue(key, out cachedValue))
                {
                    return cachedValue;
                }

                var value = await factory();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
                };

                _cache.Set(key, value, cacheOptions);
                return value;
            }
            finally
            {
                lockObj.Release();
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}