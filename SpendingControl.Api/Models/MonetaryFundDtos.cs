using System;
using System.ComponentModel.DataAnnotations;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Api.Models
{
    public class MonetaryFundCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public FundType Type { get; set; }
        [StringLength(100)]
        public string? AccountNumberOrDescription { get; set; }
        [Range(0, double.MaxValue)]
        public decimal InitialBalance { get; set; }
    }

    public class MonetaryFundPatchDto
    {
        // Todas opcionales: si vienen null no se modifican
        [StringLength(200)]
        public string? Name { get; set; }
        public FundType? Type { get; set; }
        [StringLength(100)]
        public string? AccountNumberOrDescription { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? InitialBalance { get; set; }
    }

    public class MonetaryFundResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public FundType Type { get; set; }
        public string? AccountNumberOrDescription { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
