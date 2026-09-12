using ECommerce.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ProcessPaymentAsync(decimal amount,
            CancellationToken cancellationToken)
        {
            // Temporary implementation.
            // Later this will call a real payment provider.

            return Task.FromResult(true);
        }
    }
}
