using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public bool IsActive { get; private set; }

        private Brand()
        {
        }
        private Brand(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = true;
        }
        public static Brand Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Brand name is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Brand description is required.");

            return new Brand(name,description);
        }

        public void Update(string name,string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Brand name is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Brand description is required.");

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
