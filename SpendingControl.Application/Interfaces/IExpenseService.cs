using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<(SpendingHeader header, IEnumerable<OverdraftWarning> overdraftWarnings)> CreateExpenseAsync(SpendingHeader header);
        Task<SpendingHeader?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<SpendingHeader>> GetListAsync(Guid userId, DateTime? from = null, DateTime? to = null, int page = 1, int size = 50);
        Task<IEnumerable<(int ExpenseTypeId, decimal Total)>> GetExecutedByTypeAsync(Guid userId, DateTime from, DateTime to, IEnumerable<int>? filterExpenseTypeIds = null);
    }
}
