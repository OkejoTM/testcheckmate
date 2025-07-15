namespace Application;

using Microsoft.Extensions.Logging;
using MediatR;

public abstract class BaseRequestHandler<TRequest, TResponse>
    : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
    protected readonly ILogger<BaseRequestHandler<TRequest, TResponse>> _logger;

    public BaseRequestHandler(ILogger<BaseRequestHandler<TRequest, TResponse>> logger) {
        _logger = logger;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, CancellationToken ct) {
        try {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            return await HandleRequest(request, ct);
        }
        catch (Exception ex) {
            _logger.LogError(ex, $"Error handling request {typeof(TRequest).Name}");
            return ex;
        }
    }

    protected abstract Task<Result<TResponse>> HandleRequest(TRequest request, CancellationToken ct);
}
