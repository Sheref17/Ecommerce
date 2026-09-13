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
    public class OutboxRepository : IOutboxRepository
    {
        private readonly ApplicationDbContext _context;

        public OutboxRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<OutboxMessage>> GetUnprocessedAsync(int batchSize,
            CancellationToken cancellationToken)
        {
            return await _context.OutboxMessages
                .Where(x => x.ProcessedOn == null)
                .OrderBy(x => x.OccurredOn)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(OutboxMessage message,CancellationToken cancellationToken)
        {
            _context.OutboxMessages.Update(message);

            return Task.CompletedTask;
        }
    }
}
