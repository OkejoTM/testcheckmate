using Domain.Entities;
using Infrustructure.Database;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Dtos;
using Dtos.Receipts;
using Application.Extensions;

namespace Application.Handlers.Receipts.GetAllReceipts;

public class GetAllReceiptsRequestHandler : BaseRequestHandler<GetAllReceiptsRequest, TableDto<ReceiptTableItemDto>> {
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllReceiptsRequestHandler(CheckMateDbContext dbContext, IMapper mapper, ILogger<GetAllReceiptsRequestHandler> logger)
        : base(logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<TableDto<ReceiptTableItemDto>>> HandleRequest(GetAllReceiptsRequest request, CancellationToken ct) {
        var query = _dbContext.Receipts.AsQueryable();
        query = query.ApplyFilters(request.Filters);
        query = query.ApplySort(request.SortBy, request.SortDir);
        query = query.ApplyPagination(request.ItemsPerPage, request.PageNumber);
        var receipts = await query.ToListAsync(ct);

        var receiptTableItemsList = _mapper.Map<List<ReceiptTableItemDto>>(receipts);
        _logger.LogTrace($"Receipts were successfully retrieved from the database.");

        var totalItemsCount = receipts.Count;
        var totalPagesCount = (int)Math.Ceiling((double)totalItemsCount / request.ItemsPerPage);
        var receiptsTableDto = new TableDto<ReceiptTableItemDto> {
            ItemsPerPage = request.ItemsPerPage,
            PageNumber = request.PageNumber,
            TotalPages = totalPagesCount,
            TotalItems = totalItemsCount,
            Items = receiptTableItemsList.ToArray()
        };

        return receiptsTableDto;
    }
}
