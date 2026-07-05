using CinemaTickets.Models;

namespace CinemaTickets.Services;

public interface IActorService
{
    Task<IEnumerable<Actor>> GetAllAsync();
    Task<Actor?> GetByIdAsync(int id);
    Task AddAsync(Actor a);
    Task UpdateAsync(Actor a);
    Task DeleteAsync(int id);
}
