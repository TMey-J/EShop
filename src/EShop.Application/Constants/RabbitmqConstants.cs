namespace EShop.Application.Constants;

public static class RabbitmqConstants
{
    public static class QueueNames
    {
        public const string Tag = nameof(Tag);
        public const string Category = nameof(Category);
        public const string CategoryFeature = nameof(CategoryFeature);
        public const string User = nameof(User);
        public const string Seller = nameof(Seller);
        public const string Feature = nameof(Feature);
        public const string Product = nameof(Product);
    }
    public static class RoutingKeys
    {
        public const string Tag = nameof(Tag);
        public const string Category = nameof(Category);
        public const string CategoryFeature = nameof(CategoryFeature);
        public const string User = nameof(User);
        public const string Feature = nameof(Feature);
        public const string Seller = nameof(Seller);
        public const string Product = nameof(Product);
    }
}