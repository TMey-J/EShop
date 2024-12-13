using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.DTOs;
using EShop.Application.Model;
using EShop.Domain.Entities;
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
            var deserializeMessage = JsonConvert.DeserializeObject<MessageModel<ReadCategoryDto>>(message);
            if (deserializeMessage?.Data is null)
                throw new Exception("message is null");
            var category = new Category()
            {
                Id = deserializeMessage.Data.Id,
                Title = deserializeMessage.Data.Title,
                IsDelete = deserializeMessage.Data.IsDelete,
                Picture = deserializeMessage.Data.PictureName,
                ParentId = deserializeMessage.Data.ParentId,
            };
            switch (deserializeMessage.ActionTypes)
            {
                case ActionTypes.Create:
                    await _categoryRepository.CreateAsync(category);
                    break;
                case ActionTypes.Update:
                    await _categoryRepository.Update(category);
                    break;
                case ActionTypes.Delete:
                    await _categoryRepository.Delete(category);
                    break;
                default:
                    break;
            }


        }
    }
}
