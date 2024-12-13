namespace EShop.Application.Contracts.Services;

public interface IRabbitmqPublisherService
{
    Task PublishMessageAsync<TEntity>(MessageModel<TEntity> message, string queueName,string routeKey);
}