using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.ReadRepositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
           await _context.Products.AddAsync(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id,CancellationToken cancellationToken)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        }
        public async Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids,
            CancellationToken cancellationToken)
        {
            return await _context.Products.Where(p => ids.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }


    }
}
