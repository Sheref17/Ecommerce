using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Baskets.Dtos;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Queries.GetBasketByUserId
{
    public class GetBasketByUserIdQueryHandler: IRequestHandler<GetBasketQuery,
        BasketResponse?>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetBasketByUserIdQueryHandler(IBasketRepository basketRepository 
            , ICurrentUserService currentUserService)
        {
            _basketRepository = basketRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BasketResponse?> Handle(GetBasketQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                cancellationToken);

            if (basket is null)
                return null;

            return new BasketResponse(
                basket.Id,
                basket.UserId,
                basket.Items
                    .Select(item => new BasketItemResponse(item.ProductId,item.Quantity))
                    .ToList());
        }
    }
}
