using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Services;
using SpendingControl.Infrastructure.Persistence;

namespace SpendingControl.Infrastructure.Services
{
    internal class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _db;

        public ExpenseService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(SpendingHeader header, IEnumerable<int> overdrawnExpenseTypeIds)> CreateExpenseAsync(SpendingHeader header)
        {
            header.ValidateHasDetails();

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var fund = await _db.MonetaryFunds.FindAsync(header.MonetaryFundId);
                if (fund == null) throw new InvalidOperationException("Monetary fund not found");

                // add header and details
                _db.SpendingHeaders.Add(header);

                // update fund balance (in-memory helper)
                foreach (var d in header.Details)
                {
                    fund.ApplyWithdrawal(d.Amount);
                }

                await _db.SaveChangesAsync();

                var year = header.Date.Year;
                var month = header.Date.Month;

                // compute spent including this new header
                var spentByType = await _db.SpendingDetails
                    .Include(sd => sd.ExpenseHeader)
                    .Where(sd => sd.ExpenseHeader != null && sd.ExpenseHeader.Date.Year == year && sd.ExpenseHeader.Date.Month == month)
                    .GroupBy(sd => sd.ExpenseTypeId)
                    .Select(g => new { ExpenseTypeId = g.Key, Total = g.Sum(x => x.Amount) })
                    .ToListAsync();

                var newTotals = header.Details.GroupBy(d => d.ExpenseTypeId)
                    .Select(g => new { ExpenseTypeId = g.Key, Total = g.Sum(x => x.Amount) });

                var combined = spentByType.ToDictionary(x => x.ExpenseTypeId, x => x.Total);
                foreach (var nt in newTotals)
                {
                    if (combined.ContainsKey(nt.ExpenseTypeId)) combined[nt.ExpenseTypeId] += nt.Total;
                    else combined[nt.ExpenseTypeId] = nt.Total;
                }

                var overdrawn = new List<int>();
                var expenseTypeIds = combined.Keys.ToList();
                var budgets = await _db.Budgets
                    .Where(b => b.UserId == header.UserId && expenseTypeIds.Contains(b.SpendTypeId) && b.Year == year && b.Month == month)
                    .ToListAsync();

                foreach (var kv in combined)
                {
                    var budget = budgets.FirstOrDefault(b => b.SpendTypeId == kv.Key);
                    if (budget != null && kv.Value > budget.Amount)
                    {
                        overdrawn.Add(kv.Key);
                    }
                }

                await tx.CommitAsync();
                return (header, overdrawn);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
