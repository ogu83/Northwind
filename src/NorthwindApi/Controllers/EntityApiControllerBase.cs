using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public abstract class EntityApiControllerBase<T, S, IDT>(IBaseService<T, IDT> service) 
    : ApiControllerBase 
    where T: BaseModel 
    where S: IBaseService<T, IDT> 
{
    private readonly IBaseService<T, IDT> _service = service;

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

    /// <summary>
    /// Get /id. Gets an element with Id
    /// </summary>
    /// <param name="id">Primary Key</param>
    /// <returns>Element to correspnding Id</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<T>> Get(IDT id)
    {
        var retVal = await _service.GetById(id);
        if (retVal == null)
            return NotFound();
        return new ActionResult<T>(retVal);
    }

    /// <summary>
    /// Save the element, Elements with Id equals default value like 0, "", etc.. will be created,
    /// other elements with an actual Id will be updated.
    /// </summary>
    /// <param name="obj">The Element</param>
    /// <returns>Created or Updated Element</returns>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<T>> Put(T obj)
    {
        var retVal = await _service.Update(obj);
        if (retVal == null)
            return NotFound();
        return new ActionResult<T>(retVal);
    }
}