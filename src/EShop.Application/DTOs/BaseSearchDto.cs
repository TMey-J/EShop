namespace EShop.Application.DTOs;

public record BaseSearchDto
{
    /// <summary>
    /// 0=> don't show deleted item
    /// 1=> show also deleted item
    /// 2=> show only deleted item
    /// </summary>
    [DisplayName("وضعیت حذف شده")]
    public DeleteStatus DeleteStatus { get; set; }
    /// <summary>
    /// 0=> Ascending
    /// 1=> Descending
    /// </summary>
    [DisplayName("مرتب سازی به صورت...")]
    public SortingAs SortingAs { get; set; }

    public Pagination Pagination { get; set; } = new();
}
public enum DeleteStatus
{
    False,
    True,
    OnlyDeleted
}

public enum SortingAs
{
    Ascending,
    Descending
}


