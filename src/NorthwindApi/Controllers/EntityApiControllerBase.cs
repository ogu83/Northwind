using System.Collections;
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
    public async Task<ActionResult<List<T>>> Get()
    {
        var retVal = await _service.GetListAsync();

        if (retVal.Count > 0)
            return new ActionResult<List<T>>(retVal);
        else
            return new NotFoundResult();
    }

    /// <summary>
    /// Get List
    /// </summary>
    /// <param name="skip">Skip element count for paging</param>
    /// <param name="take">Take element count for paging</param>
    /// <returns>All Elements</returns>
    /// <response code="200">Returns all elements</response>
    /// <response code="404">If there is no elements</response>
    [HttpGet("skip/{skip}/take/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedList<T>>> Get(int skip, int take)
    {
        var retVal = await _service.GetPagedListAsync(skip, take);
        if (retVal.ItemCount > 0)
            return new ActionResult<PagedList<T>>(retVal);
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
    public virtual async Task<ActionResult<T>> Get(IDT id)
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

    /// <summary>
    /// Create the element
    /// </summary>
    /// <param name="obj">Element to be created</param>
    /// <returns>Created Element</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<T>> Post(T obj)
    {
        var retVal = await _service.Add(obj);
        if (retVal == null)
            return NotFound();
        return new ActionResult<T>(retVal);
    }

    /// <summary>
    /// Deletes an element
    /// </summary>
    /// <param name="id">Id of the element to be deleted</param>
    /// <returns>Ok if Element has been deleted</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(IDT id)
    {
        await _service.Delete(id);
        return Ok();
    }
}