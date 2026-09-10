using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order,CancellationToken cancellationToken)
        {
            await _context.Orders.AddAsync(order,cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Orders.Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        }
    }
}
