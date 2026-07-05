using System.ComponentModel.DataAnnotations;
using CinemaTickets.Models;
using CinemaTickets.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaTickets.ViewModels;

public class MovieVM
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
    [Display(Name = "Ticket Price ($)")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Status")]
    public MovieStatus Status { get; set; }

    [Required(ErrorMessage = "Screening date is required")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Screening Date & Time")]
    public DateTime DateTime { get; set; } = System.DateTime.Now.AddDays(7);

    [Range(0, 10000)]
    [Display(Name = "Available Seats")]
    public int AvailableSeats { get; set; }

    public string?    MainImg     { get; set; }
    [Display(Name = "Main Poster")]
    public IFormFile? MainImgFile { get; set; }

    [Display(Name = "Sub Images")]
    public List<IFormFile>? SubImgFiles { get; set; }
    public List<MovieSubImage> ExistingSubImages   { get; set; } = [];
    public List<int>           SubImageIdsToRemove { get; set; } = [];

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Cinema is required")]
    public int CinemaId { get; set; }

    public int? HallId { get; set; }

    public List<int>             SelectedActorIds { get; set; } = [];
    public List<ActorCheckboxVM> ActorList        { get; set; } = [];
    public SelectList?           CategoryList     { get; set; }
    public SelectList?           CinemaList       { get; set; }
    public SelectList?           HallList         { get; set; }
}
