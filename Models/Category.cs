using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2–100 characters")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Movie> Movies { get; set; } = [];
}
