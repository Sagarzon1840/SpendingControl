using System;

namespace SpendingControl.Domain.Entities
{
    public class Budget
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int SpendTypeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }

        // Convenience property to represent YYYYMM if needed
        public int YearMonth => Year * 100 + Month;
    }
}