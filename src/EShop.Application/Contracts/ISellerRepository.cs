namespace EShop.Application.Contracts
{
    public interface ISellerRepository:IGenericRepository<Seller>
    {
        Task<Seller?> FindByIdWithIncludeTypeOfPerson(long sellerId);
    }
}
