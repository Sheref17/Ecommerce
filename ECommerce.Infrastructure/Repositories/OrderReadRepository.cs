using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderReadRepository : IOrderReadRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id,int userId, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(x => x.Id == id && x.UserId == userId)
                .Select(x => new OrderResponse(
                    x.Id,
                    x.UserId,
                    x.Status.ToString(),
                    x.CreatedAt,
                    x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    x.Items
                        .Select(i => new OrderItemResponse(
                            i.ProductId,
                            i.Quantity,
                            i.UnitPrice,
                            i.Quantity * i.UnitPrice))
                        .ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OrderResponse>> GetOrdersAsync(int userId,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new OrderResponse(
                    x.Id,
                    x.UserId,
                    x.Status.ToString(),
                    x.CreatedAt,
                    x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    x.Items.Select(i => new OrderItemResponse(
                        i.ProductId,
                        i.Quantity,
                        i.UnitPrice,
                        i.Quantity * i.UnitPrice))
                        .ToList()
                )).ToListAsync(cancellationToken);
        }
    }
}
