namespace EShop.Application.Contracts
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<List<ProductImages>> GetImagesByProductIdAsync(long productId);
        Task<List<Color>> GetProductColorsAsync(long productId);
        Task<List<Tag>> GetProductTagsAsync(long productId);
        Task DeleteTagsAsync(long productId);
    }
}
