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
    internal class MonetaryFundRepository : IMonetaryFundRepository
    {
        private readonly AppDbContext _db;
        public MonetaryFundRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<MonetaryFund>> GetByUserAsync(Guid userId)
        {
            return await _db.MonetaryFunds.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<MonetaryFund?> GetByIdAsync(Guid id)
        {
            return await _db.MonetaryFunds.FindAsync(id);
        }

        public async Task<MonetaryFund> AddAsync(MonetaryFund fund)
        {
            _db.MonetaryFunds.Add(fund);
            await _db.SaveChangesAsync();

            return fund;
        }

        public async Task UpdateAsync(MonetaryFund fund)
        {
            _db.MonetaryFunds.Update(fund);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.MonetaryFunds.FindAsync(id);
            if (entity == null) return;
            _db.MonetaryFunds.Remove(entity);

            await _db.SaveChangesAsync();
        }
    }
}
