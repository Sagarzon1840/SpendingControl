using System;

namespace SpendingControl.Domain.Entities
{
    public enum FundType
    {
        BankAccount,
        Cash
    }

    public class MonetaryFund
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public FundType Type { get; set; }
        public string? AccountNumberOrDescription { get; set; }

        public decimal InitialBalance { get; set; }

        // Current balance can be computed by repositories/services. Keep as mutable helper for in-memory operations.
        public decimal CurrentBalance { get; private set; }

        public MonetaryFund()
        {
            CurrentBalance = InitialBalance;
        }

        // Apply a deposit to the in-memory balance
        public void ApplyDeposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
            CurrentBalance += amount;
        }

        // Apply a withdrawal (spending)
        public void ApplyWithdrawal(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
            CurrentBalance -= amount;
        }
    }
}