using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.AddItemToBasket
{
    public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommand>
    {
        public AddItemToBasketCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Invalid user.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Invalid product.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(
                    "Quantity must be greater than zero.");
        }
    }
}
