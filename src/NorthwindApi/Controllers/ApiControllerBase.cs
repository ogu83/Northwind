using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Controllers;

namespace NorthwindApi.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{

}
