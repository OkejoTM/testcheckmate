using System.IO;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Infrustructure.Options;
using WebDav;

namespace Infrustructure.Services;

public class NextCloudStorageService : ICloudStorageService {
    private readonly NextCloudConnectionParams _connectParams;
    private readonly WebDavClientParams _clientOptions;
    private readonly ILogger<NextCloudStorageService> _logger;

    public NextCloudStorageService(IOptions<NextCloudConnectionParams> connectParams, ILogger<NextCloudStorageService> logger) {
        _connectParams = connectParams.Value;
        _logger = logger;
        _clientOptions = new WebDavClientParams {
            BaseAddress = new Uri(_connectParams.BaseUrl),
            Credentials = new NetworkCredential(_connectParams.Username, _connectParams.Password)
        };
    }

    public async Task<string> CreateFolderAsync(string folderPath, bool createSubFolders = false) {
        if (createSubFolders) {
            string parentFolder = Path.GetDirectoryName(folderPath);
            if (!string.IsNullOrEmpty(parentFolder)) {
                await CreateFolderAsync(parentFolder, true);
            }
        }

        var fullPath = Path.Combine(_connectParams.BasePath, folderPath);
        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.Mkcol(fullPath);

            if (!response.IsSuccessful) {
                _logger.LogError($"Failed to create folder. Status: {response.StatusCode}, Reason: {response.Description}");
                throw new Exception($"WebDAV error. Failed to create folder: #{response.StatusCode}: {response.Description}");
            }

            _logger.LogInformation($"Folder was successfully created: '{fullPath}'.");
        }

        return fullPath;
    }

    public async Task<string> UploadFileAsync(string filePath, Stream fileStream, bool createSubFolders = false) {
        if (createSubFolders) {
            await CreateFolderAsync(Path.GetDirectoryName(filePath), true);
        }

        var fullPath = Path.Combine(_connectParams.BasePath, filePath);
        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.PutFile(fullPath, fileStream);

            if (!response.IsSuccessful) {
                _logger.LogError($"Failed to upload file. Status: {response.StatusCode}, Reason: {response.Description}");
                throw new Exception($"WebDAV error. Failed to upload file: #{response.StatusCode}: {response.Description}");
            }

            _logger.LogInformation($"File was successfully uploaded: '{fullPath}'.");
        }

        return fullPath;
    }

    public async Task<bool> DeleteFileAsync(string filePath) {
        var fullPath = Path.Combine(_connectParams.BasePath, filePath);

        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.Delete(fullPath);

            if (!response.IsSuccessful) {
                _logger.LogError($"Failed to delete file. Status: {response.StatusCode}, Reason: {response.Description}");
                return false;
            }

            _logger.LogInformation($"File was successfully deleted: '{fullPath}'.");
        }

        return true;
    }

    public async Task<string> MoveFileAsync(string oldFilePath, string newFilePath) {
        var oldFullPath = Path.Combine(_connectParams.BasePath, oldFilePath);
        var newFullPath = Path.Combine(_connectParams.BasePath, newFilePath);

        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.Move(oldFullPath, newFullPath);

            if (!response.IsSuccessful) {
                _logger.LogError($"Failed to move file. Status: {response.StatusCode}, Reason: {response.Description}");
                throw new Exception($"WebDAV error. Failed to move file: #{response.StatusCode}: {response.Description}");
            }

            _logger.LogInformation($"File was successfully moved: '{newFullPath}'.");
        }

        return newFullPath;
    }

    public async Task<bool> FileExistsAsync(string filePath) {
        var fullPath = Path.Combine(_connectParams.BasePath, filePath);

        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.Propfind(fullPath);
            return response.IsSuccessful && !response.Resources.First().IsCollection;
        }
    }

    public async Task<bool> FolderExistsAsync(string folderPath) {
        var fullPath = Path.Combine(_connectParams.BasePath, folderPath);

        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.Propfind(fullPath);
            return response.IsSuccessful && response.Resources.First().IsCollection;
        }
    }

    public async Task<string> GetFileUrlAsync(string filePath) {
        var fullPath = Path.Combine(_connectParams.BasePath, filePath);

        using (var client = new WebDavClient(_clientOptions)) {
            var response = await client.Propfind(fullPath);

            if (!(response.IsSuccessful && !response.Resources.First().IsCollection)) {
                _logger.LogError($"Failed to find file. Status: {response.StatusCode}, Reason: {response.Description}");
                throw new Exception($"WebDAV error. Failed to find file: #{response.StatusCode}: {response.Description}");
            }
        }

        return fullPath;
    }
}
