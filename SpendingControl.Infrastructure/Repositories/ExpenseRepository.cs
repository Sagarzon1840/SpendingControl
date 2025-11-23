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

        public async Task<IEnumerable<SpendingHeader>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null, int page = 1, int size = 50)
        {
            if (page < 1) page = 1;
            if (size < 1) size = 50;

            var query = _db.SpendingHeaders
                .Include(h => h.Details)
                .Where(h => h.UserId == userId);

            if (from.HasValue) query = query.Where(h => h.Date >= from.Value);
            if (to.HasValue) query = query.Where(h => h.Date <= to.Value);

            return await query
                .OrderByDescending(h => h.Date)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<(SpendingHeader header, IEnumerable<OverdraftWarning> overdraftWarnings)> CreateExpenseAsync(SpendingHeader header)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            header.ValidateHasDetails();
            if (header.UserId == Guid.Empty) throw new ArgumentException("UserId is required", nameof(header.UserId));
            if (header.MonetaryFundId == Guid.Empty) throw new ArgumentException("MonetaryFundId is required", nameof(header.MonetaryFundId));
            foreach (var d in header.Details) d.Validate();

            var strategy = _db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var tx = await _db.Database.BeginTransactionAsync();
                try
                {
                    var fund = await _db.MonetaryFunds.FindAsync(header.MonetaryFundId) ?? throw new InvalidOperationException("Monetary fund not found");
                    if (fund.UserId != header.UserId) throw new UnauthorizedAccessException("Fund does not belong to user");

                    var previousFundBalance = fund.CurrentBalance;

                    foreach (var d in header.Details)
                        d.ExpenseHeaderId = header.Id;

                    _db.SpendingHeaders.Add(header);

                    decimal setWithdrawal = -1;
                    foreach (var d in header.Details)
                    {
                        setWithdrawal = fund.CurrentBalance - d.Amount;                        
                        fund.ApplyWithdrawal(d.Amount, setWithdrawal);
                    }

                    if (fund.CurrentBalance < 0)
                    {
                        // mark invalid and do not persist details impact on analytics
                        header.IsValid = false;
                    }

                    await _db.SaveChangesAsync();

                    var executedByType = header.Details
                        .GroupBy(d => d.ExpenseTypeId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

                    var typeIds = executedByType.Keys.ToList();
                    var spendTypes = await _db.SpendTypes
                        .Where(st => st.UserId == header.UserId && typeIds.Contains(st.Id))
                        .ToDictionaryAsync(st => st.Id, st => st.Name);

                    var warnings = new List<OverdraftWarning>();
                    foreach (var kv in executedByType)
                    {
                        if (kv.Value > previousFundBalance)
                        {
                            warnings.Add(new OverdraftWarning
                            {
                                ExpenseTypeId = kv.Key,
                                ExpenseTypeName = spendTypes.TryGetValue(kv.Key, out var name) ? name : string.Empty,
                                Budget = previousFundBalance,
                                Executed = kv.Value,
                                Overdraft = kv.Value - previousFundBalance
                            });
                        }
                    }

                    await tx.CommitAsync();
                    return (header, (IEnumerable<OverdraftWarning>)warnings);
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<IEnumerable<(int ExpenseTypeId, decimal Total)>> GetExecutedByTypeAsync(Guid userId, DateTime from, DateTime to, IEnumerable<int>? filterExpenseTypeIds = null)
        {
            var q = _db.SpendingDetails
                .Include(d => d.ExpenseHeader)
                .Where(d => d.ExpenseHeader != null && d.ExpenseHeader.IsValid && d.ExpenseHeader.UserId == userId && d.ExpenseHeader.Date >= from && d.ExpenseHeader.Date <= to);

            if (filterExpenseTypeIds != null)
            {
                var ids = filterExpenseTypeIds.Distinct().ToList();
                if (ids.Count > 0)
                    q = q.Where(d => ids.Contains(d.ExpenseTypeId));
            }

            return await q.GroupBy(d => d.ExpenseTypeId)
                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.Amount)))
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteAsync(Guid id, Guid userId)
        {
            if (id == Guid.Empty || userId == Guid.Empty) return false;
            var header = await _db.SpendingHeaders.FirstOrDefaultAsync(h => h.Id == id);

            if (header == null) return false;
            if (header.UserId != userId) return false;
            if (!header.IsValid) return true;
            header.IsValid = false;

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
