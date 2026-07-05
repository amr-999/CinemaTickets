using CinemaTickets.Models;
using CinemaTickets.Models.Enums;
using CinemaTickets.Repository;

namespace CinemaTickets.Services;

public class MovieService : IMovieService
{
    private readonly IUnitOfWork _uow;
    public MovieService(IUnitOfWork uow) => _uow = uow;

    private const string FullIncludes = "Category,Cinema,Hall,MovieActors.Actor,SubImages";
    private const string ListIncludes = "Category,Cinema,Hall,MovieActors.Actor";

    public async Task<IEnumerable<Movie>> GetAllAsync() =>
        await _uow.Movies.GetAllAsync(includeProperties: ListIncludes);

    public async Task<Movie?> GetByIdAsync(int id) =>
        await _uow.Movies.GetAsync(m => m.Id == id);

    public async Task<Movie?> GetWithDetailsAsync(int id) =>
        await _uow.Movies.GetAsync(m => m.Id == id, includeProperties: FullIncludes);

    public async Task<(List<Movie> Items, int Total)> GetFilteredAsync(
        string? search, int? categoryId, int? cinemaId,
        string? status, int page, int pageSize, bool availableOnly = false)
    {
        var all = (await _uow.Movies.GetAllAsync(includeProperties: ListIncludes)).AsEnumerable();

        if (availableOnly)
            all = all.Where(m => m.Status != MovieStatus.Ended);

        if (!string.IsNullOrWhiteSpace(search))
            all = all.Where(m =>
                m.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                m.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (categoryId > 0)  all = all.Where(m => m.CategoryId == categoryId);
        if (cinemaId   > 0)  all = all.Where(m => m.CinemaId   == cinemaId);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<MovieStatus>(status, out var s))
            all = all.Where(m => m.Status == s);

        var list  = all.OrderByDescending(m => m.DateTime).ToList();
        var total = list.Count;
        var paged = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return (paged, total);
    }

    public async Task AddAsync(Movie movie, List<int> actorIds)
    {
        foreach (var aid in actorIds.Distinct())
            movie.MovieActors.Add(new MovieActor { ActorId = aid });
        await _uow.Movies.AddAsync(movie);
        await _uow.SaveAsync();
    }

    public async Task UpdateAsync(Movie movie, List<int> actorIds, List<int> removeSubImageIds)
    {
        if (removeSubImageIds.Count > 0)
        {
            var toRemove = await _uow.MovieSubImages
                .GetAllAsync(s => removeSubImageIds.Contains(s.Id) && s.MovieId == movie.Id);
            foreach (var s in toRemove) _uow.MovieSubImages.Remove(s);
        }
        var existing = await _uow.MovieActors.GetAllAsync(ma => ma.MovieId == movie.Id);
        foreach (var ma in existing) _uow.MovieActors.Remove(ma);
        foreach (var aid in actorIds.Distinct())
            await _uow.MovieActors.AddAsync(new MovieActor { MovieId = movie.Id, ActorId = aid });
        _uow.Movies.Update(movie);
        await _uow.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var m = await GetWithDetailsAsync(id);
        if (m is null) return;
        foreach (var ma in m.MovieActors.ToList()) _uow.MovieActors.Remove(ma);
        foreach (var si in m.SubImages.ToList())   _uow.MovieSubImages.Remove(si);
        _uow.Movies.Remove(m);
        await _uow.SaveAsync();
    }

    public async Task<(int total, int nowShowing, int comingSoon, int ended)> GetStatsAsync()
    {
        var list = (await _uow.Movies.GetAllAsync()).ToList();
        return (list.Count,
                list.Count(m => m.Status == MovieStatus.NowShowing),
                list.Count(m => m.Status == MovieStatus.ComingSoon),
                list.Count(m => m.Status == MovieStatus.Ended));
    }
}
