using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface IMonetaryFundRepository
    {
        Task<IEnumerable<MonetaryFund>> GetByUserAsync(Guid userId);
        Task<MonetaryFund?> GetByIdAsync(Guid id);
        Task<MonetaryFund> AddAsync(MonetaryFund fund);
        Task UpdateAsync(MonetaryFund fund);
        Task DeleteAsync(Guid id);
    }
}
