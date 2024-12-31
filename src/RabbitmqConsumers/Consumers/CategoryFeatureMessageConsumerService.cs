using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class CategoryFeatureMessageConsumerService : BaseRabbitmqConsumer<CategoryFeatureMessageConsumerService>
    {
        private readonly IMongoCategoryFeatureRepository _categoryFeatureRepository;

        public CategoryFeatureMessageConsumerService(
          IMongoCategoryFeatureRepository categoryFeatureRepository, ILogger<CategoryFeatureMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _categoryFeatureRepository = categoryFeatureRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.CategoryFeature;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoCategoryFeature>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _categoryFeatureRepository.CreateAsync(deserializeMessage.Data);
                    break;
                // case ActionTypes.Update:
                //     await _categoryFeatureRepository.Update(deserializeMessage.Data);
                //     break;
                case ActionTypes.Delete:
                    await _categoryFeatureRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    break;
            }


        }
    }
}
