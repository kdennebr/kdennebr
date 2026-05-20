namespace loyalty_application.DTOs;
public class CreateTransactionRequest
{
    public int CustomerId { get; set; }
    public string ExternalTransactionId { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public string Category { get; set; } = string.Empty;
}