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
        private readonly IUnitOfWork _unitOfWork;

        public ClearBasketCommandHandler(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ClearBasketCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetByUserIdAsync(request.UserId,
                cancellationToken);

            if (basket is null)
                throw new KeyNotFoundException(
                    $"Basket for user {request.UserId} was not found.");

            basket.Clear();

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
