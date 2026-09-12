using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.MarkPaymentAsPaid
{
    public record MarkPaymentAsPaidCommand(int PaymentId) : IRequest;
}
