using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Orders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<OrderResponse?> Handle(GetOrderByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _orderReadRepository.GetByIdAsync(request.Id,cancellationToken);
        }
    }
}
