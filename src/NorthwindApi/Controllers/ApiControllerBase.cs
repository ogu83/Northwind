using Microsoft.AspNetCore.Mvc;

namespace NorthwindApi.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ILogger _logger;

    protected ApiControllerBase(ILoggerFactory loggerFactory)
    {
        // this.GetType() is the concrete controller type at runtime
        _logger = loggerFactory.CreateLogger(GetType().FullName!);
    }
}
