using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using Newtonsoft.Json;

namespace RabbitmqConsumers.Consumers
{
    public class CommentMessageConsumerService : BaseRabbitmqConsumer<CommentMessageConsumerService>
    {
        private readonly IMongoCommentRepository _commentRepository;

        public CommentMessageConsumerService(
          IMongoCommentRepository commentRepository, ILogger<CommentMessageConsumerService> logger,
          IConfiguration configuration) : base(logger, configuration)
        {
            _commentRepository = commentRepository;
            Channel.QueueDeclareAsync(QueueName,false,false,false,null);
        }

        protected sealed override string QueueName => RabbitmqConstants.QueueNames.Comment;
        
        protected override async Task HandelMessageAsync(string message)
        {
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<MongoComment>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");

            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _commentRepository.CreateAsync(deserializeMessage.Data);
                    break;
                case ActionTypes.Update:
                    await _commentRepository.Update(deserializeMessage.Data);
                    break;
                case ActionTypes.Delete:
                    await _commentRepository.Delete(deserializeMessage.Data);
                    break;
                default:
                    throw new Exception("Unknown action type");
            }


        }
    }
}
