using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IBasketRepository
    {
        Task AddAsync(Basket basket,CancellationToken cancellationToken);

        Task<Basket?> GetByUserIdAsync(int userId,CancellationToken cancellationToken);

        Task DeleteAsync(Basket basket,CancellationToken cancellationToken);
        Task RemoveItemAsync(BasketItem item,CancellationToken cancellationToken);
    }
}
