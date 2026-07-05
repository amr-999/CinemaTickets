using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Models;

public class Cinema
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Cinema name is required")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be 2–150 characters")]
    [Display(Name = "Cinema Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Image")]
    public string? Img { get; set; }

    [StringLength(300)]
    [Display(Name = "Location")]
    public string? Location { get; set; }

    public ICollection<Movie> Movies { get; set; } = [];
}
