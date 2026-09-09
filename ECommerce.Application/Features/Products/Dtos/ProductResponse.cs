using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Dtos
{
    public record ProductResponse(int id, string name, string description,
        decimal price, string currency, int stock, string categoryName);
   
}
