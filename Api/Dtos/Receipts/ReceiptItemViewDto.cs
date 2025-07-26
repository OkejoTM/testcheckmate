namespace Dtos.Receipts;

public class ReceiptItemViewDto {
    public string ProductName { get; set; }
    public string Quantity { get; set; }
    public string Unit { get; set; }
    public string PricePerUnit { get; set; }
    public string TotalPrice { get; set; }
}
