using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingControl.Application.Interfaces
{
    public class BudgetVsExecutionItem
    {
        public string ExpenseTypeName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Executed { get; set; }
    }

    public interface IReportService
    {
        Task<IEnumerable<BudgetVsExecutionItem>> GetBudgetVsExecutionAsync(Guid userId, DateTime from, DateTime to);
    }
}
