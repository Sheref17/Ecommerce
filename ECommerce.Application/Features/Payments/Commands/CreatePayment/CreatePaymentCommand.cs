using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment
{
    public record CreatePaymentCommand(int Id) : IRequest<int>;
}
