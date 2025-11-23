using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;

        public ExpenseService(IExpenseRepository repo) => _repo = repo;

        public async Task<(SpendingHeader header, IEnumerable<OverdraftWarning> overdraftWarnings)> CreateExpenseAsync(SpendingHeader header)
        {
            return await _repo.CreateExpenseAsync(header);
        }

        public async Task<SpendingHeader?> GetByIdAsync(Guid id, Guid userId)
        {
            var header = await _repo.GetByIdAsync(id);
            if (header == null || header.UserId != userId) return null;
            return header;
        }

        public Task<IEnumerable<SpendingHeader>> GetListAsync(Guid userId, DateTime? from = null, DateTime? to = null, int page = 1, int size = 50)
            => _repo.GetByUserAsync(userId, from, to, page, size);

        public Task<IEnumerable<(int ExpenseTypeId, decimal Total)>> GetExecutedByTypeAsync(Guid userId, DateTime from, DateTime to, IEnumerable<int>? filterExpenseTypeIds = null)
            => _repo.GetExecutedByTypeAsync(userId, from, to, filterExpenseTypeIds);

        public async Task<bool> SoftDeleteAsync(Guid id, Guid userId)
        {
            if (userId == Guid.Empty) return false;
            if (id == Guid.Empty) return false;
            return await _repo.SoftDeleteAsync(id, userId);
        }
    }
}
