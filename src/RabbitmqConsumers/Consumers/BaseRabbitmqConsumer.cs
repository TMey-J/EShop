using System.Text;
using EShop.Application.Model;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitmqConsumers.Consumers
{
    public abstract class BaseRabbitmqConsumer<T> : IHostedService
    {
        private readonly ILogger<T> _logger;
        private readonly IConnection _connection;
        protected readonly IChannel Channel;

        protected BaseRabbitmqConsumer(ILogger<T> logger, IConfiguration configuration)
        {
            _logger = logger;
            ConnectionFactory factory = new()
            {
                    Uri = new Uri(configuration.GetConnectionString("RabbitmqConnection")!),
                ClientProvidedName = configuration.GetSection("RabbitmqSettings:ClientProvidedName").Value
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            Channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        protected abstract string QueueName { get; }
        protected abstract Task HandelMessageAsync(string message);
        private IAsyncBasicConsumer CreateConsumer(CancellationToken cancellationToken)
        {
            AsyncEventingBasicConsumer consumer = new(Channel);
            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(args.Body.ToArray());
                    await HandelMessageAsync(message);
                    await Channel.BasicAckAsync(args.DeliveryTag, false, cancellationToken);
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex.Message);
                    await Channel.BasicNackAsync(args.DeliveryTag, false, false, cancellationToken);
                }

            };
            return consumer;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Channel.BasicConsumeAsync(QueueName, false, CreateConsumer(cancellationToken), cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Channel.CloseAsync(cancellationToken: cancellationToken);
            await _connection.CloseAsync(cancellationToken: cancellationToken);

        }

    }
}
