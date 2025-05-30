using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

/// <summary>
/// Base Provider Class for the Data Base Providers
/// </summary>
/// <typeparam name="T">Entity Type</typeparam>
/// <typeparam name="IDT">Primary Key Type</typeparam>
/// <param name="context">Database Context</param>
public abstract class BaseProvider<T, IDT>(InstnwndContext context)
    : IBaseProvider<T, IDT>
    where T : class
{
    protected readonly InstnwndContext _context = context;

    protected virtual IQueryable<T> Query() => _context.Set<T>();

    public virtual Task<int> GetTotalCount() => Query().CountAsync();

    public virtual Task<List<T>> GetListAsync() => Query().ToListAsync();

    public virtual Task<List<T>> GetListAsync(int skip, int take)
        => GetValues(skip, take).ToListAsync();

    public virtual Task<List<T>> GetListAsync(int skip, int take, string orderBy, bool isAscending)
        => GetValues(skip, take, orderBy, isAscending).ToListAsync();

    public virtual IQueryable<T> GetValues(int skip, int take, string orderBy = "", bool isAscending = true, Expression<Func<T, bool>>? filter = null)
    {
        var q = Query();

        if (filter != null)
        {
            q = q.Where(filter);
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var entityType = _context.Model.FindEntityType(typeof(T))!;
            var prop = entityType
              .GetProperties()
              .FirstOrDefault(p =>
                p.Name.Equals(orderBy, StringComparison.OrdinalIgnoreCase));
            var colType = prop?.GetColumnType()?.ToLowerInvariant();
            if (colType is not ("text" or "ntext" or "image"))
            {
                q = isAscending ? q.OrderBy(orderBy) : q.OrderByDescending(orderBy);
            }
            else
            {
                throw new ArgumentException($"{entityType.Name} can't be ordered by {orderBy} because column type is {colType} on database");
            }
        }

        return q.Skip(skip).Take(take);
    }

    public virtual Task<T?> GetByIdAsync(IDT id) => FindAsync(id);

    //TODO: Value Task Implementation
    protected virtual async Task<T?> FindAsync(IDT id)
    {
        var result = await _context.Set<T>().FindAsync(id);
        return result;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var res = await _context.Set<T>().AddAsync(entity);
        return res.Entity;
    }

    public virtual async Task<T> AddAndSaveAsync(T entity)
    {
        entity = await AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual T Update(T entity)
    {
        var res = _context.Set<T>().Update(entity);
        return res.Entity;
    }

    public virtual async Task<T> UpdateAndSaveAsync(T entity)
    {
        entity = Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task Delete(IDT id)
    {
        var entity = await FindAsync(id);
        if (entity is not null)
            _context.Remove(entity);
    }

    public virtual async Task DeleteAndSaveAsync(IDT id)
    {
        await Delete(id);
        await _context.SaveChangesAsync();
    }
}