using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Basket : BaseEntity
    {
        public int UserId { get; private set; }

        private readonly List<BasketItem> _items = [];

        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        private Basket(){}

        private Basket(int userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        public static Basket Create(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user.");

            return new Basket(userId);
        }

        public void AddItem(Guid productId,int quantity)
        {
            var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.IncreaseQuantity(quantity);
                return;
            }

            var item = BasketItem.Create(productId,quantity);

            _items.Add(item);
        }

        public void RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item is null)
                return;

            _items.Remove(item);
        }

        public void UpdateItemQuantity(Guid productId,int quantity)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item is null)
                throw new KeyNotFoundException($"Product with id {productId} was not found in the basket.");

            item.UpdateQuantity(quantity);
        }
        public void Clear()
        {
            _items.Clear();
        }
    }
}
