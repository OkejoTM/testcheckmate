namespace Infrustructure.Services;

public interface ICloudStorageService {
    Task<string> CreateFolderAsync(string folderPath, bool createSubFolders = false);
    Task<string> UploadFileAsync(string filePath, Stream fileStream, bool createSubFolders = false);
    Task<bool> DeleteFileAsync(string filePath);
    Task<string> MoveFileAsync(string oldFilePath, string newFilePath);
    Task<bool> FileExistsAsync(string filePath);
    Task<bool> FolderExistsAsync(string folderPath);
    Task<string> GetFileUrlAsync(string filePath);
}
