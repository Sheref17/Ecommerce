using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment,CancellationToken cancellationToken)
        {
            await _context.Payments.AddAsync(payment,cancellationToken);
        }

        public async Task<Payment?> GetByIdAsync(Guid id,
            CancellationToken cancellationToken)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        }

        public async Task<Payment?> GetByOrderIdAsync(Guid orderId,
            CancellationToken cancellationToken)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.OrderId == orderId,cancellationToken);
        }
    }
}
