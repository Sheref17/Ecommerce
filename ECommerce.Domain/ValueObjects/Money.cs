using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; private init; }
        public string Currency { get; private init; } = default!;

        private Money()
        {
        }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency is required.");

            return new Money(amount, currency);
        }
    }
}
