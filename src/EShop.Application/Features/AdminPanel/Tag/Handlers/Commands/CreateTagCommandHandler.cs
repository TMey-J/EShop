using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Commands;

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