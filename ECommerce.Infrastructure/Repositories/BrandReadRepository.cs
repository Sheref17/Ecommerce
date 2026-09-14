using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Common;
using ECommerce.Application.Features.Brands.Dtos;
using ECommerce.Application.Features.Brands.Queries.GetBrands;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class BrandReadRepository : IBrandReadRepository
    {
        private readonly ApplicationDbContext _context;
        public BrandReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<BrandResponse>> GetAllAsync(BrandFilter filter, CancellationToken cancellationToken)
        {
            var query = _context.Brands.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Name.Contains(filter.Search));

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query.OrderBy(x => x.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new BrandResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.IsActive))
        .ToListAsync(cancellationToken);

            return new PagedResult<BrandResponse>(items,filter.PageNumber,filter.PageSize,totalCount);
        }

        public async Task<BrandResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Brands.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new BrandResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
