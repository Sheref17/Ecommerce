using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public record ProductFilter(string ? Search,int? CategoryId, decimal? MinPrice,
            decimal? MaxPrice, bool? IsActive, string? SortBy = null, bool SortDescending = false
             ,int PageNumber = 1 , int PageSize = 10);

}
