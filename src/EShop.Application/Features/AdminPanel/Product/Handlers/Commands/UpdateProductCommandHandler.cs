using EShop.Application.Features.AdminPanel.Product.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.AdminPanel.Product.Handlers.Commands;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IColorRepository colorRepository,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IRabbitmqPublisherService rabbitmqPublisher) :
    IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IColorRepository _colorRepository = colorRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id) ??
                      throw new NotFoundException(NameToReplaceInException.Product);

        var category = await _categoryRepository.FindByIdAsync(request.CategoryId)
                       ?? throw new CustomBadRequestException(["دسته بندی یافت نشد"]);
        List<string> errors = [];

        var tags = await _productRepository.GetProductTagsAsync(product.Id);
        var isAllTagsMatch = request.Tags.SequenceEqual(tags.Select(c => c.Title));
        if (!isAllTagsMatch)
        {
            foreach (var tagTitle in request.Tags)
            {
                tags.Clear();
                var tag = await _tagRepository.FindByAsync(nameof(Domain.Entities.Tag.Title), tagTitle);
                if (tag == null)
                {
                    tag = new Domain.Entities.Tag
                    {
                        Title = tagTitle,
                        IsConfirmed = false
                    };
                    await _tagRepository.CreateAsync(tag);
                }

                tags.Add(tag);
            }
        }

        if (errors.Count != 0)
        {
            throw new CustomBadRequestException(errors);
        }

        var images = await _productRepository.GetImagesByProductIdAsync(product.Id);
        List<SaveFileBase64Model> files = [];
        foreach (var image in request.Images)
        {
            var saveFile = _fileRepository.ReadyToSaveFileAsync(image,
                _filesPath.ProductImage, MaximumFilesSizeInMegaByte.ProductImages);
            files.Add(saveFile);
            images.Add(new ProductImages
            {
                ImageName = saveFile.fileNameWithExtention
            });
        }

        product.Id = product.Id;
        product.Title = request.Title;
        product.EnglishTitle = request.EnglishTitle;
        product.Description = request.Description;
        product.CategoryId = request.CategoryId;
        product.ProductTags = tags.Select(x => new ProductTag
        {
            TagId = x.Id
        }).ToList();
        product.Images = images;
        await using var transaction = await _productRepository.BeginTransactionAsync();
        try
        {
            await _productRepository.DeleteTagsAsync(product.Id);
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            foreach (var file in files)
            {
                await _fileRepository.SaveFileAsync(file);
            }
            var mongoProduct = new MongoProduct
            {
                Id = product.Id,
                Title = product.Title,
                EnglishTitle = product.EnglishTitle,
                CategoryTitle = category.Title,
                Description = product.Description,
                Tags = request.Tags,
                Images = product.Images.Select(x => x.ImageName).ToList(),
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoProduct>(
                new(ActionTypes.Update, mongoProduct),
                RabbitmqConstants.QueueNames.Product,
                RabbitmqConstants.RoutingKeys.Product);
            await transaction.CommitAsync(cancellationToken);
            return new UpdateProductCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var image in request.Images)
            {
                _fileRepository.DeleteFile(image, _filesPath.ProductImage);
            }

            throw;
        }
    }
}