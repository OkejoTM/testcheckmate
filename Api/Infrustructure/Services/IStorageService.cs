using Domain.Entities;

namespace Infrustructure.Services;

public interface IStorageService {
    Task<string> SaveAsync(StoredFile file, Stream srcFileStream);
    Task<bool> DeleteAsync(StoredFile file);
}
