using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IBrandRepository
    {
        Task AddAsync(Brand brand, CancellationToken cancellationToken);
    }
}
