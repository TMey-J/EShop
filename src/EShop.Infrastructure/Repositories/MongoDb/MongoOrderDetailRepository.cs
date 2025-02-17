using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoOrderDetailRepository(MongoDbContext mongoDb) :
        MongoGenericRepository<MongoOrderDetail>(mongoDb,MongoCollectionsName.OrderDetail), IMongoOrderDetailRepository
    {
        private readonly IMongoCollection<MongoOrderDetail> _orderDetail =
            mongoDb.GetCollection<MongoOrderDetail>(MongoCollectionsName.OrderDetail);
        private readonly IMongoCollection<MongoSellerProduct> _sellerProduct =
            mongoDb.GetCollection<MongoSellerProduct>(MongoCollectionsName.SellerProduct);
        private readonly IMongoCollection<MongoProduct> _product =
            mongoDb.GetCollection<MongoProduct>(MongoCollectionsName.Product);
        private readonly IMongoCollection<MongoColor> _color =
            mongoDb.GetCollection<MongoColor>(MongoCollectionsName.Color);
        public async Task<List<ShowOrderDetailsDto>> GetOrderDetailsByOrderIdAsync(long orderId)
        {
            var orderQueryable = from orderDetails in _orderDetail.AsQueryable().Where(x => x.OrderId == orderId)
                join sellerProduct in _sellerProduct on orderDetails.ProductId equals sellerProduct.ProductId
                where orderDetails.SellerId == sellerProduct.SellerId && orderDetails.ColorId == sellerProduct.ColorId
                join product in _product on sellerProduct.ProductId equals product.Id
                join color in _color on sellerProduct.ColorId equals color.Id
                select new ShowOrderDetailsDto
                {
                    Id = orderDetails.Id,
                    Title = product.Title,
                    Count = orderDetails.Count,
                    Image = product.Images.First(),
                    ColorName = color.Name,
                    ColorCode = color.Code,
                    BasePrice = sellerProduct.BasePrice,
                    DiscountPercentage = sellerProduct.DiscountPercentage,
                    EndOfDiscount = sellerProduct.EndOfDiscount
                };
            var orders = await MongoQueryable.ToListAsync(orderQueryable);
            foreach (var order in orders)
            {
                order.PriceWithDiscount=MathHelper.CalculatePriceWithDiscount(order.BasePrice, order.DiscountPercentage);
            }
            return orders;

        }
    }
}