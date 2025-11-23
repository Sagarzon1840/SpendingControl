using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<SpendType>> GetByUserAsync(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            return await _repo.GetByUserAsync(userId);
        }

        public async Task<SpendType> GetByIdAsync(int id, Guid userId)
        {
            if (id <= 0) throw new ArgumentException("id must be positive", nameof(id));
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));

            var entity = await _repo.GetByIdForUserAsync(userId, id) ?? throw new KeyNotFoundException("Spend type not found");

            return entity;
        }

        public async Task<SpendType> CreateAsync(SpendType spendType)
        {
            if (spendType == null) throw new ArgumentNullException(nameof(spendType));
            if (spendType.UserId == Guid.Empty) throw new ArgumentException("UserId is required", nameof(spendType.UserId));

            spendType.Name = spendType.Name?.Trim();
            if (string.IsNullOrWhiteSpace(spendType.Name)) throw new ArgumentException("Name is required", nameof(spendType.Name));
            if (spendType.Name.Length > 200) throw new ArgumentException("Name too long", nameof(spendType.Name));

            var existing = await _repo.GetByUserAsync(spendType.UserId);
            if (existing.Any(s => s.Name.Equals(spendType.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A spend type with the same name already exists for this user.");

            spendType.Code = 0; // repo will assign
            return await _repo.AddAsync(spendType);
        }

        public async Task UpdateAsync(int id, SpendType updateSpendType)
        {
            if (updateSpendType.Name?.Length > 200) throw new ArgumentException("Name has to be less than 200 characters", nameof(updateSpendType.Name));

            var spendType = await _repo.GetByIdForUserAsync(updateSpendType.UserId, id) ?? throw new KeyNotFoundException("Spend type not found");
            if (spendType is null) throw new ArgumentNullException(nameof(spendType));

            spendType.Name =
                !string.IsNullOrWhiteSpace(updateSpendType.Name) && updateSpendType.Name != spendType.Name
                ? updateSpendType.Name
                : spendType.Name;

            if (updateSpendType.IsActive != spendType.IsActive)
            {
                spendType.IsActive = updateSpendType.IsActive;
            }

            await _repo.UpdateAsync(spendType);
        }

        public async Task DeleteAsync(int id, Guid userId)
        {
            if (id <= 0) throw new ArgumentException("id must be positive", nameof(id));
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));

            var current = await _repo.GetByIdForUserAsync(userId, id) ?? throw new KeyNotFoundException("Spend type not found");

            await _repo.DeleteAsync(current.Id);
        }
    }
}
