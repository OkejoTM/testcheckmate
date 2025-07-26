using System.Security.Claims;
using Domain.Entities;
using Infrustructure.Database;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Dtos;
using Dtos.Receipts;
using Application.Extensions;
using Shared.Repositories;

namespace Application.Handlers.Receipts.GetAllReceipts;

public class GetAllReceiptsRequestHandler : BaseRequestHandler<GetAllReceiptsRequest, TableDto<ReceiptTableItemDto>> {
    private readonly ILdapRepository _ldapRepository;
    private readonly CheckMateDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllReceiptsRequestHandler(ILdapRepository ldapRepository, CheckMateDbContext dbContext, IMapper mapper, ILogger<GetAllReceiptsRequestHandler> logger)
        : base(logger)
    {
        _ldapRepository = ldapRepository;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<Result<TableDto<ReceiptTableItemDto>>> HandleRequest(GetAllReceiptsRequest request, CancellationToken ct) {
        var receipts = await _dbContext.Receipts.AsQueryable()
            .ApplyEmployeeReceiptFilter(request.UserInfo)
            .ApplyFilters(request.Filters)
            .ApplySort(request.SortBy, request.SortDir)
            .ApplyPagination(request.ItemsPerPage, request.PageNumber)
            .ToListAsync(ct);

        var users = await _ldapRepository.GetUsersAsync();
        var usersByIdsDict = users.ToDictionary(x => x.Id, x => x);

        var receiptTableItemsList = receipts
            .Select(x => {
                var receiptItem = _mapper.Map<ReceiptTableItemDto>(x);
                receiptItem.UploaderUsername = usersByIdsDict[x.UploadedByUserId].UserName;
                return receiptItem;
            });

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
