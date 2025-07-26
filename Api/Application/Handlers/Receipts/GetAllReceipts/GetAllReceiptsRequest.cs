using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Domain.Entities;
using Dtos;
using Dtos.Receipts;
using System.Security.Claims;

namespace Application.Handlers.Receipts.GetAllReceipts;

public sealed record GetAllReceiptsRequest(
    ClaimsPrincipal UserInfo,
    int ItemsPerPage,
    int PageNumber,
    KeyValuePair<string, string>[] Filters,
    string? SortBy,
    ESortDirection SortDir
) : IRequest<Result<TableDto<ReceiptTableItemDto>>>;
