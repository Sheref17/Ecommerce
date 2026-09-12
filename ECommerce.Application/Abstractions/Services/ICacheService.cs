using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key,CancellationToken cancellationToken);

        Task SetAsync(string key,string value,TimeSpan expiration,
            CancellationToken cancellationToken);

        Task RemoveAsync(string key,CancellationToken cancellationToken);
    }
}
