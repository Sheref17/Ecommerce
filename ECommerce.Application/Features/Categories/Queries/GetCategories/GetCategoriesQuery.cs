using ECommerce.Application.Common;
using ECommerce.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(CategoryFilter Filter)
        : IRequest<PagedResult<CategoryResponse>>;
}
