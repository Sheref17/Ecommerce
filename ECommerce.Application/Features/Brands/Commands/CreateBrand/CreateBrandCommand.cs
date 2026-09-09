using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Commands.CreateBrand
{
    public record CreateBrandCommand( string Name, string Description) : IRequest<int>;
}
