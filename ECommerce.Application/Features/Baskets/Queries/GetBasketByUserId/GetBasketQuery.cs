using ECommerce.Application.Features.Baskets.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Queries.GetBasketByUserId
{
    public record GetBasketQuery : IRequest<BasketResponse?>;
}
