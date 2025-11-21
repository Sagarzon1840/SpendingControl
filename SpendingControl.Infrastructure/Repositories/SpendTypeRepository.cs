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
    internal class SpendTypeRepository : ISpendTypeRepository
    {
        private readonly AppDbContext _db;
        public SpendTypeRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<SpendType>> GetByUserAsync(Guid userId)
        {
            return await _db.SpendTypes.Where(s => s.UserId == userId && s.IsActive).ToListAsync();
        }

        public async Task<SpendType?> GetByIdAsync(int id)
        {
            return await _db.SpendTypes.FindAsync(id);
        }

        public async Task<SpendType> AddAsync(SpendType spendType)
        {
            // compute next code for user
            var existing = await _db.SpendTypes.Where(s => s.UserId == spendType.UserId).ToListAsync();
            spendType.Code = SpendTypeCodeGenerator.NextCode(existing);
            _db.SpendTypes.Add(spendType);
            await _db.SaveChangesAsync();
            return spendType;
        }

        public async Task UpdateAsync(SpendType spendType)
        {
            _db.SpendTypes.Update(spendType);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _db.SpendTypes.FindAsync(id);
            if (entity == null) return;
            entity.IsActive = false; // soft delete
            await _db.SaveChangesAsync();
        }
    }
}
