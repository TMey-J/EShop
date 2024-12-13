using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class TagMessageConsumerService : BaseRabbitmqConsumer<TagMessageConsumerService>
    {
        private readonly IMongoTagRepository _tagRepository;

        public TagMessageConsumerService(
          IMongoTagRepository tagRepository, ILogger<TagMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _tagRepository = tagRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Tag;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<Tag>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _tagRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _tagRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _tagRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    break;
            }


        }
    }
}
