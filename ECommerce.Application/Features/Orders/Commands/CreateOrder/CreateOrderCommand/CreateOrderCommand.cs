using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder.CreateOrderCommand
{
    public record CreateOrderItemRequest( int ProductId,int Quantity);

    public record CreateOrderCommand(IReadOnlyList<CreateOrderItemRequest> Items) 
        : IRequest<int>;
}
