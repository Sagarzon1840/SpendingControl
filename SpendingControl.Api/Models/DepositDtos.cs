using System;
using System.ComponentModel.DataAnnotations;

namespace SpendingControl.Api.Models
{
    public class DepositResponseDto
    {
        public Guid Id { get; set; }
        public Guid FundId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
