using EShop.Application.Features.AdminPanel.Product.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.AdminPanel.Product.Handlers.Commands;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IColorRepository colorRepository,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IRabbitmqPublisherService rabbitmqPublisher) :
    IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IColorRepository _colorRepository = colorRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByAsync(nameof(Domain.Entities.Product.Title), request.Title);
        if (product != null)
        {
            throw new CustomBadRequestException([Errors.DuplicatedValue("محصول")]);
        }

        var category = await _categoryRepository.FindByIdWithIncludeFeatures(request.CategoryId)
                       ?? throw new CustomBadRequestException(["دسته بندی یافت نشد"]);
        List<string> errors = [];

        List<Domain.Entities.Tag> tags = [];
        foreach (var tagTitle in request.Tags)
        {
            var tag = await _tagRepository.FindByAsync(nameof(Domain.Entities.Tag.Title), tagTitle);
            if (tag == null)
            {
                tag = new Domain.Entities.Tag
                {
                    Title = tagTitle
                };
                await _tagRepository.CreateAsync(tag);
            }

            tags.Add(tag);
        }

        if (errors.Count != 0)
        {
            throw new CustomBadRequestException(errors);
        }
        var categoryFeature = category.CategoryFeatures?.Select(x=>x.Feature).ToList()??[];
        if (!categoryFeature.All(x => request.Features.ContainsKey(x.Name)))
        {
            throw new CustomBadRequestException(["ویژگی های دسته بندی باید وجود داشته باشد"]);
        }
        var productFeature = request.Features.Select(x => new ProductFeature
        {
            FeatureName = x.Key,
            FeatureValue = x.Value
        }).ToList();
        List<ProductImages> images = [];
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

        product = new Domain.Entities.Product
        {
            Title = request.Title,
            EnglishTitle = request.EnglishTitle,
            Description = request.Description,
            CategoryId = request.CategoryId,
            ProductTags = tags.Select(x => new ProductTag
            {
                TagId = x.Id
            }).ToList(),
            Images = images,
            ProductFeatures = productFeature
        };
        await using var transaction = await _productRepository.BeginTransactionAsync();
        try
        {
            await _productRepository.CreateAsync(product);
            await _productRepository.SaveChangesAsync();
            foreach (var file in files)
            {
                await _fileRepository.SaveFileAsync(file);
            }

            #region publish message

            var mongoProduct = new MongoProduct
            {
                Id = product.Id,
                Title = product.Title,
                EnglishTitle = product.EnglishTitle,
                CategoryId = category.Id,
                CategoryTitle = category.Title,
                Description = product.Description,
                Tags = request.Tags,
                Images = product.Images.Select(x => x.ImageName).ToList(),
                Features = product.ProductFeatures.ToDictionary(x=>x.FeatureName,x=>x.FeatureValue)
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoProduct>(
                new(ActionTypes.Create, mongoProduct),
                RabbitmqConstants.QueueNames.Product,
                RabbitmqConstants.RoutingKeys.Product);

            #endregion

            await transaction.CommitAsync(cancellationToken);
            return new CreateProductCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var image in product.Images)
            {
                _fileRepository.DeleteFile(image.ImageName, _filesPath.ProductImage);
            }

            throw;
        }
    }
}