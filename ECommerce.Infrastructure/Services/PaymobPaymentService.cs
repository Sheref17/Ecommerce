using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Application.Features.Payments.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;


namespace ECommerce.Infrastructure.Services
{
    public class PaymobPaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymobPaymentService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> ProcessPaymentAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            var secretKey = _configuration["Paymob:SecretKey"]!;
            var integrationId = _configuration["Paymob:IntegrationId"]!;
            var notification_url = _configuration["Paymob:notification_url"]!;

            var request = new PaymobIntentionRequest
            {
                amount = (int)(paymentRequest.Amount * 100),
                currency = "EGP",
                payment_methods = new[]
                {
                    int.Parse(integrationId)
                },
                special_reference = paymentRequest.Reference,
                items = paymentRequest.Items.Select(x => new PaymobItem
                {
                    name = x.Name,
                    amount = (int)(x.Amount * 100),
                    description = x.Description,
                    quantity = x.Quantity
                }).ToList(),
                notification_url = notification_url,

                billing_data = new PaymobBillingData
                {
                    first_name = paymentRequest.BillingData.FirstName,
                    last_name = paymentRequest.BillingData.LastName,
                    email = paymentRequest.BillingData.Email,
                    country = paymentRequest.BillingData.Country,
                    city = paymentRequest.BillingData.City,
                    state = paymentRequest.BillingData.State,
                    street = paymentRequest.BillingData.Street,
                    building = paymentRequest.BillingData.Building,
                    floor = paymentRequest.BillingData.Floor,
                    apartment = paymentRequest.BillingData.Apartment,
                    phone_number = paymentRequest.BillingData.PhoneNumber,
                    postal_code = paymentRequest.BillingData.PostalCode,
                }
                

            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                "https://accept.paymob.com/v1/intention/");

            httpRequest.Headers.Add("Authorization",$"Token {secretKey}");

            httpRequest.Content = JsonContent.Create(request);

            var response = await _httpClient.SendAsync(httpRequest,cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"Paymob returned {(int)response.StatusCode}: {error}");
            }

            var result = await response.Content
                .ReadFromJsonAsync<PaymobIntentionResponse>(cancellationToken);
            if(string.IsNullOrWhiteSpace(result!.ClientSecret))
                throw new InvalidOperationException("Paymob did not return a client secret.");

            var publicKey = _configuration["Paymob:PublicKey"]!;
            var checkOutUrl = $"https://accept.paymob.com/unifiedcheckout/" +
                $"?publicKey={Uri.EscapeDataString(publicKey)}" +
                $"&clientSecret={Uri.EscapeDataString(result.ClientSecret)}";

            return checkOutUrl;
              
        }
    }
}
