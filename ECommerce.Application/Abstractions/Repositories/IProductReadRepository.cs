using ECommerce.Application.Common;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Features.Products.Queries.GetProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Repositories
{
    public interface IProductReadRepository
    {
        Task<PagedResult<ProductResponse>> GetAllProudcts(ProductFilter filter ,CancellationToken cancellationToken);
        Task<ProductResponse?> GetProductById(int id , CancellationToken cancellationToken);
    }
}
