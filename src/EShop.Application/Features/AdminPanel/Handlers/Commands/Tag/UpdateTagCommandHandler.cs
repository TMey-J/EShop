using EShop.Application.Features.AdminPanel.Requests.Commands.Tag;

namespace EShop.Application.Features.AdminPanel.Handlers.Commands.Tag;

public class UpdateTagCommandHandler(ITagRepository tagRepository)
    : IRequestHandler<UpdateTagCommandRequest, UpdateTagCommandResponse>
{
    private readonly ITagRepository _tagRepository = tagRepository;

    public async Task<UpdateTagCommandResponse> Handle(UpdateTagCommandRequest request,
        CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.FindByIdAsync(request.Id) ??
                  throw new NotFoundException(NameToReplaceInException.Tag);
        
        var isNewTitleExists =
            await _tagRepository.IsExistsByAsync(nameof(Domain.Entities.Tag.Title),
            request.Title,
            request.Id);
        
        if (isNewTitleExists)
            throw new DuplicateException(NameToReplaceInException.Tag);

        tag.Title = request.Title;
        await _tagRepository.SaveChangesAsync();
        return new();
    }
}