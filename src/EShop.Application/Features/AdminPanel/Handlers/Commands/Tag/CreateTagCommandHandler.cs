using EShop.Application.Features.AdminPanel.Requests.Commands.Tag;

namespace EShop.Application.Features.AdminPanel.Handlers.Commands.Tag;

public class CreateTagCommandHandler(ITagRepository tagRepository):IRequestHandler<CreateTagCommandRequest,CreateTagCommandResponse>
{
    private readonly ITagRepository _tagRepository = tagRepository;
    
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
            Title = request.Title
        };
        await _tagRepository.CreateAsync(tag);
        await _tagRepository.SaveChangesAsync();
        return new();
    }
}