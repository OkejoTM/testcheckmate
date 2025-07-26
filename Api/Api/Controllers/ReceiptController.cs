using Application.Handlers.Receipts.CreateReceipt;
using Application.Handlers.Receipts.EditReceipt;
using Application.Handlers.Receipts.EditReceiptState;
using Application.Handlers.Receipts.GetAllReceipts;
using Application.Handlers.Receipts.GetReceipt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Dtos;
using Dtos.Receipts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Controllers;

public class ReceiptController : BaseApiController {
    public ReceiptController(ISender sender) : base(sender) {}

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateReceipt(ReceiptDto dto) {
        var result = await Sender.Send(new CreateReceiptRequest(this.User, dto));
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditReceipt(Guid id, [FromBody] string state) {
        var result = await Sender.Send(new EditReceiptStateRequest(id, state));
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}/file")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditReceipt(Guid id, ReceiptDto dto) {
        var result = await Sender.Send(new EditReceiptRequest(id, dto));
        return result.ToActionResult();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(TableDto<ReceiptTableItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllReceipts([FromQuery] int itemsPerPage = 20, [FromQuery] int pageNumber = 1) {
        var request = new GetAllReceiptsRequest(
            this.User,
            itemsPerPage,
            pageNumber,
            this.CreateFilters(),
            this.GetSortField(),
            this.GetSortDirection()
        );

        var result = await Sender.Send(request);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReceiptViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReceipt(Guid id) {
        var result = await Sender.Send(new GetReceiptRequest(id));
        return result.ToActionResult();
    }
}
