using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class OrderMessageConsumerService : BaseRabbitmqConsumer<OrderMessageConsumerService>
    {
        private readonly IMongoOrderRepository _orderRepository;
        
        public OrderMessageConsumerService(
          IMongoOrderRepository orderRepository, ILogger<OrderMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _orderRepository = orderRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Order;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoOrder>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _orderRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _orderRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _orderRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    throw new Exception("Unknown action type");
            }


        }
    }
}
