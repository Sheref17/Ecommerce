using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.DTOs
{
    public record CategoryResponse( int Id,string Name,string Description,bool IsActive);
}
