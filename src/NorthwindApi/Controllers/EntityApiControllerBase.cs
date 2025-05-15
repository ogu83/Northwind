using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public abstract class EntityApiControllerBase<T, S, IDT>(S service, ILoggerFactory loggerFactory)
    : ApiControllerBase(loggerFactory)
    where T : BaseModel
    where S : IBaseService<T, IDT>
{
    protected readonly IBaseService<T, IDT> _service = service ?? throw new ArgumentNullException(nameof(service));

    protected string _className => GetType().Name;

    /// <summary>
    /// Get List
    /// </summary>
    /// <returns>All Elements</returns>
    /// <response code="200">Returns all elements</response>
    /// <response code="404">If there is no elements</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 10)]
    public async Task<ActionResult<List<T>>> Get()
    {
        _logger.LogInformation("{0} | {1} Get Called",
            DateTime.UtcNow.ToLongTimeString(),
            _className);

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
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public async Task<ActionResult<PagedList<T>>> Get(int skip, int take)
    {
        _logger.LogInformation("{0} | {1} Get called with skip:{2},take:{3}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            skip, take);

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
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 10)]
    public virtual async Task<ActionResult<T>> Get(IDT id)
    {
        _logger.LogInformation("{0} | {1} Get called with id:{2}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            id);
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
        _logger.LogInformation("{0} | {1} Put called with obj:{2}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            JsonSerializer.Serialize(obj));
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
        _logger.LogInformation("{0} | {1} Post called with obj:{2}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            JsonSerializer.Serialize(obj));
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
        _logger.LogInformation("{0} | {1} Delete called with id:{2}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            id);
        await _service.Delete(id);
        return Ok();
    }
}