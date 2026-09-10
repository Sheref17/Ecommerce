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
    public class GetBasketByUserIdQueryHandler: IRequestHandler<GetBasketByUserIdQuery,
        BasketResponse?>
    {
        private readonly IBasketRepository _basketRepository;

        public GetBasketByUserIdQueryHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<BasketResponse?> Handle(GetBasketByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetByUserIdAsync(request.UserId,
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
