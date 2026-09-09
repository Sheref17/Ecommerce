using ECommerce.Application.Abstractions.Specifications;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductFilter filter)
        {
            Criteria  = product => (string.IsNullOrEmpty(filter.Search)
            || product.Name.Contains(filter.Search)) &&
            (!filter.CategoryId.HasValue || 
            product.CategoryId == filter.CategoryId.Value) &&
            (!filter.MinPrice.HasValue ||
            product.Price.Amount >= filter.MinPrice.Value) &&
            (!filter.MaxPrice.HasValue ||
            product.Price.Amount <= filter.MaxPrice.Value) &&
            (!filter.IsActive.HasValue ||
            product.IsActive == filter.IsActive.Value);


            ApplySorting(filter);

            ApplyPaging(filter.PageNumber , filter.PageSize);



            

        }

        private void ApplySorting(ProductFilter filter)
        {
            switch (filter.SortBy?.ToLower())
            {
                case "name":
                    if(filter.SortDescending)
                    OrderByDescending = product => product.Name;
                    else
                        OrderBy = product => product.Name;
                    break;
                case "price":
                    if (filter.SortDescending)
                        OrderByDescending = product => product.Price.Amount;
                    else
                        OrderBy = product => product.Price.Amount;
                    break;
                case "stock":
                    if (filter.SortDescending)
                        OrderByDescending = product => product.Stock;
                    else
                        OrderBy = product => product.Stock;
                    break;
                default:
                    OrderBy = product => product.Id;
                    break;
            }
        }
    }
}
