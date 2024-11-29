namespace EShop.Application.Constants;

public static class RabbitmqConstants
{
    public static class QueueNames
    {
        public const string Tag = nameof(Tag);
        public const string Category = nameof(Category);
    }
    public static class RoutingKeys
    {
        public const string Tag = nameof(Tag);
        public const string Category = nameof(Category);
    }
}