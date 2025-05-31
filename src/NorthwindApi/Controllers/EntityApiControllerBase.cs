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
        var start = DateTime.UtcNow;
        _logger.LogDebug("{0} | {1} Get Called",
            start.ToLongTimeString(),
            _className);

        var retVal = await _service.GetListAsync();

        _logger.LogDebug("{0} | {1} Get Completed in {2} sec, Result: {3}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            (DateTime.UtcNow - start).TotalSeconds,
            JsonSerializer.Serialize(retVal));

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
    /// <param name="orderBy">Order By</param>
    /// <param name="isAscending">Order By Direction true for ascending, false for descending</param>
    /// <returns>All Elements</returns>
    /// <response code="200">Returns all elements</response>
    /// <response code="404">If there is no elements</response>
    [HttpGet("skip/{skip}/take/{take}/orderby/{orderBy}/asc/{isAscending}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public Task<ActionResult<PagedList<T>>> Get(int skip, int take, string orderBy, bool isAscending) => Get(skip, take, orderBy, isAscending, "");

    /// <summary>
    /// Get List
    /// </summary>
    /// <param name="skip">Skip element count for paging</param>
    /// <param name="take">Take element count for paging</param>
    /// <param name="orderBy">Order By</param>
    /// <param name="isAscending">Order By Direction true for ascending, false for descending</param>
    /// <param name="filter">Filter string</param>
    /// <returns>All Elements</returns>
    /// <response code="200">Returns all elements</response>
    /// <response code="404">If there is no elements</response>
    [HttpGet("skip/{skip}/take/{take}/orderby/{orderBy}/asc/{isAscending}/filter/{filter?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public async Task<ActionResult<PagedList<T>>> Get(int skip, int take, string orderBy, bool isAscending, string filter = "")
    {
        var start = DateTime.UtcNow;
        _logger.LogDebug("{0} | {1} Get called with skip:{2},take:{3}",
            start.ToLongTimeString(),
            _className,
            skip, take);

        PagedList<T>? retVal = null;
        try
        {
            retVal = await _service.GetPagedListAsync(skip, take, orderBy, isAscending, filter);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "ArgumentException on GetPagedListAsync");
            return NotFound();
        }
        catch (Exception)
        {
            throw;
        }

        _logger.LogDebug("{0} | {1} Get Completed in {2}, Result: {3}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            (DateTime.UtcNow - start).TotalSeconds,
            JsonSerializer.Serialize(retVal));

        // if (retVal.ItemCount > 0)
            return new ActionResult<PagedList<T>>(retVal);
        // else
        //     return new NotFoundResult();
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
        var start = DateTime.UtcNow;
        _logger.LogDebug("{0} | {1} Get called with id:{2}",
            start.ToLongTimeString(),
            _className,
            id);

        var retVal = await _service.GetById(id);

        _logger.LogDebug("{0} | {1} Get Completed in {2}, Result: {3}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            (DateTime.UtcNow - start).TotalSeconds,
            JsonSerializer.Serialize(retVal));

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
        var start = DateTime.UtcNow;
        _logger.LogDebug("{0} | {1} Put called with obj:{2}",
            start.ToLongTimeString(),
            _className,
            JsonSerializer.Serialize(obj));

        var retVal = await _service.Update(obj);

        _logger.LogDebug("{0} | {1} Put Completed in {2}, Result: {3}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            (DateTime.UtcNow - start).TotalSeconds,
            JsonSerializer.Serialize(retVal));

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
        var start = DateTime.UtcNow;
        _logger.LogDebug("{0} | {1} Post called with obj:{2}",
            start.ToLongTimeString(),
            _className,
            JsonSerializer.Serialize(obj));

        var retVal = await _service.Add(obj);

        _logger.LogDebug("{0} | {1} Post Completed in {2}, Result: {3}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            (DateTime.UtcNow - start).TotalSeconds,
            JsonSerializer.Serialize(retVal));

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
        var start = DateTime.UtcNow;
        _logger.LogDebug("{0} | {1} Delete called with id:{2}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            id);

        await _service.Delete(id);

        _logger.LogDebug("{0} | {1} Delete Completed in {2}",
            DateTime.UtcNow.ToLongTimeString(),
            _className,
            (DateTime.UtcNow - start).TotalSeconds);

        return Ok();
    }
}