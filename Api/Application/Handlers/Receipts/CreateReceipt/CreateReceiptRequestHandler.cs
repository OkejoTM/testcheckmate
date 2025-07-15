using Application.Exceptions;
using Domain.Entities;
using Infrustructure.Database;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;
using AutoMapper;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.CreateReceipt;

public class CreateReceiptRequestHandler : BaseRequestHandler<CreateReceiptRequest, Guid> {
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateReceiptRequestHandler(CheckMateDbContext dbContext, IMapper mapper, ILogger<CreateReceiptRequestHandler> logger)
        : base(logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<Guid>> HandleRequest(CreateReceiptRequest request, CancellationToken ct) {
        if (await _dbContext.StoredFiles.FindAsync([request.Dto.FileId], ct) == null) {
            throw new ObjectNotFoundException($"Receipt's stored file #{request.Dto.FileId} not found.");
        }

        var result = _mapper.Map<Receipt>(request.Dto);

        await _dbContext.AddAsync(result, ct);
        await _dbContext.SaveChangesAsync(ct);
        _logger.LogTrace($"Receipt #{result.Id} was successfully saved to the database.");

        return result.Id;
    }
}
