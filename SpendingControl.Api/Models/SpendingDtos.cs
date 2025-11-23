using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Api.Models
{
    public class SpendingDetailCreateDto
    {
        [Required]
        public int ExpenseTypeId { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
    }

    public class SpendingHeaderCreateDto
    {
        [Required]
        public Guid MonetaryFundId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [StringLength(200)]
        public string? MerchantName { get; set; }
        [StringLength(1000)]
        public string? Observations { get; set; }
        [Required]
        public DocumentType DocumentType { get; set; }
        [MinLength(1)]
        public List<SpendingDetailCreateDto> Details { get; set; } = new();
    }

    public class SpendingDetailResponseDto
    {
        public Guid Id { get; set; }
        public int ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class OverdraftWarningDto
    {
        public string ExpenseTypeName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Executed { get; set; }
        public decimal Overdraft { get; set; }
    }

    public class SpendingHeaderResponseDto
    {
        public Guid Id { get; set; }
        public Guid MonetaryFundId { get; set; }
        public DateTime Date { get; set; }
        public string? MerchantName { get; set; }
        public string? Observations { get; set; }
        public DocumentType DocumentType { get; set; }
        public IEnumerable<SpendingDetailResponseDto> Details { get; set; } = Array.Empty<SpendingDetailResponseDto>();
        public IEnumerable<OverdraftWarningDto> OverdraftWarnings { get; set; } = Array.Empty<OverdraftWarningDto>();
    }
}
