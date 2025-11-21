using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface IMonetaryFundService
    {
        Task<IEnumerable<MonetaryFund>> GetByUserAsync(Guid userId);
        Task<MonetaryFund?> GetByIdAsync(Guid id);
        Task<MonetaryFund> CreateAsync(MonetaryFund fund);
        Task UpdateAsync(MonetaryFund fund);
        Task DeleteAsync(Guid id);
    }
}
