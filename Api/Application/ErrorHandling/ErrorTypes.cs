namespace Application;

public sealed class ErrorTypes {
    public static Error RequestHandleError(string description) => new Error("RequestHandleError", description);
}
