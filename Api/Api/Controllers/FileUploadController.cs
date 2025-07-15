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
public class FileUploadController : MyApiController {
    public FileUploadController(ISender sender) : base(sender) {}

    [HttpPost]
    public async Task<IActionResult> CreateStoredFile(IFormFile file) {
        var result = await _sender.Send(new CreateStoredFileRequest(file));
        return result.ToActionResult();
    }
}
