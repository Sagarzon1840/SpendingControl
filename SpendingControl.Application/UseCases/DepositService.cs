using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class DepositService : IDepositService
    {
        private readonly IDepositRepository _repo;
        private readonly IMonetaryFundRepository _fundRepo;

        public DepositService(IDepositRepository repo, IMonetaryFundRepository fundRepo)
        {
            _repo = repo;
            _fundRepo = fundRepo;
        }

        public async Task<IEnumerable<Deposit>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            return await _repo.GetByUserAsync(userId, from, to);
        }

        public async Task<Deposit> CreateAsync(Deposit deposit)
        {
            if (deposit == null) throw new ArgumentNullException(nameof(deposit));
            if (deposit.FundId == Guid.Empty) throw new ArgumentException("FundId is required", nameof(deposit.FundId));
            if (deposit.Amount <= 0) throw new ArgumentException("Amount must be positive", nameof(deposit.Amount));
            if (deposit.Date == default) deposit.Date = DateTime.UtcNow;

            var fund = await _fundRepo.GetByIdAsync(deposit.FundId) ?? throw new KeyNotFoundException("Fund not found");
            if (fund.UserId == Guid.Empty) throw new InvalidOperationException("Fund has no owner");
                        
            fund.ApplyDeposit(deposit.Amount);

            // Persist updated balance BEFORE adding deposit to ensure consistency
            await _fundRepo.UpdateAsync(fund);            
            return await _repo.AddAsync(deposit);
        }
    }
}
