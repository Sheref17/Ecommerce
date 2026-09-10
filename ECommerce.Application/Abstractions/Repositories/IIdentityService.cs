using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Repositories
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, int UserId, IEnumerable<string> Errors)>
            CreateUserAsync(string firstName,string lastName,string email,
            string password,CancellationToken cancellationToken);

        Task<(bool Succeeded, int UserId, IEnumerable<string> Errors)>ValidateCredentialsAsync(string email,
            string password,CancellationToken cancellationToken);

        Task<bool> AddToRoleAsync(int userId,string role,
            CancellationToken cancellationToken);
    }
}
