using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        private BasketItem(){}

        private BasketItem(Guid productId,int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Quantity = quantity;
        }

        public static BasketItem Create(Guid productId,int quantity)
        {
            if (productId == Guid.Empty)
                throw new DomainException("Invalid product.");

            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");

            return new BasketItem(productId,quantity);
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");

            Quantity += quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException(
                    "Quantity must be greater than zero.");

            Quantity = quantity;
        }
    }
}
