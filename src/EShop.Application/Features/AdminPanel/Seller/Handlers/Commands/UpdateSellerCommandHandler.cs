using EShop.Application.Features.AdminPanel.Seller.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Seller.Handlers.Commands;

public class UpdateSellerCommandHandler(
    ISellerRepository sellerRepository,
    ICityRepository cityRepository,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IRabbitmqPublisherService rabbitmqPublisher) :
    IRequestHandler<UpdateSellerCommandRequest, UpdateSellerCommandResponse>
{
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<UpdateSellerCommandResponse> Handle(UpdateSellerCommandRequest request,
        CancellationToken cancellationToken)
    {
        var seller = await _sellerRepository.FindByIdWithIncludeTypeOfPerson(request.Id) ??
                     throw new CustomBadRequestException(["فروشنده یافت نشد"]);

        var city = await _cityRepository.FindByIdAsync(request.CityId) ??
                   throw new CustomBadRequestException(["شهر یافت نشد"]);


        #region Map Seller

        seller.UserName = seller.User?.UserName ?? throw new CustomBadRequestException(["کاربر یافت نشد"]);

        seller.Address = request.Address;
        seller.CityId = city.Id;
        seller.City = city;
        seller.Website = request.Website;
        seller.IsActive = true;
        seller.DocumentStatus = DocumentStatus.Confirmed;
        seller.PostalCode = request.PostalCode;
        seller.ShopName = request.ShopName;

        #endregion

        if (seller is { LegalSeller: not null, IsLegalPerson: true })
        {
            if (request.LegalSeller == null)
            {
                throw new CustomBadRequestException(["اطلاعات مربوط فرد حقوقی را وارد کنید"]);
            }

            #region Map Legal Seller

            seller.LegalSeller.CompanyName = request.LegalSeller.CompanyName;
            seller.LegalSeller.EconomicCode = request.LegalSeller.EconomicCode;
            seller.LegalSeller.CompanyType = request.LegalSeller.CompanyType;
            seller.LegalSeller.RegisterNumber = request.LegalSeller.RegisterNumber;
            seller.LegalSeller.SignatureOwners = request.LegalSeller.SignatureOwners;
            seller.LegalSeller.ShabaNumber = request.LegalSeller.ShabaNumber;

            #endregion

            if (seller.LegalSeller?.ShabaNumber != request.LegalSeller.ShabaNumber)
            {
                //TODO: check for valid shaba number 
            }

            if (seller.LegalSeller?.RegisterNumber != request.LegalSeller.RegisterNumber)
            {
                //TODO: check for register number
            }
        }

        if (seller is { IndividualSeller: not null, IsLegalPerson: false })
        {
            if (request.IndividualSeller == null)
            {
                throw new CustomBadRequestException(["اطلاعات مربوط فرد حقوقی را وارد کنید"]);
            }

            #region Map Individual Seller

            seller.IndividualSeller.NationalId = request.IndividualSeller.NationalId;
            seller.IndividualSeller.CartOrShebaNumber = request.IndividualSeller.CartOrShebaNumber;
            seller.IndividualSeller.AboutSeller = request.IndividualSeller.AboutSeller;

            #endregion

            if (seller.IndividualSeller?.NationalId != request.IndividualSeller.NationalId)
            {
                //TODO:check for valid nationalId id
            }

            if (seller.IndividualSeller?.CartOrShebaNumber != request.IndividualSeller.CartOrShebaNumber)
            {
                //TODO: check for valid cart or shaba number 
            }
        }

        SaveFileBase64Model? saveFile = null;

        if (!string.IsNullOrWhiteSpace(request.Logo))
        {
            saveFile = _fileRepository.ReadyToSaveFileAsync(request.Logo,
                _filesPath.SellerLogo, MaximumFilesSizeInMegaByte.SellerLogo, seller.Logo);
            seller.Logo = saveFile.fileNameWithExtention;
        }

        await using var transaction = await _sellerRepository.BeginTransactionAsync();
        try
        {
            _sellerRepository.Update(seller);
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
            await _rabbitmqPublisher.PublishMessageAsync<Domain.Entities.Seller>(new(ActionTypes.Update, seller),
                RabbitmqConstants.QueueNames.Seller,
                RabbitmqConstants.RoutingKeys.Seller);
            await transaction.CommitAsync(cancellationToken);
            return new UpdateSellerCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}