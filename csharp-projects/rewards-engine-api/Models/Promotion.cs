namespace loyalty_application.Models;

public class Promotion
{
    public int PromotionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PromotionType { get; set; } = string.Empty; 
    public string? Category { get; set; }
    public decimal? Multiplier { get; set; }
    public int? BonusPoints { get; set; }
    public decimal? MinimumSpend { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}