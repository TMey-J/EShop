namespace EShop.Application.Model;

public record Pagination
{
    [DisplayName("شماره صفحه")] 
    public int CurrentPage { get; set; } = 1;

    [DisplayName("تعداد رکوردهای بازگشتی")]
    public int TakeRecord { get; set; } = 10;
    
}