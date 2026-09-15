using ECommerce.Domain.Common;
using ECommerce.Domain.Events.Products;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections;
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

        public Guid CategoryId { get; private set; }
        public Guid BrandId { get; private set; }

        public bool IsActive { get; private set; }

        private Product() { }
        private Product(string name, string description, Money price, int stock,
            Guid categoryId, Guid brandId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            IsActive = true;
            BrandId = brandId;
        }
        public static Product Create(string name, string description, Money price, int stock, 
            Guid categoryId, Guid brandId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty.");
           
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Product description cannot be empty.");
           
            if (stock <= 0)
                throw new DomainException("Stock must be greater than zero.");

            if (categoryId == Guid.Empty)
                throw new DomainException("Invalid category.");

            if (brandId == Guid.Empty)
                throw new DomainException("Invalid brand.");

            var product = new Product(name, description, price, stock, categoryId , brandId);

            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id));
            
            return product;

        }
        public void Update(string name, string description, Money price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty.");
           
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Product description cannot be empty.");
          
            if (price.Amount <= 0)
                throw new DomainException("Price must be greater than zero.");

            Name = name;
            Description = description;
            Price = price;
        }
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");

            var oldStock = Stock;

            Stock += quantity;
            AddDomainEvent(new ProductStockChangedDomainEvent(Id, oldStock, Stock));
        }
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");
            if (quantity > Stock)
                throw new DomainException("Insufficient stock.");

            var oldStock = Stock;

            Stock -= quantity;
            AddDomainEvent(new ProductStockChangedDomainEvent(Id, oldStock, Stock));
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
