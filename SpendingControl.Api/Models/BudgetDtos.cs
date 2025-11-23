using System;
using System.ComponentModel.DataAnnotations;

namespace SpendingControl.Api.Models
{
    public class BudgetCreateDto
    {
        [Required]
        public int SpendTypeId { get; set; }
        [Required]
        public int Year { get; set; }
        [Range(1,12)]
        public int Month { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }

    public class BudgetPatchDto
    {
        [Range(0.01, double.MaxValue)]
        public decimal? Amount { get; set; }
    }

    public class BudgetResponseDto
    {
        public Guid Id { get; set; }
        public int SpendTypeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }
}
