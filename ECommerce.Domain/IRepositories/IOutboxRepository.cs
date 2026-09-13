using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IOutboxRepository
    {
        Task<IReadOnlyList<OutboxMessage>> GetUnprocessedAsync(int batchSize,
            CancellationToken cancellationToken);

        Task UpdateAsync(OutboxMessage message,CancellationToken cancellationToken);
    }
}
