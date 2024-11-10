namespace EShop.Application.Common.Helpers;

public static class FileHelpers
{
    public static void DeleteFileAsync(string fileName, string path)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
        if (!File.Exists(filePath))
            throw new CustomBadRequestException(["عکس مورد نظر یافت نشد"]);
        File.Delete(filePath);
    }

    public static async Task<string> ReUploadFileAsync(string? oldFileName, string base64, string path)
    {
        if (oldFileName is not null)
            DeleteFileAsync(oldFileName, path);

        string fileName = await UploadFileAsync(base64, path);
        return fileName;
    }

    public static async Task<string> UploadFileAsync(this string base64, string path)
    {
        base64 = base64.RemoveBase64Header();
        var fileExtension = base64.GetBase64Extesion();
        var fileName = StringHelpers.GenerateUniqueName();
        await SaveFileBase64Async(new(base64, fileName, fileExtension, path));
        return $"{fileName}.{fileExtension}";
    }
    private static string RemoveBase64Header(this string base64)
    {
        var splitedBase64 = base64.Split(',');
        return splitedBase64.Length > 1 ? splitedBase64[1] : base64;
    }
    private static string GetBase64Extesion(this string base64)
    {
        return base64[..5].ToUpper() switch
        {
            "IVBOR" => "png",
            "/9J/4" => "jpg",
            "AAAAF" => "mp4",
            "JVBER" => "pdf",
            "AAABA" => "ico",
            "UMFYI" => "rar",
            "E1XYD" => "rtf",
            "U1PKC" => "txt",
            "77U/M" => "srt",
            _ => string.Empty
        };
    }
    private static async Task SaveFileBase64Async(SaveFileBase64Model model)
    {
        string filePath = new(Path.Combine(Directory.GetCurrentDirectory(), model.path));
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        var fullFilePath = filePath + $"{model.fileName}.{model.extension}";
        try
        {
            var fileBytes = Convert.FromBase64String(model.fileBase64);
            await File.WriteAllBytesAsync(fullFilePath, fileBytes);

        }
        catch (FormatException)
        {
            throw new CustomInternalServerException(["file is not base64"]);
        }
    }

    private static async Task<string> ConvertImageToBase64(this string imgName, string path)
    {
        string imagePath = new(Path.Combine(Directory.GetCurrentDirectory(), path, imgName));
        if (!File.Exists(imagePath))
            throw new CustomInternalServerException(["عکس مورد نظر پیدا نمیشود"]);

        byte[] imageArray = await File.ReadAllBytesAsync(imagePath.ToString());
        var base64ImageRepresentation = Convert.ToBase64String(imageArray);
        return base64ImageRepresentation;
    }
}
