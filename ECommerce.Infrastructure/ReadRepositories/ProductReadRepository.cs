using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Common;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Features.Products.Queries.GetProducts;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.ReadRepositories
{
    public class ProductReadRepository : IProductReadRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ProductResponse>> GetAllProudcts(ProductFilter filter, CancellationToken cancellationToken)
        {
            var specification = new ProductSpecification(filter);

            var baseQuery = _context.Products.AsNoTracking()
                .Where( p =>p.IsActive == true);

            var filteredQuery = SpecificationEvaluator.GetQuery(baseQuery, specification 
                , applyPaging:false);

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var pagedQuery = SpecificationEvaluator.GetQuery(filteredQuery, specification);

            var items = await pagedQuery
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price.Amount,
                    p.Price.Currency,
                    p.Stock,
                    _context.Categories.Where(c => c.Id == p.CategoryId)
                    .Select(c => c.Name).FirstOrDefault()! ,
                     _context.Brands.Where(b => b.Id == p.BrandId)
                     .Select(b => b.Name).FirstOrDefault()!
                    ))
                .ToListAsync(cancellationToken);

            return new PagedResult<ProductResponse>(items, filter.PageNumber, filter.PageSize, totalCount);

        }

        public async Task<ProductResponse?> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Products.AsNoTracking()
                .Where(p=>p.Id == id && p.IsActive == true)
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price.Amount,
                    p.Price.Currency,
                    p.Stock,
                    _context.Categories.Where(c => c.Id == p.CategoryId)
                    .Select(c => c.Name).FirstOrDefault()! ,
                      _context.Brands.Where(b => b.Id == p.BrandId)
                     .Select(b => b.Name).FirstOrDefault()!
                    ))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
