using Domain.Entities;
using Infrustructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Infrustructure.Services;

public class StandardStorageService : IStorageService {

    private readonly StorageServiceParams _storageParams;
    private readonly ILogger<StandardStorageService> _logger;

    public StandardStorageService(IOptions<StorageServiceParams> storageParams, ILogger<StandardStorageService> logger) {
        _storageParams = storageParams.Value;
        _logger = logger;
    }

    public async Task<string> SaveAsync(StoredFile file, Stream srcFileStream) {
        var destFilePath = Path.Combine(_storageParams.RootPath, file.Path);

        using (var destFileStream = new FileStream(destFilePath, FileMode.Create)) {
            await srcFileStream.CopyToAsync(destFileStream);
        }
        _logger.LogInformation($"Source file was successfully copied to: '{destFilePath}'.");

        return destFilePath;
    }

    public async Task<bool> DeleteAsync(StoredFile file) {
        var fullFilePath = Path.Combine(_storageParams.RootPath, file.Name);
        if (!File.Exists(fullFilePath)) {
            _logger.LogError($"File wasn't found at path: '{fullFilePath}'.");
            return false;
        }

        File.Delete(fullFilePath);
        _logger.LogInformation($"File was successfully deleted: '{fullFilePath}'.");

        return true;
    }
}
