using CinemaTickets.Models;

namespace CinemaTickets.Services;

public interface IHallService
{
    Task<IEnumerable<Hall>> GetAllAsync();
    Task<IEnumerable<Hall>> GetByCinemaAsync(int cinemaId);
    Task<Hall?> GetByIdAsync(int id);
    Task AddAsync(Hall h);
    Task UpdateAsync(Hall h);
    Task DeleteAsync(int id);
}
