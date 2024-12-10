using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class FeatureMessageConsumerService : BaseRabbitmqConsumer<FeatureMessageConsumerService>
    {
        private readonly IMongoFeatureRepository _featureRepository;

        public FeatureMessageConsumerService(
          IMongoFeatureRepository featureRepository, ILogger<FeatureMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _featureRepository = featureRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Feature;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<Feature>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _featureRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _featureRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _featureRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    break;
            }


        }
    }
}
