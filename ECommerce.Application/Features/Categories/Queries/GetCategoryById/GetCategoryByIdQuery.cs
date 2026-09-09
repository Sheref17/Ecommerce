using ECommerce.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryById
{

    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResponse?>;
}
