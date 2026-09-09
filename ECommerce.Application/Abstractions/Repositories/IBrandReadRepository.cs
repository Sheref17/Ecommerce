using ECommerce.Application.Common;
using ECommerce.Application.Features.Brands.Dtos;
using ECommerce.Application.Features.Brands.Queries.GetBrands;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Repositories
{
    public interface IBrandReadRepository
    {
        Task<PagedResult<BrandResponse>> GetAllAsync(BrandFilter filter,
            CancellationToken cancellationToken);

        Task<BrandResponse?> GetByIdAsync(int id,CancellationToken cancellationToken);
    }
}
