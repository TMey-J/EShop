namespace EShop.Application.Contracts.Services
{
    public interface IFileServices
    {
        Task<string> UploadFileAsync(string base64, string path,int maximumFileSizeInMegaByte, string? oldFileName = null);
        void DeleteFile(string fileName, string path);
    }
}
