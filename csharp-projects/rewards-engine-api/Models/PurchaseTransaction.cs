using loyalty_application.Models;

namespace loyalty_application.Models;
public class PurchaseTransaction
{
    public int PurchaseTransactionId { get; set; }
    public int CustomerId { get; set; }
    public string ExternalTransactionId { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string Category { get; set; } = string.Empty;
    public int BasePointsEarned { get; set; }
    public int BonusPointsEarned { get; set; }
    public int TotalPointsEarned { get; set; }
    public Customer? Customer { get; set; }
}