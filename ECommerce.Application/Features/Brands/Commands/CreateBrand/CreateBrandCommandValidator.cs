using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150)
                .WithMessage("Brand name is required and cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Brand description is required and cannot exceed 500 characters.");
        }
    }
}
