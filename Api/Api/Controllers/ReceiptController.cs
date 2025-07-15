using Application.Handlers.Receipts.CreateReceipt;
using Application.Handlers.Receipts.EditReceipt;
using Application.Handlers.Receipts.EditReceiptState;
using Application.Handlers.Receipts.GetAllReceipts;
using Application.Handlers.Receipts.GetReceipt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Dtos.Receipts;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

public class ReceiptController : MyApiController {
    public ReceiptController(ISender sender) : base(sender) {}

    [HttpPost]
    public async Task<IActionResult> CreateReceipt(ReceiptDto dto) {
        var result = await _sender.Send(new CreateReceiptRequest(dto));
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditReceipt(Guid id, [FromBody] string state) {
        var result = await _sender.Send(new EditReceiptStateRequest(id, state));
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}/file")]
    public async Task<IActionResult> EditReceipt(Guid id, ReceiptDto dto) {
        var result = await _sender.Send(new EditReceiptRequest(id, dto));
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReceipts([FromQuery] int itemsPerPage = 20, [FromQuery] int pageNumber = 1) {
        var request = new GetAllReceiptsRequest(
            itemsPerPage,
            pageNumber,
            this.CreateFilters(),
            this.GetSortField(),
            this.GetSortDirection()
        );

        var result = await _sender.Send(request);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReceipt(Guid id) {
        var result = await _sender.Send(new GetReceiptRequest(id));
        return result.ToActionResult();
    }
}
