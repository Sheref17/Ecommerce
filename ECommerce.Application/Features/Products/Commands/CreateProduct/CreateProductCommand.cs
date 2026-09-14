using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name,string Description, decimal Price,string Currency,
        int Stock, Guid CategoryId , Guid BrandId) : IRequest<Guid>;

}
