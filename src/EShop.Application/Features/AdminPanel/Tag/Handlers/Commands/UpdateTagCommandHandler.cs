using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Commands;

public class UpdateTagCommandHandler(ITagRepository tagRepository,IRabbitmqPublisherService rabbitmqPublisher)
    : IRequestHandler<UpdateTagCommandRequest, UpdateTagCommandResponse>
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<UpdateTagCommandResponse> Handle(UpdateTagCommandRequest request,
        CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.FindByIdAsync(request.Id) ??
                  throw new NotFoundException(NameToReplaceInException.Tag);

        var isNewTitleExists =
            await _tagRepository.IsExistsByAsync(nameof(Domain.Entities.Tag.Title), request.Title, request.Id);
        
        if (isNewTitleExists)
            throw new DuplicateException(NameToReplaceInException.Tag);

        tag.Title = request.Title;
        await _tagRepository.SaveChangesAsync();
        await _rabbitmqPublisher.PublishMessageAsync<Domain.Entities.Tag>(
            new(ActionTypes.Update, tag),
            RabbitmqConstants.QueueNames.Tag,
            RabbitmqConstants.RoutingKeys.Tag);
        return new();
    }
}