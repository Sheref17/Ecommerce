using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.DTOs
{
    public record OrderItemResponse(Guid ProductId,int Quantity, decimal UnitPrice,
        decimal Total);

    public record OrderResponse(Guid Id,int UserId,string Status,DateTime CreatedAt,decimal Total,
        IReadOnlyList<OrderItemResponse> Items);
}
