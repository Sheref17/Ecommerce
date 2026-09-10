using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Dtos
{
    public record BasketItemResponse( int ProductId,int Quantity);

    public record BasketResponse(int Id,int UserId,IReadOnlyList<BasketItemResponse> Items);
}
