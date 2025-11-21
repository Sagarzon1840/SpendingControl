using System;

namespace SpendingControl.Domain.Entities
{
    public class SpendingDetail
    {
        public Guid Id { get; set; }
        public Guid ExpenseHeaderId { get; set; }
        public int ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }

        //Nav properties
        public SpendingHeader? ExpenseHeader { get; set; }
        public SpendType? ExpenseType { get; set; }        
        public void Validate()
        {
            if (ExpenseTypeId <= 0) throw new ArgumentException("ExpenseTypeId must be provided.", nameof(ExpenseTypeId));
            if (Amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(Amount));
        }
    }
}