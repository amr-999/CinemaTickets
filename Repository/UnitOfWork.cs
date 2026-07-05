using CinemaTickets.Data;
using CinemaTickets.Models;

namespace CinemaTickets.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public IGenericRepository<Category>      Categories    { get; }
    public IGenericRepository<Cinema>        Cinemas       { get; }
    public IGenericRepository<Hall>          Halls         { get; }
    public IGenericRepository<Actor>         Actors        { get; }
    public IGenericRepository<Movie>         Movies        { get; }
    public IGenericRepository<MovieActor>    MovieActors   { get; }
    public IGenericRepository<MovieSubImage> MovieSubImages { get; }

    public UnitOfWork(AppDbContext db)
    {
        _db            = db;
        Categories     = new GenericRepository<Category>(db);
        Cinemas        = new GenericRepository<Cinema>(db);
        Halls          = new GenericRepository<Hall>(db);
        Actors         = new GenericRepository<Actor>(db);
        Movies         = new GenericRepository<Movie>(db);
        MovieActors    = new GenericRepository<MovieActor>(db);
        MovieSubImages = new GenericRepository<MovieSubImage>(db);
    }

    public async Task<int> SaveAsync() => await _db.SaveChangesAsync();
    public void Dispose() => _db.Dispose();
}
