using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface ISpendTypeService
    {
        Task<IEnumerable<SpendType>> GetByUserAsync(Guid userId);
        Task<SpendType?> GetByIdAsync(int id);
        Task<SpendType> CreateAsync(SpendType spendType);
        Task UpdateAsync(SpendType spendType, Guid userId);
        Task DeleteAsync(int id, Guid userId);
    }
}
