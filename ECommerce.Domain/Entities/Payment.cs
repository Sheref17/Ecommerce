using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Payment() { }

        private Payment(Guid orderId,decimal amount)
        {
            Id= Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public static Payment Create(Guid orderId,decimal amount)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order.");

            if (amount <= 0)
                throw new ArgumentException(
                    "Payment amount must be greater than zero.");

            return new Payment(orderId, amount);
        }

        public void MarkAsPaid()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending payments can be marked as paid.");

            Status = PaymentStatus.Paid;
        }

        public void MarkAsFailed()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending payments can be marked as failed.");

            Status = PaymentStatus.Failed;
        }
    }
}
