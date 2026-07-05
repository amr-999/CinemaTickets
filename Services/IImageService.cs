namespace CinemaTickets.Services;

public interface IImageService
{
    Task<string?> SaveImageAsync(IFormFile? file, string folder);
    Task<List<string>> SaveImagesAsync(List<IFormFile> files, string folder);
    void DeleteImage(string? imagePath);
}
