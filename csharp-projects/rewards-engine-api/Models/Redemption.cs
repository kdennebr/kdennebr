namespace loyalty_application.Models;

public class Redemption
{
    public int RedemptionId { get; set; }
    public int CustomerId { get; set; }
    public int RewardId { get; set; }
    public int PointsSpent { get; set; }
    public DateTime RedemptionDate { get; set; } = DateTime.UtcNow;
    public Customer? Customer { get; set; }
    public Reward? Reward { get; set; }
}