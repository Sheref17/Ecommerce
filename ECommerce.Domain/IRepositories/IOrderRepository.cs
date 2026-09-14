using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order,CancellationToken cancellationToken);

        Task<Order?> GetByIdAsync(Guid id,CancellationToken cancellationToken);
    }
}
