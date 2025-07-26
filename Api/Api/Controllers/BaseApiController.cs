using Application;
using Infrustructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseApiController : ControllerBase {
    protected readonly ISender Sender;

    public BaseApiController(ISender sender) {
        Sender = sender;
    }
}
