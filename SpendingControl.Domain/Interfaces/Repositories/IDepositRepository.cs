using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface IDepositRepository
    {
        Task<IEnumerable<Deposit>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null);
        Task<Deposit> AddAsync(Deposit deposit);
    }
}
