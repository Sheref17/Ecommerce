using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }

        string? Email { get; }

        string? FirstName { get; }

        string? LastName { get; }
    }
}
