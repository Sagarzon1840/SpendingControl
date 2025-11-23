using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingControl.Application.Interfaces
{
    public class MovementDto
    {
        public DateTime Date { get; set; }
        public Guid FundId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public interface IMovementService
    {
        Task<IEnumerable<MovementDto>> GetMovementsAsync(Guid userId, DateTime? from = null, DateTime? to = null, int page = 1, int size = 50);
    }
}
