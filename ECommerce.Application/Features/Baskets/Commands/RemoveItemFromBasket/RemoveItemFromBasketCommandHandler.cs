using ECommerce.Application.Abstractions.Services;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveItemFromBasketCommandHandler(IBasketRepository basketRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveItemFromBasketCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new UnauthorizedAccessException($"User is not authenticated.");
            var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                cancellationToken);

            if (basket is null)
                throw new KeyNotFoundException(
                    $"Basket for user {userId} was not found.");

            var item = basket.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (item is null)
                throw new KeyNotFoundException(
                    $"Product with id {request.ProductId} was not found in the basket.");   

            basket.RemoveItem(request.ProductId);
            await _basketRepository.RemoveItemAsync(item, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
