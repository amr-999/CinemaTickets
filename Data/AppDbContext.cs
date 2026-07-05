using CinemaTickets.Models;
using CinemaTickets.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category>      Categories    => Set<Category>();
    public DbSet<Cinema>        Cinemas       => Set<Cinema>();
    public DbSet<Hall>          Halls         => Set<Hall>();
    public DbSet<Actor>         Actors        => Set<Actor>();
    public DbSet<Movie>         Movies        => Set<Movie>();
    public DbSet<MovieActor>    MovieActors   => Set<MovieActor>();
    public DbSet<MovieSubImage> MovieSubImages => Set<MovieSubImage>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<MovieActor>().HasKey(x => new { x.MovieId, x.ActorId });
        mb.Entity<MovieActor>()
            .HasOne(x => x.Movie).WithMany(m => m.MovieActors).HasForeignKey(x => x.MovieId);
        mb.Entity<MovieActor>()
            .HasOne(x => x.Actor).WithMany(a => a.MovieActors).HasForeignKey(x => x.ActorId);

        mb.Entity<MovieSubImage>()
            .HasOne(x => x.Movie).WithMany(m => m.SubImages)
            .HasForeignKey(x => x.MovieId).OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Hall>()
            .HasOne(h => h.Cinema).WithMany().HasForeignKey(h => h.CinemaId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Movie>()
            .HasOne(m => m.Hall).WithMany(h => h.Movies)
            .HasForeignKey(m => m.HallId).OnDelete(DeleteBehavior.SetNull).IsRequired(false);

        // ── Seed ──────────────────────────────────────────────
        mb.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Action"  },
            new Category { Id = 2, Name = "Drama"   },
            new Category { Id = 3, Name = "Comedy"  },
            new Category { Id = 4, Name = "Horror"  },
            new Category { Id = 5, Name = "Sci-Fi"  }
        );

        mb.Entity<Cinema>().HasData(
            new Cinema { Id = 1, Name = "Grand Cinema",  Location = "Downtown"   },
            new Cinema { Id = 2, Name = "Star Cinema",   Location = "Westside"   },
            new Cinema { Id = 3, Name = "Galaxy Cinema", Location = "North Mall" }
        );

        mb.Entity<Hall>().HasData(
            new Hall { Id = 1, Name = "Screen 1", TotalSeats = 200, CinemaId = 1, Description = "Main IMAX Screen" },
            new Hall { Id = 2, Name = "Screen 2", TotalSeats = 150, CinemaId = 1, Description = "Standard Hall"    },
            new Hall { Id = 3, Name = "Hall A",   TotalSeats = 180, CinemaId = 2, Description = "Dolby Atmos"      },
            new Hall { Id = 4, Name = "VIP Room", TotalSeats =  60, CinemaId = 2, Description = "Premium Seating"  },
            new Hall { Id = 5, Name = "Hall 1",   TotalSeats = 220, CinemaId = 3, Description = "4DX Experience"   }
        );

        mb.Entity<Actor>().HasData(
            new Actor { Id = 1, Name = "Tom Hanks",          Bio = "Academy Award-winning actor."  },
            new Actor { Id = 2, Name = "Scarlett Johansson",  Bio = "Hollywood A-list actress."    },
            new Actor { Id = 3, Name = "Leonardo DiCaprio",   Bio = "Oscar-winning actor."         },
            new Actor { Id = 4, Name = "Jennifer Lawrence",   Bio = "Hunger Games star."           },
            new Actor { Id = 5, Name = "Robert Downey Jr.",   Bio = "Iron Man himself."            }
        );

        var d = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        mb.Entity<Movie>().HasData(
            new Movie { Id = 1, Name = "Galactic Warriors",
                Description = "An epic sci-fi adventure across the cosmos where humanity battles an alien empire.",
                Price = 12.99m, Status = MovieStatus.NowShowing, AvailableSeats = 145,
                DateTime = d.AddDays(2), CategoryId = 5, CinemaId = 1, HallId = 1, CreatedAt = d },
            new Movie { Id = 2, Name = "Love in Paris",
                Description = "A heartwarming romance between two strangers in the City of Light.",
                Price = 9.99m, Status = MovieStatus.ComingSoon, AvailableSeats = 180,
                DateTime = d.AddDays(20), CategoryId = 2, CinemaId = 2, HallId = 3, CreatedAt = d },
            new Movie { Id = 3, Name = "The Dark Secret",
                Description = "A chilling horror mystery set in an isolated mansion.",
                Price = 11.50m, Status = MovieStatus.Ended, AvailableSeats = 0,
                DateTime = d.AddDays(-10), CategoryId = 4, CinemaId = 3, HallId = 5, CreatedAt = d },
            new Movie { Id = 4, Name = "Laughing Out Loud",
                Description = "A hilarious comedy following three mismatched roommates in New York City.",
                Price = 8.99m, Status = MovieStatus.NowShowing, AvailableSeats = 55,
                DateTime = d.AddDays(5), CategoryId = 3, CinemaId = 1, HallId = 2, CreatedAt = d }
        );

        mb.Entity<MovieActor>().HasData(
            new MovieActor { MovieId = 1, ActorId = 3 },
            new MovieActor { MovieId = 1, ActorId = 5 },
            new MovieActor { MovieId = 2, ActorId = 1 },
            new MovieActor { MovieId = 2, ActorId = 4 },
            new MovieActor { MovieId = 3, ActorId = 2 },
            new MovieActor { MovieId = 4, ActorId = 1 },
            new MovieActor { MovieId = 4, ActorId = 4 }
        );
    }
}
