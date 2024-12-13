using System.Text;
using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Uri = System.Uri;

namespace EShop.Infrastructure.Services
{
    public class RabbitmqPublisherService(IOptionsSnapshot<SiteSettings> siteSettings) : IRabbitmqPublisherService
    {
        private readonly SiteSettings _siteSettings = siteSettings.Value;

        public async Task PublishMessageAsync<TEntity>(MessageModel<TEntity> message, string queueName, string routeKey)
        {
            ConnectionFactory factory = new()
            {
                Uri = new Uri(_siteSettings.Rabbitmq.Uri),
                ClientProvidedName = _siteSettings.Rabbitmq.ClientProvidedName
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var exchangeName = _siteSettings.Rabbitmq.ExchangeName;

            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
            await channel.QueueDeclareAsync(queueName, false, false, false, null);
            await channel.QueueBindAsync(queueName, exchangeName, routeKey, null);

            var serializeMessage = JsonConvert.SerializeObject(message);
            var massageBodyBytes = Encoding.UTF8.GetBytes(serializeMessage);

            await channel.BasicPublishAsync(exchangeName, routeKey,massageBodyBytes);
            await channel.CloseAsync();
            await connection.CloseAsync();
        }
    }
}
