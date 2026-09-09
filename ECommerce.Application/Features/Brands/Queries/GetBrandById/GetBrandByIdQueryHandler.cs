using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Brands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandResponse?>
    {
        private readonly IBrandReadRepository _brandReadRepository;

        public GetBrandByIdQueryHandler(IBrandReadRepository brandReadRepository)
        {
            _brandReadRepository = brandReadRepository;
        }

        public async Task<BrandResponse?> Handle( GetBrandByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _brandReadRepository.GetByIdAsync(request.Id,cancellationToken);
        }
    }
}
