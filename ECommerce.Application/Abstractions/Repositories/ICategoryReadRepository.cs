using ECommerce.Application.Common;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.Application.Features.Categories.Queries.GetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Repositories
{
    public interface ICategoryReadRepository
    {
        public Task<PagedResult<CategoryResponse>> GetAllAsync(CategoryFilter filter
            , CancellationToken cancellationToken);
        public Task<CategoryResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
