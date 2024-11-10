namespace EShop.Application.Common;

public static class GenericPagitionExtention
{
    public static (IQueryable<T>,int) Page<T>(this IQueryable<T> query, int currentPage, int takeRecord)
    {
        var pageCount = (int)Math.Ceiling((double)query.Count() / takeRecord);
        if (currentPage>pageCount)
            currentPage = pageCount;
        return (query.Skip((currentPage - 1) * takeRecord).Take(takeRecord),pageCount);
    }
}