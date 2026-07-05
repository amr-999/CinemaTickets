using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Models;

public class Actor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Actor name is required")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be 2–150 characters")]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Photo")]
    public string? Img { get; set; }

    [StringLength(1000)]
    [Display(Name = "Biography")]
    public string? Bio { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; } = [];
}
