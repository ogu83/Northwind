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

    public virtual Task<int> GetTotalCount(string filter = "")
    {
        var q = Query();
        var entityType = _context.Model.FindEntityType(typeof(T))!;
        var properties = entityType.GetProperties();
        q = FilterQuery(filter, q, properties);
        return q.CountAsync();
    }

    public virtual Task<List<T>> GetListAsync() => Query().ToListAsync();

    public virtual Task<List<T>> GetListAsync(int skip, int take)
        => GetValues(skip, take).ToListAsync();

    public virtual Task<List<T>> GetListAsync(int skip, int take, string orderBy, bool isAscending)
        => GetValues(skip, take, orderBy, isAscending).ToListAsync();

    public virtual Task<List<T>> GetListAsync(int skip, int take, string orderBy, bool isAscending, string filter)
        => GetValues(skip, take, orderBy, isAscending, filter).ToListAsync();

    public virtual IQueryable<T> GetValues(int skip, int take, string orderBy = "", bool isAscending = true, string filter = "")
    {
        var q = Query();
        var entityType = _context.Model.FindEntityType(typeof(T))!;
        var properties = entityType.GetProperties();
        q = FilterQuery(filter, q, properties);

        if (!string.IsNullOrEmpty(orderBy))
        {
            var prop = properties.FirstOrDefault(p => p.Name.Equals(orderBy, StringComparison.OrdinalIgnoreCase));
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

    private static IQueryable<T> FilterQuery(string filter, IQueryable<T> q, IEnumerable<Microsoft.EntityFrameworkCore.Metadata.IProperty> properties)
    {
        filter = filter.Trim();
        if (filter != "")
        {
            IQueryable<T>? qc = null;
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (property.PropertyInfo is null) continue;

                var filterableAttribute = property.PropertyInfo.GetCustomAttributes(typeof(FilterableAttribute), false).FirstOrDefault();
                if (filterableAttribute is not null)
                {
                    if (qc is null)
                        qc = q.Contains(propertyName, filter);
                    else
                        qc = qc.Union(q.Contains(propertyName, filter));
                }
            }

            q = qc ?? q;
        }

        return q;
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