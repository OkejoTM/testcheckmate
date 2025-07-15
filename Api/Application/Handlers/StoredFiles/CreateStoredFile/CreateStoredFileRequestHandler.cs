using Domain.Entities;
using Infrustructure.Database;
using Infrustructure.Services;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;

namespace Application.Handlers.StoredFiles.CreateStoredFile;

public class CreateStoredFileRequestHandler : BaseRequestHandler<CreateStoredFileRequest, Guid> {
    private readonly IStorageService _storageService;
    private readonly CheckMateDbContext _dbContext;

    public CreateStoredFileRequestHandler(IStorageService storageService, CheckMateDbContext dbContext, ILogger<CreateStoredFileRequestHandler> logger)
        : base(logger)
    {
        _storageService = storageService;
        _dbContext = dbContext;
    }

    protected override async Task<Result<Guid>> HandleRequest(CreateStoredFileRequest request, CancellationToken ct) {
        StoredFile result = new StoredFile {
            Name = request.File.FileName,
            Path = Guid.NewGuid() + Path.GetExtension(request.File.FileName),
            MimeType = MimeKit.MimeTypes.GetMimeType(request.File.FileName)
        };

        await _storageService.SaveAsync(result, request.File.OpenReadStream());
        _logger.LogTrace($"File was successfully saved at path: {result.Path}.");

        await _dbContext.AddAsync(result, ct);
        await _dbContext.SaveChangesAsync(ct);
        _logger.LogTrace($"File was successfully saved to the database: {result.Path}.");

        return result.Id;
    }
}
