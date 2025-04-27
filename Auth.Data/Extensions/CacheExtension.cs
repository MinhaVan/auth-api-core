using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Domain.Interfaces.Repositories;

namespace Auth.Data.Extensions;

public static class CacheExtension
{
    public static async Task<T> TryGetAsync<T>(
        this IRedisRepository redisCache,
        string key,
        Func<Task<T>> getDataFunc,
        int expirationInMinutes = 15
    ) where T : class
    {
        try
        {
            var fixedKey = "auth:" + key;
            var cached = await redisCache.GetAsync<T>(fixedKey);
            if (cached != null && !EqualityComparer<T>.Default.Equals(cached, default))
            {
                return cached;
            }

            var data = await getDataFunc();
            if (data != null && !EqualityComparer<T>.Default.Equals(data, default))
            {
                await redisCache.SetAsync(fixedKey, data, expirationInMinutes);
            }

            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao tentar obter ou setar os dados no cache: " + ex.Message);
            return await getDataFunc();
        }
    }

}
