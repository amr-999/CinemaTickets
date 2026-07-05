using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.ViewModels;

public class CinemaVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Cinema name is required")]
    [StringLength(150, MinimumLength = 2)]
    [Display(Name = "Cinema Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Location")]
    public string? Location { get; set; }

    public string?    Img     { get; set; }
    [Display(Name = "Cinema Image")]
    public IFormFile? ImgFile { get; set; }
}
