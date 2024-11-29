using EShop.Application.Common.Exceptions;
using EShop.Application.Contracts.Services;
using EShop.Application.Model;

namespace EShop.Infrastructure.Repositories;

public class FileRepository:IFileRepository
{
    public async Task<string> UploadFileAsync(string base64, string path,int maximumFileSizeInMegaByte, string? oldFileName = null)
    {
        var fileSize = base64.GetFileSizeFromBase64String(true);
        if (fileSize>maximumFileSizeInMegaByte)
        {
            throw new CustomBadRequestException([$"فایل نباید بیشتر از {maximumFileSizeInMegaByte} میگابایت باشد"]);
        }
        oldFileName = oldFileName?.Split('.')[0];
        base64 = base64.RemoveBase64Header();
        var fileExtension = base64.GetBase64Extension();
        var fileName = oldFileName ?? StringHelpers.GenerateUniqueName();
        await FileHelpers.SaveFileBase64Async(new SaveFileBase64Model(base64, fileName, fileExtension, path));
        return $"{fileName}.{fileExtension}";
    }

    public void DeleteFile(string fileName, string path)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
        if (!File.Exists(filePath))
            throw new CustomBadRequestException(["فایل مورد نظر یافت نشد"]);
        File.Delete(filePath);
    }
    
}