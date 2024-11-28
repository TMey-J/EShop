using EShop.Application.Features.AdminPanel.Category.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Commands;

public class CreateCategoryCommandHandler(
    ICategoryRepository category,
    IFileServices fileServices,
    IOptionsSnapshot<SiteSettings> siteSettings)
    : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _category = category;
    private readonly IFileServices _fileServices = fileServices;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByAsync(nameof(Domain.Entities.Category.Title), request.Title);
        if (category is not null)
        {
            throw new DuplicateException(NameToReplaceInException.Category);
        }
        category = new Domain.Entities.Category()
        {
            Title = request.Title,
            Picture = string.IsNullOrWhiteSpace(request.PictureBase64) ? null :
            await _fileServices.UploadFileAsync(request.PictureBase64,
                _filesPath.Category,
                (int)FileHelpers.MaximumFilesSizeInMegaByte.CategoryPicture)
        };

        if (request.Parent is not null)
        {
            var parentCategory = await _category.FindByIdAsync(request.Parent ?? 0)
                ?? throw new NotFoundException(NameToReplaceInException.ParentCategory);

            var lastChild = await _category.GetLastChildHierarchyIdAsync(parentCategory!);

            category.Parent = parentCategory.Parent.GetDescendant(lastChild, null);
        }
        await _category.CreateAsync(category);
        await _category.SaveChangesAsync();

        return new CreateCategoryCommandResponse();
    }
}
