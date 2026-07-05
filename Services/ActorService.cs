using CinemaTickets.Models;
using CinemaTickets.Repository;

namespace CinemaTickets.Services;

public class ActorService : IActorService
{
    private readonly IUnitOfWork _uow;
    public ActorService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Actor>> GetAllAsync() =>
        await _uow.Actors.GetAllAsync();

    public async Task<Actor?> GetByIdAsync(int id) =>
        await _uow.Actors.GetAsync(a => a.Id == id);

    public async Task AddAsync(Actor a)
        { await _uow.Actors.AddAsync(a); await _uow.SaveAsync(); }

    public async Task UpdateAsync(Actor a)
        { _uow.Actors.Update(a); await _uow.SaveAsync(); }

    public async Task DeleteAsync(int id)
    {
        var e = await GetByIdAsync(id);
        if (e != null) { _uow.Actors.Remove(e); await _uow.SaveAsync(); }
    }
}
