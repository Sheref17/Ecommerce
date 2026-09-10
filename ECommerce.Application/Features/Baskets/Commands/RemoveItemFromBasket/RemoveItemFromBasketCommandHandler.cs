using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandler : IRequestHandler<RemoveItemFromBasketCommand>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveItemFromBasketCommandHandler(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle( RemoveItemFromBasketCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetByUserIdAsync(request.UserId,
                cancellationToken);

            if (basket is null)
                throw new KeyNotFoundException(
                    $"Basket for user {request.UserId} was not found.");

            basket.RemoveItem(request.ProductId);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
