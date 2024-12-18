namespace EShop.Application.Common.Helpers;

public static class FileHelpers
{
    public static string RemoveBase64Header(this string base64)
    {
        var splitedBase64 = base64.Split(',');
        return splitedBase64.Length > 1 ? splitedBase64[1] : base64;
    }

    public static string GetBase64Extension(this string base64)
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
    public static async Task SaveFileBase64Async(SaveFileBase64Model model)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), model.path);
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        var fullFilePath = filePath + $"/{model.fileName}.{model.extension}";
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
    public static double GetFileSizeFromBase64String(this string base64String,
        bool applyPaddingsRules = false,
        UnitsOfMeasurement unitsOfMeasurement = UnitsOfMeasurement.MegaByte)
    {
        if (string.IsNullOrEmpty(base64String)) return 0;
            
        var base64Length =  base64String.AsSpan().Slice(base64String.IndexOf(',') + 1).Length;
            
        var fileSizeInByte = Math.Ceiling((double)base64Length / 4) * 3;
           
        if (applyPaddingsRules && base64Length >= 2)
        {
            var paddings = base64String.AsSpan()[^2..];
            fileSizeInByte = paddings.EndsWith("==") ? fileSizeInByte - 2 :
                paddings[1].Equals('=') ? fileSizeInByte - 1 : fileSizeInByte;
        }
        return fileSizeInByte > 0 ? fileSizeInByte / (int)unitsOfMeasurement : 0;
    }
    public enum UnitsOfMeasurement
    {
        /// <summary>
        /// B.
        /// </summary>
        Byte = 1,
        /// <summary>
        /// KB.
        /// </summary>
        KiloByte = 1_024,
        /// <summary>
        /// MB.
        /// </summary>
        MegaByte = 1_048_576
    }
}
