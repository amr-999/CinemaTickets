using CinemaTickets.Models;

namespace CinemaTickets.Repository;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Category>     Categories    { get; }
    IGenericRepository<Cinema>       Cinemas       { get; }
    IGenericRepository<Hall>         Halls         { get; }
    IGenericRepository<Actor>        Actors        { get; }
    IGenericRepository<Movie>        Movies        { get; }
    IGenericRepository<MovieActor>   MovieActors   { get; }
    IGenericRepository<MovieSubImage> MovieSubImages { get; }
    Task<int> SaveAsync();
}
