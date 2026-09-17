using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobWebhookRequest
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("obj")]
        public PaymobTransactionDto Obj { get; set; } = null!;

        [JsonPropertyName("hmac")]
        public string Hmac { get; set; } = null!;
    }
}
