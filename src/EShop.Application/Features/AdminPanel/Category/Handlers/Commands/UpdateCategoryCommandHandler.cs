using EShop.Application.Features.AdminPanel.Category.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Commands;

public class UpdateCategoryCommandHandler(
    ICategoryRepository category,
    IFileServices fileServices,
    IOptionsSnapshot<SiteSettings> siteSettings)
    : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    private readonly ICategoryRepository _category = category;
    private readonly IFileServices _fileServices = fileServices;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;

    public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request,
        CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdAsync(request.Id) ??
                       throw new NotFoundException(NameToReplaceInException.Category);
        category.Title = request.NewTitle;
        if (!string.IsNullOrWhiteSpace(request.NewPicture))
        {
            category.Picture =
                await _fileServices.UploadFileAsync(
                    request.NewPicture,
                    _filesPath.Category,
                    (int)FileHelpers.MaximumFilesSizeInMegaByte.CategoryPicture,
                    category.Picture);
        }

        if (request.NewParentId is not null)
        {
            var newParentCategory = await _category.FindByIdAsync(request.NewParentId ?? 0) ??
                                 throw new NotFoundException(NameToReplaceInException.ParentCategory);
            
            var categoryChildren = await _category.GetCategoryChildrenAsync(category);

            var lastChild = await _category.GetLastChildHierarchyIdAsync(newParentCategory);
            
            category.Parent = newParentCategory.Parent.GetDescendant(lastChild);
            
            lastChild = null;
            foreach (var categoryChild in categoryChildren)
            {
                categoryChild.Parent = category.Parent.GetDescendant(lastChild);
                lastChild = categoryChild.Parent;
            }
        }
        
        await _category.SaveChangesAsync();
        return new();
    }
}