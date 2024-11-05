using EShop.Application.Contracts;
using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Features.AdminPanel.Handlers.Commands.Category;

public class CreateCategoryCommandHandler(ICategoryRepository category,
    IOptionsSnapshot<SiteSettings> siteSettings)
    : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _category = category;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByAsync(nameof(Domain.Entities.Category.Title), request.Title);
        if (category is not null)
        {
            throw new DuplicateException("دسته بندی");
        }
        category = new Domain.Entities.Category()
        {
            Title = request.Title,
            Picture = string.IsNullOrWhiteSpace(request.PictureBase64) ? null :
            await request.PictureBase64.UploadFileAsync(_filesPath.Category)
        };

        if (request.Parent is not null)
        {
            var parentCategory = await _category.FindByIdAsync(request.Parent ?? 0)
                ?? throw new CustomBadRequestException(["دسته بندی والد یافت نشد"]);

            var lastChild = await _category.GetLastChildHierarchyIdAsync(parentCategory!);

            category.Parent = parentCategory.Parent.GetDescendant(lastChild, null);
        }
        await _category.CreateAsync(category);
        await _category.SaveChangesAsync();

        return new();
    }
}
