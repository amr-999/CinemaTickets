using CinemaTickets.Models;
using CinemaTickets.Repository;

namespace CinemaTickets.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _uow;
    public CategoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await _uow.Categories.GetAllAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await _uow.Categories.GetAsync(c => c.Id == id);

    public async Task AddAsync(Category c)
        { await _uow.Categories.AddAsync(c); await _uow.SaveAsync(); }

    public async Task UpdateAsync(Category c)
        { _uow.Categories.Update(c); await _uow.SaveAsync(); }

    public async Task DeleteAsync(int id)
    {
        var e = await GetByIdAsync(id);
        if (e != null) { _uow.Categories.Remove(e); await _uow.SaveAsync(); }
    }
}
