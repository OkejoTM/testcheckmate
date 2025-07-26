namespace Domain.Enums;

public enum EReceiptState {
    AwaitConfirm,
    AwaitProcess,
    Rejected,
    InProcessing,
    Recognized,
    NotRecognized
}
