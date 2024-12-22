using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Commands;

public class CreateTagCommandHandler(ITagRepository tagRepository,
    IRabbitmqPublisherService rabbitmqPublisher):
    IRequestHandler<CreateTagCommandRequest,CreateTagCommandResponse>
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<CreateTagCommandResponse> Handle(CreateTagCommandRequest request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.FindByAsync(nameof(Domain.Entities.Tag.Title),
            request.Title);
        if (tag != null)
        {
            throw new DuplicateException("تگ");
        }
        
        tag = new Domain.Entities.Tag()
        {
            Title = request.Title,
            IsConfirmed = true
        };
        await _tagRepository.CreateAsync(tag);
        await _tagRepository.SaveChangesAsync();
        await _rabbitmqPublisher.PublishMessageAsync<Domain.Entities.Tag>(
            new(ActionTypes.Create, tag),
            RabbitmqConstants.QueueNames.Tag,
            RabbitmqConstants.RoutingKeys.Tag);
        return new();
    }
}