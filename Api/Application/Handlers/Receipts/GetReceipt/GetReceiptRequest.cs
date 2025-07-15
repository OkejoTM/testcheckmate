using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Dtos.Receipts;

namespace Application.Handlers.Receipts.GetReceipt;

public sealed record GetReceiptRequest(Guid Id) : IRequest<Result<ReceiptViewDto>>;
