namespace WEB_253502_POBORTSEVA.UI.Services.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile formFile);
        Task DeleteFileAsync(string fileName);

    }
}
