using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaTickets.Models.Enums;

namespace CinemaTickets.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Movie name is required")]
    [StringLength(200, MinimumLength = 2)]
    [Display(Name = "Movie Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99")]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Ticket Price ($)")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Status")]
    public MovieStatus Status { get; set; }

    [Required(ErrorMessage = "Screening date is required")]
    [Display(Name = "Screening Date & Time")]
    public DateTime DateTime { get; set; }

    [Display(Name = "Main Image")]
    public string? MainImg { get; set; }

    [Range(0, 10000)]
    [Display(Name = "Available Seats")]
    public int AvailableSeats { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required]
    [Display(Name = "Cinema")]
    public int CinemaId { get; set; }
    public Cinema? Cinema { get; set; }

    [Display(Name = "Hall")]
    public int? HallId { get; set; }
    public Hall? Hall { get; set; }

    public ICollection<MovieSubImage> SubImages  { get; set; } = [];
    public ICollection<MovieActor>   MovieActors { get; set; } = [];
}
