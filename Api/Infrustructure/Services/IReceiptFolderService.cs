using Domain.Entities;
using Shared.Entities;

namespace Infrustructure.Services;

public interface IReceiptFolderService {
    Task<string> SaveReceiptToCloudAsync(Receipt receipt, StoredFile storedFile, Stream fileStream, User user, CancellationToken ct);
    Task<bool> DeleteReceiptFromCloudAsync(Receipt receipt, StoredFile storedFile, User user, CancellationToken ct);
}
