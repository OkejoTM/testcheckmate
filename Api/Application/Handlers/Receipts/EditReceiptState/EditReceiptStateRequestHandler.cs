using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;
using Infrustructure.Database;
using Infrustructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MimeKit.Utils;
using AutoMapper;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.EditReceiptState;

public class EditReceiptStateRequestHandler : BaseRequestHandler<EditReceiptStateRequest, Guid> {
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly Dictionary<EReceiptState, Func<Receipt, Task<Receipt>>> _handlers;

    public EditReceiptStateRequestHandler(CheckMateDbContext dbContext, IMapper mapper, ILogger<EditReceiptStateRequestHandler> logger)
        : base(logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _handlers = new Dictionary<EReceiptState, Func<Receipt, Task<Receipt>>> {
            { EReceiptState.AwaitConfirm, HandleAwaitConfirm },
            { EReceiptState.AwaitProcess, HandleAwaitProcess },
            { EReceiptState.Rejected, HandleRejected },
            { EReceiptState.InProcessing, HandleInProcessing },
            { EReceiptState.Recognized, HandleRecognized },
            { EReceiptState.NotRecognized, HandleNotRecognized }
        };
    }

    protected override async Task<Result<Guid>> HandleRequest(EditReceiptStateRequest request, CancellationToken ct) {
        var receipt = await _dbContext.Receipts.FindAsync([request.Id], ct);
        if (receipt == null) {
            throw new ObjectNotFoundException($"Receipt #{request.Id} not found.");
        }

        var newState = (EReceiptState)Enum.Parse(typeof(EReceiptState), request.State, true);

        if (!IsValidStateTransition(receipt.State, newState)) {
            _logger.LogError($"Invalid state transition of receipt #{receipt.Id}: {receipt.State} -> {newState}.");
            throw new ArgumentException($"Invalid state transition: {receipt.State} -> {newState}");
        }

        if (!_handlers.TryGetValue(newState, out var handler)) {
            _logger.LogError($"Failed to handle state transition of receipt #{receipt.Id}: {receipt.State} -> {newState}.");
            throw new ArgumentException($"Unsupported state handling.");
        }
        receipt.State = newState;
        await _dbContext.SaveChangesAsync(ct);
        _logger.LogTrace($"Receipt #{receipt.Id}'s state was successfully edited and saved to the database.");

        var result = await handler(receipt);
        _logger.LogTrace($"Receipt #{receipt.Id} was successfully processed while changing to state {newState}.");
        return result.Id;
    }

    private bool IsValidStateTransition(EReceiptState currState, EReceiptState newState) {
        if (currState == newState) {
            return false;
        }

        return (currState, newState) switch {
            (EReceiptState.AwaitConfirm, EReceiptState.AwaitProcess) => true,
            (EReceiptState.AwaitConfirm, EReceiptState.Rejected) => true,

            (EReceiptState.AwaitProcess, EReceiptState.Rejected) => true,
            (EReceiptState.AwaitProcess, EReceiptState.InProcessing) => true,

            (EReceiptState.InProcessing, EReceiptState.NotRecognized) => true,
            (EReceiptState.InProcessing, EReceiptState.Recognized) => true,
            (EReceiptState.InProcessing, EReceiptState.Rejected) => true,

            (EReceiptState.Rejected, EReceiptState.AwaitProcess) => true,

            (EReceiptState.NotRecognized, EReceiptState.Rejected) => true,
            (EReceiptState.NotRecognized, EReceiptState.Recognized) => true,

            _ => false
        };
    }

    private async Task<Receipt> HandleAwaitConfirm(Receipt receipt) {
        return receipt;
    }

    private async Task<Receipt> HandleAwaitProcess(Receipt receipt) {
        return receipt;
    }

    private async Task<Receipt> HandleRejected(Receipt receipt) {
        return receipt;
    }

    private async Task<Receipt> HandleInProcessing(Receipt receipt) {
        return receipt;
    }

    private async Task<Receipt> HandleRecognized(Receipt receipt) {
        return receipt;
    }

    private async Task<Receipt> HandleNotRecognized(Receipt receipt) {
        return receipt;
    }
}
