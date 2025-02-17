using EShop.Application.Contracts.MongoDb;
using EShop.Infrastructure.Databases;
using EShop.Infrastructure.Repositories.MongoDb;
using RabbitmqConsumers.Consumers;

namespace RabbitmqConsumers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbContext>();
            services.ConfigureMongoRepositories();
            services.AddHostedService<TagMessageConsumerService>();
            services.AddHostedService<UserMessageConsumerService>();
            services.AddHostedService<FeatureMessageConsumerService>();
            services.AddHostedService<CategoryFeatureMessageConsumerService>();
            services.AddHostedService<CategoryMessageConsumerService>();
            services.AddHostedService<SellerMessageConsumerService>();
            services.AddHostedService<ProductMessageConsumerService>();
            services.AddHostedService<SellerProductMessageConsumerService>();
            services.AddHostedService<CommentMessageConsumerService>();
            services.AddHostedService<OrderMessageConsumerService>();
            services.AddHostedService<OrderDetailMessageConsumerService>();
            return services;
        }
        
        private static void ConfigureMongoRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IMongoTagRepository, MongoTagRepository>();
            services.AddSingleton<IMongoUserRepository, MongoUserRepository>();
            services.AddSingleton<IMongoFeatureRepository, MongoFeatureRepository>();
            services.AddSingleton<IMongoCategoryFeatureRepository, MongoCategoryFeatureRepository>();
            services.AddSingleton<IMongoCategoryRepository, MongoCategoryRepository>();
            services.AddSingleton<IMongoSellerRepository, MongoSellerRepository>();
            services.AddSingleton<IMongoProductRepository, MongoProductRepository>();
            services.AddSingleton<IMongoSellerProductRepository, MongoSellerProductRepository>();
            services.AddSingleton<IMongoCommentRepository, MongoCommentRepository>();
            services.AddSingleton<IMongoOrderRepository, MongoOrderRepository>();
            services.AddSingleton<IMongoOrderDetailRepository, MongoOrderDetailRepository>();
        }
    }
}