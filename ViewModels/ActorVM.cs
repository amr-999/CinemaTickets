using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.ViewModels;

public class ActorVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Actor name is required")]
    [StringLength(150, MinimumLength = 2)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Biography")]
    public string? Bio { get; set; }

    public string?    Img     { get; set; }
    [Display(Name = "Photo")]
    public IFormFile? ImgFile { get; set; }
}
