using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Repositories
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(int userId,CancellationToken cancellationToken);
    }
}
