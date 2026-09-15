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
    public class BasketRepository : IBasketRepository
    {
        private readonly ApplicationDbContext _context;

        public BasketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Basket basket,CancellationToken cancellationToken)
        {
            await _context.Baskets.AddAsync(basket,cancellationToken);
        }

        public async Task<Basket?> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken)
        {
            return await _context.Baskets
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId,cancellationToken);
        }

        public Task DeleteAsync(Basket basket,CancellationToken cancellationToken)
        {
            _context.Baskets.Remove(basket);

            return Task.CompletedTask;
        }

        public Task RemoveItemAsync(BasketItem item,CancellationToken cancellationToken)
        {
            _context.BasketItems.Remove(item);

            return Task.CompletedTask;
        }
    }
}
