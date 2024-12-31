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

        var category = await _categoryRepository.FindByIdAsync(request.CategoryId)
                       ?? throw new CustomBadRequestException(["دسته بندی یافت نشد"]);
        List<string> errors = [];

        List<Color> colors = [];
        foreach (var colorCode in request.ColorsCode)
        {
            var color = await _colorRepository.FindByAsync(nameof(Color.ColorCode), colorCode);
            if (color == null)
            {
                errors.Add($"رنگی با کد {colorCode} موجود نیست");
            }
            else
            {
                colors.Add(color);
            }
        }

        List<Domain.Entities.Tag> tags = [];
        foreach (var tagTitle in request.Tags)
        {
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

        if (errors.Count != 0)
        {
            throw new CustomBadRequestException(errors);
        }

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
        product = new Domain.Entities.Product {
            Title = request.Title,
            EnglishTitle = request.EnglishTitle,
            Description = request.Description,
            DiscountPercentage = request.DiscountPercentage == 0 ? null : request.DiscountPercentage,
            BasePrice = request.BasePrice,
            EndOfDiscount = request.EndOfDiscount,
            CategoryId = request.CategoryId,
            Colors = colors,
            Tags = tags,
            Images = images,
            SellersProducts = new List<SellerProduct>()
            {
                new()
                {
                    Count = request.Count, SellerId = request.SellerId
                }
            }
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

            var colorsToDictionary = colors.ToDictionary(color => color.ColorCode, color => color.ColorName);
            var mongoProduct = new MongoProduct {
                Id = product.Id,
                Title = product.Title,
                EnglishTitle = product.EnglishTitle,
                BasePrice = product.BasePrice,
                DiscountPercentage = product.DiscountPercentage,
                EndOfDiscount = product.EndOfDiscount,
                Count = request.Count,
                CategoryTitle = category.Title,
                Description = product.Description,
                Colors = colorsToDictionary,
                Tags = request.Tags,
                Images = product.Images.Select(x => x.ImageName).ToList(),
                SellerId = request.SellerId,
                SellerProduct = new MongoSellerProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    SellerId = request.SellerId,
                    Count = request.Count,
                    ProductId = product.Id
                }
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoProduct>(
                new(ActionTypes.Create, mongoProduct),
                RabbitmqConstants.QueueNames.Product,
                RabbitmqConstants.RoutingKeys.Product);

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