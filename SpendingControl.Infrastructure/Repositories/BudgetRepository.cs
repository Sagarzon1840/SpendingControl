using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;
using SpendingControl.Infrastructure.Persistence;

namespace SpendingControl.Infrastructure.Repositories
{
    internal class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _db;
        public BudgetRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Budget>> GetByUserAndMonthAsync(Guid userId, int year, int month)
        {
            return await _db.Budgets.Where(b => b.UserId == userId && b.Year == year && b.Month == month).ToListAsync();
        }

        public async Task<Budget?> GetByIdAsync(Guid id)
        {
            return await _db.Budgets.FindAsync(id);
        }

        public async Task<Budget> AddAsync(Budget budget)
        {
            _db.Budgets.Add(budget);
            await _db.SaveChangesAsync();
            return budget;
        }

        public async Task UpdateAsync(Budget budget)
        {
            _db.Budgets.Update(budget);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Budgets.FindAsync(id);
            if (entity == null) return;
            _db.Budgets.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
