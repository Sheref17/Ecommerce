using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Commands.StartProcessingOrder
{
    public record StartProcessingOrderCommand( int OrderId) : IRequest;
}
