namespace CinemaTickets.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _env;
    private static readonly string[] _allowed = [".jpg",".jpeg",".png",".gif",".webp"];

    public ImageService(IWebHostEnvironment env) => _env = env;

    public async Task<string?> SaveImageAsync(IFormFile? file, string folder)
    {
        if (file == null || file.Length == 0) return null;
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowed.Contains(ext)) return null;

        var dir = Path.Combine(_env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(dir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        await using var stream = new FileStream(Path.Combine(dir, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
        return $"/uploads/{folder}/{fileName}";
    }

    public async Task<List<string>> SaveImagesAsync(List<IFormFile> files, string folder)
    {
        var paths = new List<string>();
        foreach (var f in files)
        {
            var p = await SaveImageAsync(f, folder);
            if (p != null) paths.Add(p);
        }
        return paths;
    }

    public void DeleteImage(string? path)
    {
        if (string.IsNullOrEmpty(path)) return;
        var full = Path.Combine(_env.WebRootPath,
            path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(full)) File.Delete(full);
    }
}
