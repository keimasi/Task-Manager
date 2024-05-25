namespace TaskManager.Services;

public class FileUpload
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUpload(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> Upload(IFormFile file)
    {
        if (file == null) return "";

        var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploaded", "images");
        
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(directoryPath, fileName);
        await using var output = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(output);
        return fileName;
    }

    public void Delete(string filePath)
    {
        var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploaded", "images", filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
