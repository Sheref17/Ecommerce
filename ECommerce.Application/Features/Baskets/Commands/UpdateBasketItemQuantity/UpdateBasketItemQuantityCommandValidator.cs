using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityCommandValidator
        : AbstractValidator<UpdateBasketItemQuantityCommand>
    {
        public UpdateBasketItemQuantityCommandValidator()
        {
            RuleFor(x => x.Quantity)
              .GreaterThan(0)
              .WithMessage(
                  "Quantity must be greater than zero.");
        }
    }
}
