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
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, 
        IReadOnlyList<OrderResponse?>>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICurrentUserService _currentUserService;
        public GetOrdersQueryHandler(IOrderReadRepository orderReadRepository ,
           ICurrentUserService currentUserService
            )
        {
            _orderReadRepository = orderReadRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<OrderResponse?>> Handle(GetOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return await _orderReadRepository.GetOrdersAsync(userId.Value, cancellationToken);
        }
    }
}
