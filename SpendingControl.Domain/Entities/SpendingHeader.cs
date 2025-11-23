using System;
using System.Collections.Generic;
using System.Linq;

namespace SpendingControl.Domain.Entities
{
    public enum DocumentType
    {
        Receipt,
        Invoice,
        Other
    }

    public class SpendingHeader
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public Guid MonetaryFundId { get; set; }
        public string? Observations { get; set; }
        public string? MerchantName { get; set; }
        public DocumentType DocumentType { get; set; }
        public List<SpendingDetail> Details { get; set; } = new List<SpendingDetail>();
        public decimal TotalAmount => Details?.Sum(d => d.Amount) ?? 0m;
        public bool IsValid { get; set; } = true; // mark invalid when overdraft occurs

        // Validate that a header always has at least one detail before saving
        public void ValidateHasDetails()
        {
            if (Details == null || !Details.Any())
            {
                throw new InvalidOperationException("Expense header must have at least one detail.");
            }
        }
    }
}