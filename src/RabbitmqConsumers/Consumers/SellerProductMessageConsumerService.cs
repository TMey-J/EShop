using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class SellerProductMessageConsumerService : BaseRabbitmqConsumer<SellerProductMessageConsumerService>
    {
        private readonly IMongoSellerProductRepository _sellerProductRepository;


        public SellerProductMessageConsumerService(
            ILogger<SellerProductMessageConsumerService> logger,
            IConfiguration configuration, IMongoSellerProductRepository sellerProductRepository) : base(logger,
            configuration)
        {
            _sellerProductRepository = sellerProductRepository;
            Channel.QueueDeclareAsync(QueueName, false, false, false);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.SellerProduct;

        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoSellerProduct>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");

            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _sellerProductRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _sellerProductRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _sellerProductRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    throw new Exception("Unknown action type");
                    
            }
        }
    }
}