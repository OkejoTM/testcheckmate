using Domain.Enums;
using System.Reflection;

namespace Domain.Entities;

public class Receipt : Entity {
    public Guid FileId { get; set; }
    public string? Comment { get; set; }
    public EReceiptState State { get; set; } = EReceiptState.AwaitConfirm;

    public int UploadedByUserId { get; set; }
    public EOperationType? OperationType { get; set; }
    public ECategoryByStore? CategoryByStore { get; set; }
    public ECategoryByPrice? CategoryByPrice { get; set; }

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

    public List<ReceiptItem> Items { get; set; } = new();
}
