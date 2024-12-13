using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Identity;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class UserMessageConsumerService : BaseRabbitmqConsumer<TagMessageConsumerService>
    {
        private readonly IMongoUserRepository _userRepository;

        public UserMessageConsumerService(
          IMongoUserRepository userRepository, ILogger<TagMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _userRepository = userRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.User;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<User>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _userRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _userRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _userRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    break;
            }


        }
    }
}
