using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface ISpendTypeService
    {
        Task<IEnumerable<SpendType>> GetByUserAsync(Guid userId);
        Task<SpendType> GetByIdAsync(int id, Guid userId);
        Task<SpendType> CreateAsync(SpendType spendType);
        Task UpdateAsync(int id, SpendType dto);
        Task DeleteAsync(int id, Guid userId);
    }
}
