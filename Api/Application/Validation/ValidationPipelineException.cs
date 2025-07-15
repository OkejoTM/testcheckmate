namespace Application.Validation;

public class ValidationPipelineException : Exception {
    public List<ValidationError> Errors { get; }

    public ValidationPipelineException(List<ValidationError> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}
