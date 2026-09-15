using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Queries.GetBrands
{
    public record BrandFilter(string? Search,int PageNumber = 1,int PageSize = 10);
}
