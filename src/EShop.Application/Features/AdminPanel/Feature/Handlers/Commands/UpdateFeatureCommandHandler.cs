using EShop.Application.Features.AdminPanel.Feature.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.AdminPanel.Feature.Handlers.Commands;

public class UpdateFeatureCommandHandler(IFeatureRepository featureRepository,IRabbitmqPublisherService publisher)
    :IRequestHandler<UpdateFeatureCommandRequest,UpdateFeatureCommandResponse>
{
    private readonly IFeatureRepository _featureRepository = featureRepository;
    private readonly IRabbitmqPublisherService _publisher = publisher;

    public async Task<UpdateFeatureCommandResponse> Handle(UpdateFeatureCommandRequest request, CancellationToken cancellationToken)
    {
        var feature = await _featureRepository.FindByIdAsync(request.Id) ?? throw new NotFoundException(NameToReplaceInException.Feature);
        
        feature.Name = request.Name;
        
        _featureRepository.Update(feature);
        await _featureRepository.SaveChangesAsync();
       await _publisher.PublishMessageAsync<MongoFeature>(new
               (ActionTypes.Update, new MongoFeature()
               {
                   Name = feature.Name,
                   Id = feature.Id
               }), RabbitmqConstants.QueueNames.Feature,
           RabbitmqConstants.RoutingKeys.Feature);
       return new UpdateFeatureCommandResponse();
    }
}