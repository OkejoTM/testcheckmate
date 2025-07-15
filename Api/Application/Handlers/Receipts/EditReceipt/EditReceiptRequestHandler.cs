using Application.Exceptions;
using Domain.Entities;
using Infrustructure.Database;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;
using AutoMapper;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.EditReceipt;

public class EditReceiptRequestHandler : BaseRequestHandler<EditReceiptRequest, Guid> {
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public EditReceiptRequestHandler(CheckMateDbContext dbContext, IMapper mapper, ILogger<EditReceiptRequestHandler> logger)
        : base(logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<Guid>> HandleRequest(EditReceiptRequest request, CancellationToken ct) {
        var receipt = await _dbContext.Receipts.FindAsync([request.Id], ct);
        if (receipt == null) {
            throw new ObjectNotFoundException($"Receipt #{request.Id} not found.");
        }
        if (await _dbContext.StoredFiles.FindAsync([request.Dto.FileId], ct) == null) {
            throw new ObjectNotFoundException($"Receipt's stored file #{request.Dto.FileId} not found.");
        }

        _mapper.Map(request.Dto, receipt);

        await _dbContext.SaveChangesAsync(ct);
        _logger.LogTrace($"Receipt #{receipt.Id} was successfully edited and saved to the database.");

        return receipt.Id;
    }
}
