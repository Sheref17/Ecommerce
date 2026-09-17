using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobIntentionResponse
    {
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = null!;
    }
}
