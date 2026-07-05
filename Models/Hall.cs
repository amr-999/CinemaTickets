using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Models;

public class Hall
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hall name is required")]
    [StringLength(100, MinimumLength = 1)]
    [Display(Name = "Hall Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Total seats is required")]
    [Range(1, 10000, ErrorMessage = "Seats must be between 1 and 10000")]
    [Display(Name = "Total Seats")]
    public int TotalSeats { get; set; }

    [StringLength(200)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Cinema")]
    public int CinemaId { get; set; }
    public Cinema? Cinema { get; set; }

    public ICollection<Movie> Movies { get; set; } = [];
}
