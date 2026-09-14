using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(Guid id , string name , string description ,
        decimal price , string Currency) : IRequest;
    
}
