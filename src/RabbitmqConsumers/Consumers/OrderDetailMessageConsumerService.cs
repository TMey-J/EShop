using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class OrderDetailMessageConsumerService : BaseRabbitmqConsumer<OrderDetailMessageConsumerService>
    {
        private readonly IMongoOrderDetailRepository _orderDetailRepository;
        
        public OrderDetailMessageConsumerService(
          IMongoOrderDetailRepository orderDetailRepository, ILogger<OrderDetailMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _orderDetailRepository = orderDetailRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.OrderDetail;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoOrderDetail>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _orderDetailRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _orderDetailRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _orderDetailRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    throw new Exception("Unknown action type");
            }


        }
    }
}
