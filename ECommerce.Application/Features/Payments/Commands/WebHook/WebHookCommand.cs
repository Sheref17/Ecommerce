using ECommerce.Application.Features.Payments.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.WebHook
{
    public record WebHookCommand(PaymobWebhookRequest request ) : IRequest;
    
}
