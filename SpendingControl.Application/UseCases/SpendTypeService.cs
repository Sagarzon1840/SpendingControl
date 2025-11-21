using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class SpendTypeService : ISpendTypeService
    {
        private readonly ISpendTypeRepository _repo;

        public SpendTypeService(ISpendTypeRepository repo) => _repo = repo;

        public async Task<IEnumerable<SpendType>> GetByUserAsync(Guid userId) => await _repo.GetByUserAsync(userId);

        public async Task<SpendType?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<SpendType> CreateAsync(SpendType spendType) => await _repo.AddAsync(spendType);

        public async Task UpdateAsync(SpendType spendType) => await _repo.UpdateAsync(spendType);

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
