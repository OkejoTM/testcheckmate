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
public class MyApiController : ControllerBase {
    protected readonly ISender _sender;

    public MyApiController(ISender sender) {
        _sender = sender;
    }
}
