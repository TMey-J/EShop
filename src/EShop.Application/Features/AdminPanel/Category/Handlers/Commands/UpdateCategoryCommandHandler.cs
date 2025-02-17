using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

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
        SaveFileBase64Model? saveFile = null;
        if (!string.IsNullOrWhiteSpace(request.NewPicture))
        {
            saveFile = _fileServices.ReadyToSaveFileAsync(
                request.NewPicture,
                _filesPath.Category,
                MaximumFilesSizeInMegaByte.CategoryPicture,
                category.Picture);
            category.Picture = saveFile.fileNameWithExtention;
        }

        if (request.ParentId != category.ParentId)
        {
            if (await _category.IsHasChild(category))
            {
                throw new CustomBadRequestException(["این دسته بندی دارای زیردسته است. ابتدا آنها را حذف کند"]);
            }

            var newParentCategory = await _category.FindByIdAsync(request.ParentId) ??
                                    throw new NotFoundException(NameToReplaceInException.ParentCategory);
            category.ParentId = newParentCategory.Id;
        }

        await using var transaction = await _category.BeginTransactionAsync();

        try
        {
            await _category.SaveChangesAsync();
            if (saveFile is not null)
            {
                await _fileServices.SaveFileAsync(saveFile);
            }

            var mongoCategory = new MongoCategory
            {
                Id = category.Id,
                Title = category.Title,
                Picture = category.Picture,
                ParentId = category.ParentId
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoCategory>(
                new(ActionTypes.Update, mongoCategory),
                RabbitmqConstants.QueueNames.Category,
                RabbitmqConstants.RoutingKeys.Category);
            
            await transaction.CommitAsync(cancellationToken);
            return new UpdateCategoryCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}