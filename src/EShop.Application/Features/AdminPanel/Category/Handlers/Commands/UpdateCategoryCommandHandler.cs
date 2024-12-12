using EShop.Application.Features.AdminPanel.Category.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Commands;

public class UpdateCategoryCommandHandler(
    ICategoryRepository category,
    IFileRepository fileServices,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IRabbitmqPublisherService rabbitmqPublisher)
    : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    private readonly ICategoryRepository _category = category;
    private readonly IFileRepository _fileServices = fileServices;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;

    public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request,
        CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdAsync(request.Id) ??
                       throw new NotFoundException(NameToReplaceInException.Category);
        category.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.NewPicture))
        {
            category.Picture =
                await _fileServices.UploadFileAsync(
                    request.NewPicture,
                    _filesPath.Category,
                    (int)FileHelpers.MaximumFilesSizeInMegaByte.CategoryPicture,
                    category.Picture);
        }

        if (request.ParentId!=category.ParentId)
        {
            if (await _category.IsHasChild(category))
            {
                throw new CustomBadRequestException(["این دسته بندی دارای زیردسته است. ابتدا آنها را حذف کند"]);
            }
            var newParentCategory = await _category.FindByIdAsync(request.ParentId) ??
                                    throw new NotFoundException(NameToReplaceInException.ParentCategory);
            category.ParentId = newParentCategory.Id;
        }
        
        await _category.SaveChangesAsync();
        var readCategoryDto=new ReadCategoryDto(category.Id, category.Title,category.ParentId, category.Picture, category.IsDelete);
        await _rabbitmqPublisher.PublishMessageAsync<ReadCategoryDto>(
            new(ActionTypes.Update, readCategoryDto),
            RabbitmqConstants.QueueNames.Category,
            RabbitmqConstants.RoutingKeys.Category);
        return new();
    }
}