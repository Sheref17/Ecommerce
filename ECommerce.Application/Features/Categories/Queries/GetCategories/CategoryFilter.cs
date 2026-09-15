using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategories
{
    public record CategoryFilter(string? Search,int PageNumber = 1, int PageSize = 10);
}
