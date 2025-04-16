using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public abstract class EntityApiControllerBase<T, S, IDT> : ApiControllerBase 
    where T: class 
    where S: IBaseService<T, IDT> 
{
    private readonly IBaseService<T, IDT> _service;

    protected EntityApiControllerBase(IBaseService<T, IDT> service)
    {
        _service = service;
    }

    /// <summary>
    /// Get List
    /// </summary>
    /// <returns>All Elements</returns>
    /// <response code="200">Returns all elements</response>
    /// <response code="404">If there is no elements</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<T>>> Get()
    {
        var retVal = await _service.GetListAsync();
        if (retVal.Count > 0)
            return new ActionResult<IEnumerable<T>>(retVal);
        else
            return new NotFoundResult();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<T>> Get(IDT id)
    {
        var retVal = await _service.GetById(id);
        return new ActionResult<T>(retVal);
    }
}
