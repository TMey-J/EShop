namespace EShop.Application.Common.Helpers;

public static class MathHelper
{
    public static uint CalculatePriceWithDiscount(uint price, byte discount)
    {
        return discount > 0 ? price - (discount * price / 100) : price;
    }
    public static uint CalculateTotalSum(List<int> prices)
    {
        return (uint)prices.Sum();
    }
}