using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobSourceDataDto
    {
        [JsonPropertyName("pan")]
        public string Pan { get; set; } = null!;

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }
}
