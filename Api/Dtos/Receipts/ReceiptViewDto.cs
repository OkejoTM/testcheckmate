using Domain.Enums;

namespace Dtos.Receipts;

public class ReceiptViewDto {
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public string Comment { get; set; }
    public string State { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? UploaderUsername { get; set; }
    public string? OperationType { get; set; }
    public string? CategoryByStore { get; set; }
    public string? CategoryByPrice { get; set; }
    public string? CloudLink { get; set; }

    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? Total { get; set; }
    public string? FiscalNumber { get; set; }
    public string? FiscalDocument { get; set; }
    public string? FiscalSign { get; set; }
    public string? INN { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? StoreName { get; set; }
    public string? VatAmount { get; set; }

    public List<ReceiptItemViewDto> Items { get; set; } = new();
}
