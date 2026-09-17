using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobIntentionRequest
    {
        public int amount { get; set; }
        public string currency { get; set; } = "EGP";
        public int[] payment_methods { get; set; } = [];
        public string special_reference { get; set; } = null!;
        public List<PaymobItem> items { get; set; } = [];
        public string notification_url { get; set; } = null!;
        public PaymobBillingData billing_data { get; set; } = new();
    }
}
