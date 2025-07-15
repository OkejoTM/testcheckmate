namespace Application;

public class Result<T> {
    public bool IsSuccess { get; }
    public T Value { get; }
    public Exception? Error { get; }

    public Result(T value) {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    public Result(Exception? error) {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failure(Exception error) => new Result<T>(error);

    public static implicit operator Result<T>(Exception error) => Result<T>.Failure(error);
    public static implicit operator Result<T>(T value) => Result<T>.Success(value);

    public override string ToString() => IsSuccess ? $"Success: {Value}" : $"Failure: {Error.Message}";
}
