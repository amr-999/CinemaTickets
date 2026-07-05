using CinemaTickets.Models;

namespace CinemaTickets.Services;

public interface ICinemaService
{
    Task<IEnumerable<Cinema>> GetAllAsync();
    Task<Cinema?> GetByIdAsync(int id);
    Task AddAsync(Cinema c);
    Task UpdateAsync(Cinema c);
    Task DeleteAsync(int id);
}
