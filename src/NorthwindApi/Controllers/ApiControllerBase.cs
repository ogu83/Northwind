using Microsoft.AspNetCore.Mvc;

namespace NorthwindApi.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ILogger _logger;

    protected ApiControllerBase(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(GetType().FullName!);
    }
}
