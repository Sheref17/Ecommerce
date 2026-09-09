using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.Filter.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.Filter.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("Page size must be between 1 and 50.");

            RuleFor(x => x.Filter.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Filter.MinPrice.HasValue);

            RuleFor(x => x.Filter.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Filter.MaxPrice.HasValue);

            RuleFor(x => x.Filter)
                .Must(filter =>
                    !filter.MinPrice.HasValue ||
                    !filter.MaxPrice.HasValue ||
                    filter.MinPrice <= filter.MaxPrice)
                .WithMessage("Minimum price cannot be greater than maximum price.");

            RuleFor(x => x.Filter.SortBy)
                .Must(BeValidSortField)
                .When(x => !string.IsNullOrWhiteSpace(x.Filter.SortBy))
                .WithMessage("Invalid sort field.");
        }

        private static bool BeValidSortField(string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name" => true,
                "price" => true,
                "stock" => true,
                _ => false
            };
        }
    }
}
