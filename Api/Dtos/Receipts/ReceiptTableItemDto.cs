using Domain.Enums;

namespace Dtos.Receipts;

public class ReceiptTableItemDto {
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public string Comment { get; set; }
    public string State { get; set; }

    public DateTime CreatedAt { get; set; }
    public string UploaderUsername { get; set; }

    public string? Total { get; set; }
    public string? StoreName { get; set; }
    public string? CategoryByStore { get; set; }
}
