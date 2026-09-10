using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        private OrderItem()
        {
        }

        private OrderItem( int productId,int quantity,decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public static OrderItem Create(int productId,int quantity,
            decimal unitPrice)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product.");

            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            if (unitPrice < 0)
                throw new ArgumentException(
                    "Unit price cannot be negative.");

            return new OrderItem( productId,quantity,unitPrice);
        }

        public decimal GetTotal()
        {
            return Quantity * UnitPrice;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = quantity;
        }
    }
}
