using System.Linq.Expressions;
using CinemaTickets.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _db;
    protected readonly DbSet<T>    _set;

    public GenericRepository(AppDbContext db) { _db = db; _set = db.Set<T>(); }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null)
    {
        IQueryable<T> q = _set;
        if (filter != null) q = q.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var p in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                q = q.Include(p.Trim());
        return await q.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter,
                                    string? includeProperties = null)
    {
        IQueryable<T> q = _set;
        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var p in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                q = q.Include(p.Trim());
        return await q.FirstOrDefaultAsync(filter);
    }

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> q = _set;
        if (filter != null) q = q.Where(filter);
        return await q.CountAsync();
    }
}
