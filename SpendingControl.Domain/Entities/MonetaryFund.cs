using System.Text.Json.Serialization;

namespace SpendingControl.Domain.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
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

        // Helper for in-memory operations.
        public decimal CurrentBalance { get; private set; }

        public MonetaryFund()
        {
            CurrentBalance = InitialBalance;
        }

        // Apply a deposit to the in-memory balance
        public void ApplyDeposit(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
            CurrentBalance += amount;
        }

        // Apply a withdrawal (spending)
        public void ApplyWithdrawal(decimal amount, decimal setWithdrawal)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));            
            if (setWithdrawal < 0) return;

            CurrentBalance = setWithdrawal;
        }
    }
}