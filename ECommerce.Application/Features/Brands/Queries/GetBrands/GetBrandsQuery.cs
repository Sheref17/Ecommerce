using ECommerce.Application.Common;
using ECommerce.Application.Features.Brands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Queries.GetBrands
{
    public record GetBrandsQuery(BrandFilter filter)
        : IRequest<PagedResult<BrandResponse>>;
}
