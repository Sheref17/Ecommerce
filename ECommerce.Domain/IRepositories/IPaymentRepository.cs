using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment,CancellationToken cancellationToken);

        Task<Payment?> GetByIdAsync(Guid id,CancellationToken cancellationToken);
        Task<Payment?> GetByOrderIdAsync(Guid orderId,CancellationToken cancellationToken);
    }
}
