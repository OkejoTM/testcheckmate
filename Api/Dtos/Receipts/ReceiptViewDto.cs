namespace Dtos.Receipts;

public class ReceiptViewDto {
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public string Comment { get; set; }
    public string State { get; set; }
}
