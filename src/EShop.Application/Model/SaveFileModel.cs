namespace EShop.Application.Model
{
    public record SaveFileBase64Model(byte[] fileBytes, string fileNameWithExtention, string path);
}
