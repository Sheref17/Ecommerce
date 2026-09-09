using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Queries.GetBrands
{
    public class GetBrandsQueryValidator: AbstractValidator<GetBrandsQuery>
    {
        public GetBrandsQueryValidator()
        {
            RuleFor(x => x.filter.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage(
                    "Page number must be greater than or equal to 1.");

            RuleFor(x => x.filter.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage(
                    "Page size must be between 1 and 50.");
        }
    }
}
