using MediatR;
using Microsoft.Extensions.Logging;
using Dtos.Receipts;
using System.Security.Claims;

namespace Application.Handlers.Receipts.CreateReceipt;

public sealed record CreateReceiptRequest(ClaimsPrincipal UserInfo, ReceiptDto Dto) : IRequest<Result<Guid>>;
