using Infrustructure;
using Application.Handlers.StoredFiles.CreateStoredFile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("/upload")]
public class FileUploadController : BaseApiController {
    public FileUploadController(ISender sender) : base(sender) {}

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStoredFile(IFormFile file) {
        var result = await Sender.Send(new CreateStoredFileRequest(file));
        return result.ToActionResult();
    }
}
