using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<Budget>> GetByUserAndMonthAsync(Guid userId, int year, int month);
        Task<Budget?> GetByIdAsync(Guid id);
        Task<Budget> CreateAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Guid id);
    }
}
