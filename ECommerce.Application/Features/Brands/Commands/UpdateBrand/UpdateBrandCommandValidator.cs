using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.description)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
