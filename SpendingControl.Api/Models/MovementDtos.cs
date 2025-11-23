using System;

namespace SpendingControl.Api.Models
{
    public class MovementResponseDto
    {
        public DateTime Date { get; set; }
        public Guid FundId { get; set; }
        public string Type { get; set; } = string.Empty; // Deposit or Expense
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
