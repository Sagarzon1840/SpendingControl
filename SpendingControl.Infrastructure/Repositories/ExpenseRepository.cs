using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;
using SpendingControl.Infrastructure.Persistence;

namespace SpendingControl.Infrastructure.Repositories
{
    internal class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _db;
        public ExpenseRepository(AppDbContext db) => _db = db;

        public async Task<SpendingHeader> AddAsync(SpendingHeader header)
        {
            _db.SpendingHeaders.Add(header);
            await _db.SaveChangesAsync();
            return header;
        }

        public async Task<SpendingHeader?> GetByIdAsync(Guid id)
        {
            return await _db.SpendingHeaders
                .Include(h => h.Details)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
