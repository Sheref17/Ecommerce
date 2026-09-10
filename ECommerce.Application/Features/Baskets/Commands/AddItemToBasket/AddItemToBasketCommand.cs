using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.AddItemToBasket
{
    public record AddItemToBasketCommand(int UserId,int ProductId,int Quantity) : IRequest;
}
