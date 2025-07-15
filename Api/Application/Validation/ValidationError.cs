namespace Application.Validation;

public sealed record ValidationError(string PropertyName, string? Description = null) {
    public static readonly ValidationError None = new("");
}
