namespace SpendingControl.Domain.Entities
{
    public class OverdraftWarning
    {
        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Executed { get; set; }
        public decimal Overdraft { get; set; }
    }
}
