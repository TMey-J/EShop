using EShop.Application.Common.Exceptions;
using EShop.Application.Model;
using Microsoft.Extensions.Logging;

namespace EShop.Infrastructure.Repositories;

public class FileRepository(ILogger<FileRepository> logger):IFileRepository
{
    private readonly ILogger<FileRepository> _logger = logger;

    public SaveFileBase64Model ReadyToSaveFileAsync(string base64, string path,int maximumFileSizeInMegaByte, string? oldFileName = null)
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
        var fileNameWithExtension =$"{fileName}.{fileExtension}";
        try
        {
            var fileBytes = Convert.FromBase64String(base64);
            return new SaveFileBase64Model(fileBytes, fileNameWithExtension, path);
        }
        catch (FormatException)
        {
            _logger.LogWarning("can not convert file to base64 string");
            throw new CustomBadRequestException(["فایل معتبر نیست"]);
        }
    }

    public async Task SaveFileAsync(SaveFileBase64Model saveFile)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), saveFile.path);
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        var fullFilePath = filePath + $"/{saveFile.fileNameWithExtention}";
            await File.WriteAllBytesAsync(fullFilePath, saveFile.fileBytes);
            
    }

    public void DeleteFile(string fileName, string path)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
        if (!File.Exists(filePath))
        {
            _logger.LogWarning($"this file path is not found: {filePath}");
        }
            
        File.Delete(filePath);

    }
    
}