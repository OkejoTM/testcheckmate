using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.StoredFiles.CreateStoredFile;

public sealed record CreateStoredFileRequest(IFormFile File) : IRequest<Result<Guid>>;
