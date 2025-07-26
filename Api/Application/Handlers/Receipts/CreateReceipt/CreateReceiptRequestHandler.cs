using System.Security.Claims;
using Infrustructure.Options;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;
using Infrustructure.Database;
using Infrustructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;
using AutoMapper;
using Dtos.Receipts;
using Shared;
using Shared.Repositories;

namespace Application.Handlers.Receipts.CreateReceipt;

public class CreateReceiptRequestHandler : BaseRequestHandler<CreateReceiptRequest, Guid> {
    private readonly ILdapRepository _ldapRepository;
    private readonly IReceiptFolderService _receiptFolderService;
    private readonly StorageServiceParams _storageParams;
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateReceiptRequestHandler(ILdapRepository ldapRepository, IReceiptFolderService receiptFolderService, IOptions<StorageServiceParams> storageParams, CheckMateDbContext dbContext, IMapper mapper, ILogger<CreateReceiptRequestHandler> logger)
        : base(logger)
    {
        _ldapRepository = ldapRepository;
        _receiptFolderService = receiptFolderService;
        _storageParams = storageParams.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<Guid>> HandleRequest(CreateReceiptRequest request, CancellationToken ct) {
        var storedFile = await _dbContext.StoredFiles.FindAsync([request.Dto.FileId], ct);
        if (storedFile == null) {
            throw new ObjectNotFoundException($"Receipt's stored file #{request.Dto.FileId} not found.");
        }

        var uploaderUserId = int.Parse(request.UserInfo.FindFirst(ClaimTypes.NameIdentifier).Value);
        var uploaderUserRoleName = request.UserInfo.FindFirst(ClaimTypes.Role).Value;

        var result = _mapper.Map<Receipt>(request.Dto);
        result.UploadedByUserId = uploaderUserId;

        if (uploaderUserRoleName == Roles.OfficeManager) {
            result.State = EReceiptState.AwaitProcess;
            // Further sending to cv
        }

        await _dbContext.AddAsync(result, ct);
        await _dbContext.SaveChangesAsync(ct);
        _logger.LogTrace($"Receipt #{result.Id} created by user #{uploaderUserId} ({uploaderUserRoleName}) was successfully saved to the database.");

        using (var storedFileStream = File.OpenRead(Path.Combine(_storageParams.RootPath, storedFile.Path))) {
            var user = await _ldapRepository.GetUserAsync(result.UploadedByUserId);
            await _receiptFolderService.SaveReceiptToCloudAsync(result, storedFile, storedFileStream, user, ct);
        }

        return result.Id;
    }
}
