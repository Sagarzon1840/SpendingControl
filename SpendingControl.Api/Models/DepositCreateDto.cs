using System;
using System.ComponentModel.DataAnnotations;

namespace SpendingControl.Api.Models
{
    public class DepositCreateDto
    {
        [Required]
        public Guid FundId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
