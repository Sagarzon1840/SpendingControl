using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Application.Interfaces
{
    public interface IDepositService
    {
        Task<IEnumerable<Deposit>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null);
        Task<Deposit> CreateAsync(Deposit deposit);
    }
}