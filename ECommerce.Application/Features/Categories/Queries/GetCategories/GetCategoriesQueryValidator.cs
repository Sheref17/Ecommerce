using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
    {
        public GetCategoriesQueryValidator()
        {
            RuleFor(x => x.Filter.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage(
                    "Page number must be greater than or equal to 1.");

            RuleFor(x => x.Filter.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage(
                    "Page size must be between 1 and 50.");
        }
    }
}
