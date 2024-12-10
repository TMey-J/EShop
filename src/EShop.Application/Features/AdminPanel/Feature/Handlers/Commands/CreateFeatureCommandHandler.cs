using EShop.Application.Features.AdminPanel.Feature.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Feature.Handlers.Commands;

public class CreateFeatureCommandHandler(IFeatureRepository featureRepository,IRabbitmqPublisherService publisher)
    :IRequestHandler<CreateFeatureCommandRequest,CreateFeatureCommandResponse>
{
    private readonly IFeatureRepository _featureRepository = featureRepository;
    private readonly IRabbitmqPublisherService _publisher = publisher;

    public async Task<CreateFeatureCommandResponse> Handle(CreateFeatureCommandRequest request, CancellationToken cancellationToken)
    {
       var feature=await _featureRepository.FindByAsync(nameof(Domain.Entities.Feature.Name),
           request.Name);
       if (feature != null)
       {
           throw new DuplicateException(NameToReplaceInException.Feature);
       }

       feature = new()
       {
           Name = request.Name,
       };
       await _featureRepository.CreateAsync(feature);
       await _featureRepository.SaveChangesAsync();
       await _publisher.PublishMessageAsync<Domain.Entities.Feature>(new(ActionTypes.Create, feature), RabbitmqConstants.QueueNames.Feature,
           RabbitmqConstants.RoutingKeys.Feature);
       return new CreateFeatureCommandResponse();
    }
}