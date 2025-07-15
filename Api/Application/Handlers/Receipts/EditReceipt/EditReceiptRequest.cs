using MediatR;
using Microsoft.Extensions.Logging;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.EditReceipt;

public sealed record EditReceiptRequest(Guid Id, ReceiptDto Dto) : IRequest<Result<Guid>>;
