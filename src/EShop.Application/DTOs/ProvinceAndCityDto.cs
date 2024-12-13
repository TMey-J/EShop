namespace EShop.Application.DTOs;

public class ProvinceDto
{
    public long Id { get; set; }
    public string Name { get; set; }=string.Empty;
}
public class CityDto
{
    public long Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public long Province_Id { get; set; }
}