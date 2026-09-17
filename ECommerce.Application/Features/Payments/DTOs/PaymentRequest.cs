using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }

        public List<PaymentItem> Items { get; set; } = [];
        public string Reference { get; set; } = null!;

        public BillingData BillingData { get; set; } = new();
    }

    public class PaymentItem
    {
        public string Name { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Description { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
