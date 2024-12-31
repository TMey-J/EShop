using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class CategoryMessageConsumerService : BaseRabbitmqConsumer<CategoryMessageConsumerService>
    {
        private readonly IMongoCategoryRepository _categoryRepository;

        public CategoryMessageConsumerService(
          IMongoCategoryRepository categoryRepository, ILogger<CategoryMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _categoryRepository = categoryRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Category;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoCategory>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");

            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _categoryRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _categoryRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _categoryRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    break;
            }


        }
    }
}
