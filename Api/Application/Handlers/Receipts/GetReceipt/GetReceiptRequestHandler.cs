using Application.Exceptions;
using Domain.Entities;
using Infrustructure.Database;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MimeKit.Utils;
using AutoMapper;
using Dtos.Receipts;
using Shared.Repositories;

namespace Application.Handlers.Receipts.GetReceipt;

public class GetReceiptRequestHandler : BaseRequestHandler<GetReceiptRequest, ReceiptViewDto> {
    private readonly ILdapRepository _ldapRepository;
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetReceiptRequestHandler(ILdapRepository ldapRepository, CheckMateDbContext dbContext, IMapper mapper, ILogger<GetReceiptRequestHandler> logger)
        : base(logger)
    {
        _ldapRepository = ldapRepository;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<ReceiptViewDto>> HandleRequest(GetReceiptRequest request, CancellationToken ct) {
        var receipt = await _dbContext.Receipts.FindAsync([request.Id], ct);
        if (receipt == null) {
            throw new ObjectNotFoundException($"Receipt #{request.Id} not found.");
        }

        var result = _mapper.Map<ReceiptViewDto>(receipt);
        result.Items.AddRange(
            await _dbContext.ReceiptItems
                .Where(e => e.ReceiptId == request.Id)
                .Select(e => _mapper.Map<ReceiptItemViewDto>(e))
                .ToListAsync(ct)
        );
        var user = await _ldapRepository.GetUserAsync(receipt.UploadedByUserId);
        result.UploaderUsername = user.UserName;

        _logger.LogTrace($"Receipt #{request.Id} was successfully retrieved from the database.");

        return result;
    }
}
