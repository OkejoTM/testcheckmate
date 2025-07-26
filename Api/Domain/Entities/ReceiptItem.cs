namespace Domain.Entities;

public class ReceiptItem : Entity {
    public string ProductName { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string PricePerUnit { get; set; } = string.Empty;
    public string TotalPrice { get; set; } = string.Empty;

	public Guid ReceiptId { get; set; }
    public Receipt Receipt { get; set; } = null!;
}
