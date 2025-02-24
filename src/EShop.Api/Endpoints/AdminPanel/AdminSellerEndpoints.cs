﻿using EShop.Application.Features.AdminPanel.Seller.Requests.Commands;
using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;

namespace EShop.Api.Endpoints.AdminPanel
{
    public class AdminSellerEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Admin/Seller").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
            group.MapPut(nameof(Update)+ "/{id}", Update);
            group.MapGet(nameof(GetAll), GetAll);
            group.MapGet(nameof(Get) + "/{id}", Get);
        }

        #region Api Bodies
        private static async Task<IResult> Get(long id, IMediator mediator)
        {
            var response = await mediator.Send(new GetSellerQueryRequest { Id = id });
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> GetAll(
            [FromBody] GetAllSellersQueryRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> Create(CreateSellerCommandRequest request, IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> Update(long id,[FromBody]UpdateSellerCommandRequest request, IMediator mediator)
        {
            request.Id = id;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        #endregion
    }
}