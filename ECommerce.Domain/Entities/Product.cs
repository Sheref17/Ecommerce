using ECommerce.Domain.Common;
using ECommerce.Domain.Events.Products;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        public Money Price { get; private set; } = default!;

        public int Stock { get; private set; }

        public int CategoryId { get; private set; }

        public bool IsActive { get; private set; }

        private Product() { }
        private Product(string name, string description, Money price, int stock, int categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            IsActive = true;
        }
        public static Product Create(string name, string description, Money price, int stock, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
           
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Product description cannot be empty.");
           
            if (stock < 0)
                throw new ArgumentException("Stock cannot be negative.");
           
            if (categoryId <= 0)
                throw new ArgumentException("Invalid category.");

           var product = new Product(name, description, price, stock, categoryId);

            product.AddDomainEvent(new ProductCreatedDomainEvent(product));
            
            return product;

        }
        public void Update(string name, string description, Money price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
           
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Product description cannot be empty.");
           
   
            Name = name;
            Description = description;
            Price = price;
        }
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var oldStock = Stock;

            Stock += quantity;
            AddDomainEvent(new ProductStockChangedDomainEvent(this, oldStock, Stock));
        }
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (quantity > Stock)
                throw new InvalidOperationException("Insufficient stock.");

            var oldStock = Stock;

            Stock -= quantity;
            AddDomainEvent(new ProductStockChangedDomainEvent(this, oldStock, Stock));
        }
        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

    }
}
