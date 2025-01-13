using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class SellerMessageConsumerService : BaseRabbitmqConsumer<SellerMessageConsumerService>
    {
        private readonly IMongoSellerRepository _sellerRepository;

        public SellerMessageConsumerService(
          IMongoSellerRepository sellerRepository, ILogger<SellerMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _sellerRepository = sellerRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Seller;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoSeller>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _sellerRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _sellerRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _sellerRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    throw new Exception("Unknown action type");
            }


        }
    }
}
