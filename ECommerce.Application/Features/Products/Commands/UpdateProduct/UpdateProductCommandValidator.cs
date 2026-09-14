using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty();

            RuleFor(x => x.name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);
        }
    }
}
