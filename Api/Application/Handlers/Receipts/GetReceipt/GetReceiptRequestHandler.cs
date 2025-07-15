using Application.Exceptions;
using Domain.Entities;
using Infrustructure.Database;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MimeKit.Utils;
using AutoMapper;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.GetReceipt;

public class GetReceiptRequestHandler : BaseRequestHandler<GetReceiptRequest, ReceiptViewDto> {
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetReceiptRequestHandler(CheckMateDbContext dbContext, IMapper mapper, ILogger<GetReceiptRequestHandler> logger)
        : base(logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<ReceiptViewDto>> HandleRequest(GetReceiptRequest request, CancellationToken ct) {
        var receipt = await _dbContext.Receipts.FindAsync([request.Id], ct);
        if (receipt == null) {
            throw new ObjectNotFoundException($"Receipt #{request.Id} not found.");
        }

        var result = _mapper.Map<ReceiptViewDto>(receipt);

        _logger.LogTrace($"Receipt #{request.Id} was successfully retrieved from the database.");

        return result;
    }
}
