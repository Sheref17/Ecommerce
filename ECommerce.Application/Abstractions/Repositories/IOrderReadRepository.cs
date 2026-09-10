using ECommerce.Application.Features.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Repositories
{
    public interface IOrderReadRepository
    {
        Task<OrderResponse?> GetByIdAsync(int id, int userId ,
            CancellationToken cancellationToken);
        Task<IReadOnlyList<OrderResponse>> GetOrdersAsync(int userId,
            CancellationToken cancellationToken);
    }
}
