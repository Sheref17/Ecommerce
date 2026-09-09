using ECommerce.Application.Common;
using ECommerce.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery(ProductFilter Filter) : IRequest<PagedResult<ProductResponse>>;
   
}
