using CinemaTickets.Models;
using CinemaTickets.Repository;

namespace CinemaTickets.Services;

public class HallService : IHallService
{
    private readonly IUnitOfWork _uow;
    public HallService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Hall>> GetAllAsync() =>
        await _uow.Halls.GetAllAsync(includeProperties: "Cinema");

    public async Task<IEnumerable<Hall>> GetByCinemaAsync(int cinemaId) =>
        await _uow.Halls.GetAllAsync(h => h.CinemaId == cinemaId, includeProperties: "Cinema");

    public async Task<Hall?> GetByIdAsync(int id) =>
        await _uow.Halls.GetAsync(h => h.Id == id, includeProperties: "Cinema");

    public async Task AddAsync(Hall h)     { await _uow.Halls.AddAsync(h); await _uow.SaveAsync(); }
    public async Task UpdateAsync(Hall h)  { _uow.Halls.Update(h);          await _uow.SaveAsync(); }
    public async Task DeleteAsync(int id)
    {
        var e = await GetByIdAsync(id);
        if (e != null) { _uow.Halls.Remove(e); await _uow.SaveAsync(); }
    }
}
