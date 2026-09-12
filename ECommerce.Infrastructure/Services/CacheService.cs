using ECommerce.Application.Abstractions.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<string?> GetAsync(string key,CancellationToken cancellationToken)
        {
            var value = await _database.StringGetAsync(key);

            return value.IsNull ? null : value.ToString();
        }

        public async Task SetAsync(string key,string value,TimeSpan expiration,CancellationToken cancellationToken)
        {
            await _database.StringSetAsync(key,value,expiration);
        }

        public async Task RemoveAsync(string key,CancellationToken cancellationToken)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
