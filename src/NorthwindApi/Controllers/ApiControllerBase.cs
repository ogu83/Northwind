using Microsoft.AspNetCore.Mvc;

namespace NorthwindApi.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{

}
