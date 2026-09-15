using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Commands.UpdateBrand
{
    public record UpdateBrandCommand(Guid id ,string name , string description) : IRequest;
 
}
