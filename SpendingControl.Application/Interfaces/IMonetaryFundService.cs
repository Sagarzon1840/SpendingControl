using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface IMonetaryFundService
    {
        Task<IEnumerable<MonetaryFund>> GetByUserAsync(Guid userId);
        Task<MonetaryFund> GetByIdAsync(Guid id, Guid userId);
        Task<MonetaryFund> CreateAsync(MonetaryFund fund);
        Task UpdateAsync(MonetaryFund fund, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
