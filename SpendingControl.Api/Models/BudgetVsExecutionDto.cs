using System;

namespace SpendingControl.Api.Models
{
    public class BudgetVsExecutionDto
    {
        public string ExpenseTypeName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Executed { get; set; }
    }
}
