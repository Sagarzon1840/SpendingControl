using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class DepositService : IDepositService
    {
        private readonly IDepositRepository _repo;

        public DepositService(IDepositRepository repo) => _repo = repo;

        public async Task<IEnumerable<Deposit>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null) => await _repo.GetByUserAsync(userId, from, to);

        public async Task<Deposit> CreateAsync(Deposit deposit) => await _repo.AddAsync(deposit);
    }
}
