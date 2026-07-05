using CinemaTickets.Models;
using CinemaTickets.Repository;

namespace CinemaTickets.Services;

public class CinemaService : ICinemaService
{
    private readonly IUnitOfWork _uow;
    public CinemaService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Cinema>> GetAllAsync() =>
        await _uow.Cinemas.GetAllAsync();

    public async Task<Cinema?> GetByIdAsync(int id) =>
        await _uow.Cinemas.GetAsync(c => c.Id == id);

    public async Task AddAsync(Cinema c)
        { await _uow.Cinemas.AddAsync(c); await _uow.SaveAsync(); }

    public async Task UpdateAsync(Cinema c)
        { _uow.Cinemas.Update(c); await _uow.SaveAsync(); }

    public async Task DeleteAsync(int id)
    {
        var e = await GetByIdAsync(id);
        if (e != null) { _uow.Cinemas.Remove(e); await _uow.SaveAsync(); }
    }
}
