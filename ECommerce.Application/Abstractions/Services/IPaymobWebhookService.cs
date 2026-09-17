using ECommerce.Application.Features.Payments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IPaymobWebhookService
    {
        bool VerifyHmac(PaymobWebhookRequest request);
        Guid GetOrderId(PaymobWebhookRequest request);
    }
}
