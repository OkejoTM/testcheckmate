using MediatR;
using Microsoft.Extensions.Logging;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.EditReceiptState;

public sealed record EditReceiptStateRequest(Guid Id, string State) : IRequest<Result<Guid>>;
