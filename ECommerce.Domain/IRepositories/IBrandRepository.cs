using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IBrandRepository
    {
        Task AddAsync(Brand brand, CancellationToken cancellationToken);
        Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Brand?> GetByNameAsync(string name , CancellationToken cancellationToken = default);
    }
}
