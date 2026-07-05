using CinemaTickets.Models;

namespace CinemaTickets.Services;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task<Movie?> GetWithDetailsAsync(int id);
    Task AddAsync(Movie movie, List<int> actorIds);
    Task UpdateAsync(Movie movie, List<int> actorIds, List<int> removeSubImageIds);
    Task DeleteAsync(int id);
    Task<(List<Movie> Items, int Total)> GetFilteredAsync(
        string? search, int? categoryId, int? cinemaId,
        string? status, int page, int pageSize, bool availableOnly = false);
    Task<(int total, int nowShowing, int comingSoon, int ended)> GetStatsAsync();
}
