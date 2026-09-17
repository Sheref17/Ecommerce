using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.DTOs
{
    public class CheckoutResponse
    {
        public Guid OrderId { get; set; }
        public Guid? PaymentId { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
