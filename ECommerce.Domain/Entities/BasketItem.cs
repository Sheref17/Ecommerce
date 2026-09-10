using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; private set; }

        public int Quantity { get; private set; }

        private BasketItem(){}

        private BasketItem(int productId,int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public static BasketItem Create( int productId,int quantity)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            return new BasketItem(productId,quantity);
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity += quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            Quantity = quantity;
        }
    }
}
