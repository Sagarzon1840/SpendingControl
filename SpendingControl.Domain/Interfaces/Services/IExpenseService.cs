using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Services
{
    public interface IExpenseService
    {        
        Task<(SpendingHeader header, IEnumerable<int> overdrawnExpenseTypeIds)> CreateExpenseAsync(SpendingHeader header);
    }
}
