using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Payments.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class PaymobWebhookService : IPaymobWebhookService
    {
        private readonly IConfiguration _configuration;

        public PaymobWebhookService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool VerifyHmac(PaymobWebhookRequest request)
        {
            var hmacSecret = _configuration["Paymob:HmacSecret"];
            var obj = request.Obj;

            static string Bool(bool value) => value ? "true" : "false";

            var concatenated =
                $"{obj.AmountCents}" +
                $"{obj.CreatedAt}" +
                $"{obj.Currency}" +
                $"{Bool(obj.ErrorOccured)}" +
                $"{Bool(obj.HasParentTransaction)}" +
                $"{obj.Id}" +
                $"{obj.IntegrationId}" +
                $"{Bool(obj.Is3dSecure)}" +
                $"{Bool(obj.IsAuth)}" +
                $"{Bool(obj.IsCapture)}" +
                $"{Bool(obj.IsRefunded)}" +
                $"{Bool(obj.IsStandalonePayment)}" +
                $"{Bool(obj.IsVoided)}" +
                $"{obj.Order.Id}" +
                $"{obj.Owner}" +
                $"{Bool(obj.Pending)}" +
                $"{obj.SourceData.Pan}" +
                $"{obj.SourceData.SubType}" +
                $"{obj.SourceData.Type}" +
                $"{Bool(obj.Success)}";

            using var hmac = new HMACSHA512(
                Encoding.UTF8.GetBytes(hmacSecret!));

            var hash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(concatenated));

            var calculatedHmac = Convert.ToHexString(hash)
                .ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(calculatedHmac),
                Encoding.UTF8.GetBytes(request.Hmac));
        }
        public Guid GetOrderId(PaymobWebhookRequest request)
        {
            var merchantOrderId = request.Obj.Order?.MerchantOrderId;

            if (string.IsNullOrWhiteSpace(merchantOrderId))
                throw new InvalidOperationException("Paymob callback does not contain merchant order id.");

            if (!Guid.TryParse(merchantOrderId, out var orderId))
                throw new InvalidOperationException("Invalid merchant order id.");

            return orderId;
        }
    }
}
