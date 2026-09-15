using ECommerce.Application.Abstractions.Services;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.ClearBasket
{
    public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public ClearBasketCommandHandler(IBasketRepository basketRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ClearBasketCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new UnauthorizedAccessException($"User is not authenticated.");

            var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                cancellationToken);

            if (basket is null)
                throw new KeyNotFoundException(
                    $"Basket for user {userId.Value} was not found.");

            basket.Clear();

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
