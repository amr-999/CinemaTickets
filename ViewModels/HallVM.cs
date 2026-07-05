using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaTickets.ViewModels;

public class HallVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hall name is required")]
    [StringLength(100, MinimumLength = 1)]
    [Display(Name = "Hall Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Total seats is required")]
    [Range(1, 10000)]
    [Display(Name = "Total Seats")]
    public int TotalSeats { get; set; }

    [StringLength(200)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Cinema is required")]
    [Display(Name = "Cinema")]
    public int CinemaId { get; set; }

    public SelectList? CinemaList { get; set; }
}
