using CinemaTickets.Models;

namespace CinemaTickets.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category c);
    Task UpdateAsync(Category c);
    Task DeleteAsync(int id);
}
