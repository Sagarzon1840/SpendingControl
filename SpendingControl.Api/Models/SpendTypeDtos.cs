using System;
using System.ComponentModel.DataAnnotations;

namespace SpendingControl.Api.Models
{
    public class SpendTypeCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }

    public class SpendTypePatchDto
    {
        [StringLength(200)]
        public string? Name { get; set; }
    }

    public class SpendTypeUpdateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }

    public class SpendTypeResponseDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
