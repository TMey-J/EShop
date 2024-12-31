using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Commands;

public class CreateCategoryCommandHandler(
    ICategoryRepository category,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IRabbitmqPublisherService rabbitmqPublisher)
    : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _category = category;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request,
        CancellationToken cancellationToken)
    {
        var category = await _category.FindByAsync(nameof(Domain.Entities.Category.Title), request.Title);
        if (category is not null)
        {
            throw new DuplicateException(NameToReplaceInException.Category);
        }

        category = new Domain.Entities.Category()
        {
            Title = request.Title,
        };
        var saveFile = string.IsNullOrWhiteSpace(request.PictureBase64)
            ? null
            : _fileRepository.ReadyToSaveFileAsync(request.PictureBase64,
                _filesPath.Category,
                MaximumFilesSizeInMegaByte.CategoryPicture);
        category.Picture = saveFile?.fileNameWithExtention;

        if (request.Parent is not null)
        {
            var parentCategory = await _category.FindByIdAsync(request.Parent ?? 0)
                                 ?? throw new NotFoundException(NameToReplaceInException.ParentCategory);

            category.ParentId = parentCategory.Id;
        }

        await using var transaction = await _category.BeginTransactionAsync();
        try
        {
            await _category.CreateAsync(category);
            await _category.SaveChangesAsync();
            if (saveFile is not null)
            {
                await _fileRepository.SaveFileAsync(saveFile);
            }

            var mongoCategory = new MongoCategory
            {
                Id = category.Id,
                Title = category.Title,
                Picture = category.Picture,
                ParentId = category.ParentId
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoCategory>(
                new(ActionTypes.Create, mongoCategory),
                RabbitmqConstants.QueueNames.Category,
                RabbitmqConstants.RoutingKeys.Category);
            await transaction.CommitAsync(cancellationToken);
            return new CreateCategoryCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            if (saveFile is not null)
            {
                _fileRepository.DeleteFile(saveFile.fileNameWithExtention, _filesPath.Category);
            }

            throw;
        }
    }
}