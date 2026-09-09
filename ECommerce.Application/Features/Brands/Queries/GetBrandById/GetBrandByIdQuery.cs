using ECommerce.Application.Features.Brands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Queries.GetBrandById
{
    public record GetBrandByIdQuery(int Id) : IRequest<BrandResponse?>;
}
