using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface IBudgetRepository
    {
        Task<IEnumerable<Budget>> GetByUserAndMonthAsync(Guid userId, int year, int month);
        Task<Budget?> GetByIdAsync(Guid id);
        Task<Budget> AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Guid id);
    }
}
