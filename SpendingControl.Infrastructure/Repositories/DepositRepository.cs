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
    internal class DepositRepository : IDepositRepository
    {
        private readonly AppDbContext _db;
        public DepositRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Deposit>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null)
        {            
            var q = from d in _db.Deposits
                    join f in _db.MonetaryFunds on d.FundId equals f.Id
                    where f.UserId == userId
                    select d;

            if (from.HasValue) q = q.Where(d => d.Date >= from.Value);
            if (to.HasValue) q = q.Where(d => d.Date <= to.Value);

            return await q.OrderByDescending(d => d.Date).ToListAsync();
        }

        public async Task<Deposit> AddAsync(Deposit deposit)
        {
            _db.Deposits.Add(deposit);
            await _db.SaveChangesAsync();

            return deposit;
        }
    }
}
