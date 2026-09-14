using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        private OrderItem()
        {
        }

        private OrderItem( Guid productId,int quantity,decimal unitPrice)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public static OrderItem Create(Guid productId,int quantity,
            decimal unitPrice)
        {
            if (productId == Guid.Empty)
                throw new DomainException("Invalid product.");

            if (quantity <= 0)
                throw new DomainException(
                    "Quantity must be greater than zero.");

            if (unitPrice < 0)
                throw new DomainException(
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
                throw new DomainException("Quantity must be greater than zero.");

            Quantity = quantity;
        }
    }
}
