using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.UpdateBasketItemQuantity
{
    public record UpdateBasketItemQuantityCommand(Guid ProductId,int Quantity)
        : IRequest;
}
