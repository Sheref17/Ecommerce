using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id,CancellationToken cancellationToken);
        Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids,
            CancellationToken cancellationToken);



    }
}
