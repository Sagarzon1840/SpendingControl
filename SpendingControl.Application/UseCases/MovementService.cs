using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class MovementService : IMovementService
    {
        private readonly IDepositRepository _depositRepo;
        private readonly IExpenseRepository _expenseRepo;

        public MovementService(IDepositRepository depositRepo, IExpenseRepository expenseRepo)
        {
            _depositRepo = depositRepo;
            _expenseRepo = expenseRepo;
        }

        public async Task<IEnumerable<MovementDto>> GetMovementsAsync(Guid userId, DateTime? from = null, DateTime? to = null, int page = 1, int size = 50)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            if (page < 1) page = 1;
            if (size < 1) size = 50;

            var deposits = await _depositRepo.GetByUserAsync(userId, from, to);
            var expenses = await _expenseRepo.GetByUserAsync(userId, from, to, 1, int.MaxValue);

            var depositMovements = deposits.Select(d => new MovementDto
            {
                Date = d.Date,
                FundId = d.FundId,
                Type = "Deposit",
                Amount = d.Amount,
                Description = d.Description
            });

            var expenseMovements = expenses.Select(e => new MovementDto
            {
                Date = e.Date,
                FundId = e.MonetaryFundId,
                Type = "Expense",
                Amount = e.Details.Sum(x => x.Amount),
                Description = e.Observations ?? e.MerchantName
            });

            return depositMovements
                .Concat(expenseMovements)
                .OrderByDescending(m => m.Date)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
    }
}
