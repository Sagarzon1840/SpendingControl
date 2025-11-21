using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<MonetaryFund>> GetByUserAsync(Guid userId) => await _repo.GetByUserAsync(userId);

        public async Task<MonetaryFund?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<MonetaryFund> CreateAsync(MonetaryFund fund) => await _repo.AddAsync(fund);

        public async Task UpdateAsync(MonetaryFund fund) => await _repo.UpdateAsync(fund);

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    }
}
