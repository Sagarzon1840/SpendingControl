using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class MonetaryFundService : IMonetaryFundService
    {
        private readonly IMonetaryFundRepository _repo;

        public MonetaryFundService(IMonetaryFundRepository repo) => _repo = repo;

        public async Task<IEnumerable<MonetaryFund>> GetByUserAsync(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            return await _repo.GetByUserAsync(userId);
        }

        public async Task<MonetaryFund> GetByIdAsync(Guid id, Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            var fund = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Monetary fund not found");
            if (fund.UserId != userId) throw new UnauthorizedAccessException("You do not own this monetary fund");

            return fund;
        }

        public async Task<MonetaryFund> CreateAsync(MonetaryFund fund)
        {
            if (fund == null) throw new ArgumentNullException(nameof(fund));
            if (fund.UserId == Guid.Empty) throw new ArgumentException("UserId is required", nameof(fund.UserId));

            fund.Name = fund.Name?.Trim();

            if (string.IsNullOrWhiteSpace(fund.Name)) throw new ArgumentException("Name is required", nameof(fund.Name));
            if (fund.Name.Length > 100) throw new ArgumentException("Name too long", nameof(fund.Name));
            if (fund.InitialBalance < 0) throw new ArgumentException("Initial balance cannot be negative", nameof(fund.InitialBalance));

            var existing = await _repo.GetByUserAsync(fund.UserId);

            if (existing.Any(f => f.Name.Equals(fund.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A monetary fund with the same name already exists for this user.");
            
            fund.ApplyDeposit(fund.InitialBalance);

            return await _repo.AddAsync(fund);
        }

        public async Task UpdateAsync(MonetaryFund fund, Guid userId)
        {
            if (fund == null) throw new ArgumentNullException(nameof(fund));
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));

            var current = await _repo.GetByIdAsync(fund.Id) ?? throw new KeyNotFoundException("Monetary fund not found");
            if (current.UserId != userId) throw new UnauthorizedAccessException("You do not own this monetary fund");

            fund.Name = fund.Name?.Trim();
            if (string.IsNullOrWhiteSpace(fund.Name)) throw new ArgumentException("Name is required", nameof(fund.Name));
            if (fund.Name.Length > 100) throw new ArgumentException("Name too long", nameof(fund.Name));
            if (fund.InitialBalance < 0) throw new ArgumentException("Initial balance cannot be negative", nameof(fund.InitialBalance));

            var existing = await _repo.GetByUserAsync(userId);
            if (existing.Any(f => f.Id != fund.Id && f.Name.Equals(fund.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A monetary fund with the same name already exists for this user.");
            
            fund.UserId = current.UserId;

            await _repo.UpdateAsync(fund);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            var current = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Monetary fund not found");
            if (current.UserId != userId) throw new UnauthorizedAccessException("You do not own this monetary fund");

            await _repo.DeleteAsync(id);
        }
    }
}
