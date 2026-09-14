using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Events.Products
{
    public sealed record ProductStockChangedDomainEvent(Guid ProductId, int OldStock,
        int NewStock) : BaseDomainEvent;
   
}
