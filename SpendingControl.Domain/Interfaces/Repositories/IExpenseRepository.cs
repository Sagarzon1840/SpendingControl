using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface IExpenseRepository
    {
        Task<SpendingHeader> AddAsync(SpendingHeader header);
        Task<SpendingHeader?> GetByIdAsync(Guid id);
        Task<IEnumerable<SpendingHeader>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null, int page = 1, int size = 50);        
        Task<(SpendingHeader header, IEnumerable<OverdraftWarning> overdraftWarnings)> CreateExpenseAsync(SpendingHeader header);
        // Sum executed amounts per expense type for user within inclusive date range.
        Task<IEnumerable<(int ExpenseTypeId, decimal Total)>> GetExecutedByTypeAsync(Guid userId, DateTime from, DateTime to, IEnumerable<int>? filterExpenseTypeIds = null);
    }
}
