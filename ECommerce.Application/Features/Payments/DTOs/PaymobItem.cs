using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobItem
    {
        public string name { get; set; } = null!;

        public int amount { get; set; }

        public string description { get; set; } = null!;

        public int quantity { get; set; }
    }
}
