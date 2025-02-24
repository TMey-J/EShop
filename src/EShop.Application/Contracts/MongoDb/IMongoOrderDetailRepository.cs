﻿using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoOrderDetailRepository:IMongoGenericRepository<MongoOrderDetail>
    {
        Task<List<ShowOrderDetailsDto>> GetOrderDetailsByOrderIdAsync(long orderId);
    }
}
