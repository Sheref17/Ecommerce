using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobOrderDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("merchant_order_id")]
        public string? MerchantOrderId { get; set; }
    }
}
