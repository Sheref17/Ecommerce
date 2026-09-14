using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Dtos
{
    public record BasketItemResponse( Guid ProductId,int Quantity);

    public record BasketResponse(Guid Id,int UserId,IReadOnlyList<BasketItemResponse> Items);
}
