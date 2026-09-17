using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Application.Features.Payments.DTOs;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Commands.Checkout
{
    public record CheckoutCommand(CheckOutRequest request) : IRequest<CheckoutResponse>;
  
}
