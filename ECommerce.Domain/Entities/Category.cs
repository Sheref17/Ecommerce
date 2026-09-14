using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public bool IsActive { get; private set; } 
        private Category() { }
        private Category(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = true;

        }
        public static Category Create(string name, string description)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            if(string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Category description cannot be empty.");
            }
            return new Category(name, description);
        }
        public void Update(string name ,string description)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            if(string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Category description cannot be empty.");
            }
            Name = name;
            Description = description;

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
