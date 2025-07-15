using MediatR;
using Microsoft.Extensions.Logging;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.CreateReceipt;

public sealed record CreateReceiptRequest(ReceiptDto Dto) : IRequest<Result<Guid>>;
