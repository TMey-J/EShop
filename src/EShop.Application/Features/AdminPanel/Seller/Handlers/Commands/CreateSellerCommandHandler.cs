using EShop.Application.Features.AdminPanel.Seller.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Seller.Handlers.Commands;

public class CreateSellerCommandHandler(
    ISellerRepository sellerRepository,
    ICityRepository cityRepository,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IApplicationUserManager userManager,
    IRabbitmqPublisherService rabbitmqPublisher) :
    IRequestHandler<CreateSellerCommandRequest, CreateSellerCommandResponse>
{
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<CreateSellerCommandResponse> Handle(CreateSellerCommandRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString()) ??
                   throw new CustomBadRequestException(["کاربر یافت نشد"]);
        var isUserASeller = await _sellerRepository.IsExistsByAsync(nameof(Domain.Entities.Seller.UserId), user.Id);
        if (isUserASeller)
        {
            throw new CustomBadRequestException(["این کاربر از قبل یک فروشنده است"]);
        }

        var city = await _cityRepository.FindByIdAsync(request.CityId) ??
                   throw new CustomBadRequestException(["شهر یافت نشد"]);
        var seller = new Domain.Entities.Seller()
        {
            UserId = user.Id,
            UserName = user.UserName,
            IsLegalPerson = request.IsLegalPerson,
            Address = request.Address,
            CityId = city.Id,
            City = city,
            Website = request.Website,
            IsActive = true,
            DocumentStatus = DocumentStatus.Confirmed,
            PostalCode = request.PostalCode,
            ShopName = request.ShopName
        };
        if (request.LegalSeller != null)
        {
            seller.LegalSeller = new LegalSeller()
            {
                CompanyName = request.LegalSeller.CompanyName!,
                EconomicCode = request.LegalSeller.EconomicCode,
                CompanyType = request.LegalSeller.CompanyType,
                RegisterNumber = request.LegalSeller.RegisterNumber!,
                SignatureOwners = request.LegalSeller.SignatureOwners!,
                ShabaNumber = request.LegalSeller.ShabaNumber
            };
            //TODO: check for valid shaba number
            //TODO: check for register number
        }
        else if (request.IndividualSeller != null)
        {
            seller.IndividualSeller = new IndividualSeller()
            {
                NationalId = request.IndividualSeller.NationalId!,
                CartOrShebaNumber = request.IndividualSeller.CartOrShebaNumber,
                AboutSeller = request.IndividualSeller.AboutSeller
            };
            //TODO:check for valid nationalId id
            //TODO:check for valid shaba or cart number
        }
        else
        {
            throw new CustomBadRequestException(["فروشنده یا باید فردحقیقی باشد یا فرد حقوقی"]);
        }

        SaveFileBase64Model? saveFile = null;

        if (!string.IsNullOrWhiteSpace(request.Logo))
        {
            saveFile = _fileRepository.ReadyToSaveFileAsync(request.Logo,
                _filesPath.SellerLogo, MaximumFilesSizeInMegaByte.SellerLogo);
            seller.Logo = saveFile.fileNameWithExtention;
        }

        await using var transaction = await _sellerRepository.BeginTransactionAsync();
        try
        {
            await _sellerRepository.CreateAsync(seller);
            await _sellerRepository.SaveChangesAsync();

            if (saveFile is not null)
            {
                await _fileRepository.SaveFileAsync(saveFile);
            }

            seller.User = null;
            if (seller.IndividualSeller is not null)
                seller.IndividualSeller.Seller = null;
            if (seller.LegalSeller is not null)
                seller.LegalSeller.Seller = null;
            await _rabbitmqPublisher.PublishMessageAsync<Domain.Entities.Seller>(new(ActionTypes.Create, seller),
                RabbitmqConstants.QueueNames.Seller,
                RabbitmqConstants.RoutingKeys.Seller);
            
            await transaction.CommitAsync(cancellationToken);
            return new CreateSellerCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            
            if (saveFile is not null)
            {
                _fileRepository.DeleteFile(saveFile.fileNameWithExtention, _filesPath.SellerLogo);
            }

            throw;
        }
    }
}