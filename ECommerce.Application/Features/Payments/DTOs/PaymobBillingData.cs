using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.DTOs
{
    public class PaymobBillingData
    {
        public string first_name { get; set; } = "Test";

        public string last_name { get; set; } = "User";

        public string email { get; set; } = "test@example.com";

        public string phone_number { get; set; } = "01000000000";

        public string apartment { get; set; } = "NA";

        public string floor { get; set; } = "NA";

        public string building { get; set; } = "NA";

        public string street { get; set; } = "NA";

        public string postal_code { get; set; } = "NA";

        public string city { get; set; } = "Cairo";

        public string state { get; set; } = "Cairo";

        public string country { get; set; } = "EG";
    }
}
