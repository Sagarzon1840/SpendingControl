using System;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface IExpenseRepository
    {
        Task<SpendingHeader> AddAsync(SpendingHeader header);
        Task<SpendingHeader?> GetByIdAsync(Guid id);
    }
}
