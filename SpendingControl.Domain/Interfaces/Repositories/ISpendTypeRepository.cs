using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface ISpendTypeRepository
    {
        Task<IEnumerable<SpendType>> GetByUserAsync(Guid userId);
        Task<SpendType?> GetByIdAsync(int id);
        Task<SpendType> AddAsync(SpendType spendType);
        Task UpdateAsync(SpendType spendType);
        Task DeleteAsync(int id);
    }
}
