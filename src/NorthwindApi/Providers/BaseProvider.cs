
using System.Linq.Expressions;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public abstract class BaseProvider<T>(InstnwndContext context) where T: class
{
    protected readonly InstnwndContext _context = context;
    protected virtual IQueryable<T> Query() => _context.Set<T>();
    public virtual IQueryable<T> GetValues(int skip, int take, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderBy = null)
    {
        var q = Query();
        if (filter != null)
        {
            q = q.Where(filter);
        }
        if (orderBy != null)
        {
            q = q.OrderBy(orderBy);
        }
        return q.Skip(skip).Take(take);
    }

    public virtual T? GetById(int id) => Query().FirstOrDefault();
    
    public virtual async Task<T?> FindAsync(int id)
    {
        var result = await _context.Set<T>().FindAsync(id);
        return result;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var res = await _context.Set<T>().AddAsync(entity);
        return res.Entity;
    }

    public virtual T Update(T entity)
    {
        var res = _context.Set<T>().Update(entity);
        return res.Entity;
    }
}