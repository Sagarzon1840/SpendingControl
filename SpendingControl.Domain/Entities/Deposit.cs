using System;

namespace SpendingControl.Domain.Entities
{
    public class Deposit
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid FundId { get; set; }
        public decimal Amount { get; set; }
    }
}