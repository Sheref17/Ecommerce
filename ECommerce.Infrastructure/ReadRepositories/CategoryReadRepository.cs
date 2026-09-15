using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Common;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.Application.Features.Categories.Queries.GetCategories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.ReadRepositories
{
    public class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CategoryResponse>> GetAllAsync(CategoryFilter filter,
            CancellationToken cancellationToken)
        {
            var query = _context.Categories
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Name.Contains(filter.Search));
        

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
          

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new CategoryResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.IsActive))
                .ToListAsync(cancellationToken);

            return new PagedResult<CategoryResponse>(items,filter.PageNumber,filter.PageSize,totalCount);
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id,CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new CategoryResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
