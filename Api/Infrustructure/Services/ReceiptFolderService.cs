using System.IO;
using Domain.Entities;
using Domain.Enums;
using Infrustructure.Database;
using Infrustructure.Extensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Entities;

namespace Infrustructure.Services;

public class ReceiptFolderService : IReceiptFolderService {
    public const string EmployeeCompensationFolderPath = "Компенсации сотрудникам";
    public const string CorporateCardExpensesFolderPath = "Расходы по корпоративной карте";

    private readonly ICloudStorageService _cloudStorageService;
    private readonly CheckMateDbContext _dbContext;
    private readonly ILogger<ReceiptFolderService> _logger;

    public ReceiptFolderService(ICloudStorageService cloudStorageService, CheckMateDbContext dbContext, ILogger<ReceiptFolderService> logger) {
        _cloudStorageService = cloudStorageService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> SaveReceiptToCloudAsync(Receipt receipt, StoredFile storedFile, Stream fileStream, User user, CancellationToken ct) {
        var receiptFilePath = receipt.ConstructCloudFilePath(storedFile, user);
        await _cloudStorageService.UploadFileAsync(receiptFilePath, fileStream, true);

        _logger.LogInformation($"Receipt #{receipt.Id} was successfully saved to Cloud storage.");
        return receiptFilePath;
    }

    public async Task<bool> DeleteReceiptFromCloudAsync(Receipt receipt, StoredFile storedFile, User user, CancellationToken ct) {
        var receiptFilePath = receipt.ConstructCloudFilePath(storedFile, user);
        return await _cloudStorageService.DeleteFileAsync(receiptFilePath);
    }
}
