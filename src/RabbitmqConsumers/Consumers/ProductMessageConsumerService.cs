using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class ProductMessageConsumerService : BaseRabbitmqConsumer<ProductMessageConsumerService>
    {
        private readonly IMongoProductRepository _productRepository;
        private readonly IMongoSellerProductRepository _sellerProductRepository;


        public ProductMessageConsumerService(
          IMongoProductRepository productRepository, ILogger<ProductMessageConsumerService> logger,
          IConfiguration configuration, IMongoSellerProductRepository sellerProductRepository) : base(logger, configuration)
        {
            _productRepository = productRepository;
            _sellerProductRepository = sellerProductRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Product;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoProduct>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _productRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _productRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _productRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    break;
            }


        }
    }
}
