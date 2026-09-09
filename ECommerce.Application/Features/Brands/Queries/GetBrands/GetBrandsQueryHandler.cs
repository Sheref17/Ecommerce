using ECommerce.Application.Abstractions.Repositories;
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
    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery,PagedResult<BrandResponse>>
    {
        private readonly IBrandReadRepository _brandReadRepository;
        public GetBrandsQueryHandler(IBrandReadRepository brandReadRepository)
        {
            _brandReadRepository = brandReadRepository;
        }
        public async Task<PagedResult<BrandResponse>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            return await _brandReadRepository.GetAllAsync( request.filter,cancellationToken);
        }
    }
}
