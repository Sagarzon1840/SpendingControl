using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Budget>> GetByUserAndMonthAsync(Guid userId, int year, int month) => await _repo.GetByUserAndMonthAsync(userId, year, month);

        public async Task<Budget?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<Budget> CreateAsync(Budget budget) => await _repo.AddAsync(budget);

        public async Task UpdateAsync(Budget budget) => await _repo.UpdateAsync(budget);

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    }
}
