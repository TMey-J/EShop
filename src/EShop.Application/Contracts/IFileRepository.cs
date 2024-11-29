namespace EShop.Application.Contracts
{
    public interface IFileRepository
    {
        Task<string> UploadFileAsync(string base64, string path,int maximumFileSizeInMegaByte, string? oldFileName = null);
        void DeleteFile(string fileName, string path);
    }
}
