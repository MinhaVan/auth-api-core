using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Domain.Interfaces.Repositories;
using Newtonsoft.Json;

namespace Auth.Data.Extensions;


public static class RepositoryExtension
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
                var jsonData = JsonConvert.SerializeObject(data);
                await redisCache.SetAsync(fixedKey, jsonData, expirationInMinutes);
            }

            return data;
        }
        catch
        {
            // Em caso de falha, tenta novamente a função de dados
            return await getDataFunc();
        }
    }

}
