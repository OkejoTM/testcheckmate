using Application.Exceptions;
using Domain.Entities;
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
    private readonly Dictionary<ReceiptState, Func<Receipt, Task<Receipt>>> _handlers;

    public EditReceiptStateRequestHandler(CheckMateDbContext dbContext, IMapper mapper, ILogger<EditReceiptStateRequestHandler> logger)
        : base(logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _handlers = new Dictionary<ReceiptState, Func<Receipt, Task<Receipt>>> {
            { ReceiptState.AwaitConfirm, HandleAwaitConfirm },
            { ReceiptState.AwaitProcess, HandleAwaitProcess },
            { ReceiptState.Rejected, HandleRejected },
            { ReceiptState.InProcessing, HandleInProcessing },
            { ReceiptState.Recognized, HandleRecognized },
            { ReceiptState.NotRecognized, HandleNotRecognized }
        };
    }

    protected override async Task<Result<Guid>> HandleRequest(EditReceiptStateRequest request, CancellationToken ct) {
        var receipt = await _dbContext.Receipts.FindAsync([request.Id], ct);
        if (receipt == null) {
            throw new ObjectNotFoundException($"Receipt #{request.Id} not found.");
        }

        var newState = (ReceiptState)Enum.Parse(typeof(ReceiptState), request.State, true);

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

    private bool IsValidStateTransition(ReceiptState currState, ReceiptState newState) {
        if (currState == newState) {
            return false;
        }

        return (currState, newState) switch {
            (ReceiptState.AwaitConfirm, ReceiptState.AwaitProcess) => true,
            (ReceiptState.AwaitConfirm, ReceiptState.Rejected) => true,

            (ReceiptState.AwaitProcess, ReceiptState.Rejected) => true,
            (ReceiptState.AwaitProcess, ReceiptState.InProcessing) => true,

            (ReceiptState.InProcessing, ReceiptState.NotRecognized) => true,
            (ReceiptState.InProcessing, ReceiptState.Recognized) => true,
            (ReceiptState.InProcessing, ReceiptState.Rejected) => true,

            (ReceiptState.Rejected, ReceiptState.AwaitProcess) => true,

            (ReceiptState.NotRecognized, ReceiptState.Rejected) => true,
            (ReceiptState.NotRecognized, ReceiptState.Recognized) => true,

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
