using loyalty_application.Models;

namespace loyalty_application.Models;

public class PointLedgerEntry
{
    public int PointLedgerEntryId { get; set; }
    public int CustomerId { get; set; }
    public int PointsChanged { get; set; }
    public int? PurchaseTransactionId { get; set; }
    public int? RedemptionId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Customer? Customer { get; set; }
    public PurchaseTransaction? PurchaseTransaction { get; set; }
    public Redemption? Redemption { get; set; }
}