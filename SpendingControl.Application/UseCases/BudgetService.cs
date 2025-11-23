using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _repo;

        public BudgetService(IBudgetRepository repo) => _repo = repo;

        public async Task<IEnumerable<Budget>> GetByUserAndMonthAsync(Guid userId, int year, int month)
        {
            ValidateUser(userId);
            ValidateYearMonth(year, month);

            return await _repo.GetByUserAndMonthAsync(userId, year, month);
        }

        public async Task<Budget?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<Budget> CreateAsync(Budget budget)
        {
            if (budget == null) throw new ArgumentNullException(nameof(budget));
            ValidateUser(budget.UserId);
            ValidateYearMonth(budget.Year, budget.Month);
            ValidateSpendType(budget.SpendTypeId);
            ValidateAmount(budget.Amount);

            // One budget per user, spendType, year, month
            var existingMonthBudgets = await _repo.GetByUserAndMonthAsync(budget.UserId, budget.Year, budget.Month);

            if (existingMonthBudgets.Any(b => b.SpendTypeId == budget.SpendTypeId))
                throw new InvalidOperationException("Budget already exists for this expense type and month.");

            return await _repo.AddAsync(budget);
        }

        public async Task UpdateAsync(Budget budget)
        {
            if (budget == null) throw new ArgumentNullException(nameof(budget));
            var current = await _repo.GetByIdAsync(budget.Id) ?? throw new KeyNotFoundException("Budget not found");

            
            ValidateAmount(budget.Amount);

            current.Amount = budget.Amount;
            await _repo.UpdateAsync(current);
        }

        public async Task DeleteAsync(Guid id)
        {
            var current = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Budget not found");
            await _repo.DeleteAsync(current.Id);
        }

        private static void ValidateUser(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId is required", nameof(userId));
        }

        private static void ValidateYearMonth(int year, int month)
        {
            if (year < 2000 || year > 2100) throw new ArgumentException("Year out of range", nameof(year));
            if (month < 1 || month > 12) throw new ArgumentException("Month must be 1-12", nameof(month));
        }

        private static void ValidateSpendType(int spendTypeId)
        {
            if (spendTypeId <= 0) throw new ArgumentException("SpendTypeId must be positive", nameof(spendTypeId));
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }
    }
}
