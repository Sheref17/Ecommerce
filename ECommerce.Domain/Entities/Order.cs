using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; private set; }

        public OrderStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private readonly List<OrderItem> _items = [];

        public IReadOnlyCollection<OrderItem> Items =>_items.AsReadOnly();

        private Order()
        {
        }

        private Order(int userId)
        {
            UserId = userId;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public static Order Create(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user.");

            return new Order(userId);
        }

        public void AddItem(int productId,int quantity,decimal unitPrice)
        {
            var item = OrderItem.Create(productId,quantity,unitPrice);

            _items.Add(item);
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item is null)
                return;

            _items.Remove(item);
        }

        public decimal GetTotal()
        {
            return _items.Sum(x => x.GetTotal());
        }
        public void Confirm()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be confirmed.");

            Status = OrderStatus.Confirmed;
        }

        public void StartProcessing()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed orders can start processing.");

            Status = OrderStatus.Processing;
        }

        public void Ship()
        {
            if (Status != OrderStatus.Processing)
                throw new InvalidOperationException("Only processing orders can be shipped.");

            Status = OrderStatus.Shipped;
        }

        public void Deliver()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Only shipped orders can be delivered.");

            Status = OrderStatus.Delivered;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Delivered orders cannot be cancelled.");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled.");

            Status = OrderStatus.Cancelled;
        }
    }
}
