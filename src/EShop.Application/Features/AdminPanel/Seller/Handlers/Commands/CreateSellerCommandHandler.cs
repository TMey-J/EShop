using EShop.Application.Features.AdminPanel.Seller.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

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
        var seller = new Domain.Entities.Seller() {
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
            //TODO: check for valid shaba number
            //TODO: check for register number
            seller.LegalSeller = new LegalSeller()
            {
                CompanyName = request.LegalSeller.CompanyName!,
                EconomicCode = request.LegalSeller.EconomicCode,
                CompanyType = request.LegalSeller.CompanyType,
                RegisterNumber = request.LegalSeller.RegisterNumber!,
                SignatureOwners = request.LegalSeller.SignatureOwners!,
                ShabaNumber = request.LegalSeller.ShabaNumber
            };
            
        }
        else if (request.IndividualSeller != null)
        {
            //TODO:check for valid nationalId id
            //TODO:check for valid shaba or cart number
            seller.IndividualSeller = new IndividualSeller()
            {
                NationalId = request.IndividualSeller.NationalId!,
                CartOrShebaNumber = request.IndividualSeller.CartOrShebaNumber,
                AboutSeller = request.IndividualSeller.AboutSeller
            };
            
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

            var mongoCity = new MongoCity()
            {
                Id = city.Id,
                ProvinceId = city.ProvinceId,
                Title = city.Title,
                IsDelete = city.IsDelete,
                Province = new MongoProvince()
                {
                 Id   = city.Province!.Id,
                 Title = city.Province!.Title
                }
            };
            MongoLegalSeller? mongoLegalSeller = null;
            if (seller.LegalSeller != null)
            {
                mongoLegalSeller =new MongoLegalSeller
                {
                    CompanyName = seller.LegalSeller.CompanyName,
                    EconomicCode = seller.LegalSeller.EconomicCode,
                    CompanyType = seller.LegalSeller.CompanyType,
                    RegisterNumber = seller.LegalSeller.RegisterNumber,
                    SignatureOwners = seller.LegalSeller.SignatureOwners,
                    ShabaNumber = seller.LegalSeller.ShabaNumber
                };
            }
            MongoIndividualSeller? mongoIndividualSeller = null;
            if (seller.IndividualSeller != null)
            {
                mongoIndividualSeller= new MongoIndividualSeller()
                {
                    NationalId = seller.IndividualSeller.NationalId,
                    CartOrShebaNumber = seller.IndividualSeller.CartOrShebaNumber,
                    AboutSeller = seller.IndividualSeller.AboutSeller
                };
            }
            
            var mongoSeller = new MongoSeller() {
                Id = seller.Id,
                UserId = seller.UserId,
                UserName = seller.UserName,
                IsLegalPerson = seller.IsLegalPerson,
                Address = seller.Address,
                CityId = seller.CityId,
                City = mongoCity,
                Website = seller.Website,
                IsActive = true,
                DocumentStatus = DocumentStatus.Confirmed,
                PostalCode = seller.PostalCode,
                ShopName = seller.ShopName,
                Logo = seller.Logo,
                RejectReason = seller.RejectReason,
                LegalSeller = mongoLegalSeller,
                IndividualSeller = mongoIndividualSeller
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoSeller>(new(ActionTypes.Create, mongoSeller),
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