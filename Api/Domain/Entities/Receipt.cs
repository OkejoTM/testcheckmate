using System.Reflection;

namespace Domain.Entities;

public enum ReceiptState {
    AwaitConfirm,
    AwaitProcess,
    Rejected,
    InProcessing,
    Recognized,
    NotRecognized
}

public class Receipt : Entity {
    public Guid FileId { get; set; }
    public string? Comment { get; set; }
    public ReceiptState State { get; set; } = ReceiptState.AwaitConfirm;
}
