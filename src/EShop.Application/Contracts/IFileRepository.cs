namespace EShop.Application.Contracts
{
    public interface IFileRepository
    {
        SaveFileBase64Model ReadyToSaveFileAsync(string base64, string path,int maximumFileSizeInMegaByte, string? oldFileName = null);
        
        Task SaveFileAsync(SaveFileBase64Model saveFile);
        void DeleteFile(string fileName, string path);
    }
}
