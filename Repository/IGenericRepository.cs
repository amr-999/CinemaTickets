using System.Linq.Expressions;

namespace CinemaTickets.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
                                      string? includeProperties = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter,
                      string? includeProperties = null);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
}
