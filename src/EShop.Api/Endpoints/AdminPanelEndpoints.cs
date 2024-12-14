using Carter;
using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Application.Features.AdminPanel.Feature.Requests.Commands;
using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;
using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Api.Endpoints;

public class AdminPanelEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/AdminPanel").AddEndpointFilter<ApiResultEndpointFilter>();
        
        #region User

        group.MapPost(nameof(CreateUser), CreateUser);
        group.MapPut(nameof(UpdateUser), UpdateUser);
        group.MapGet(nameof(GetAllUsers), GetAllUsers);
        group.MapGet(nameof(GetUser)+"/{id}", GetUser);

        #endregion

        #region Category

        group.MapPost(nameof(CreateCategory), CreateCategory);
        group.MapPost(nameof(AddFeaturesToCategory), AddFeaturesToCategory);
        group.MapPut(nameof(UpdateCategory), UpdateCategory);
        group.MapGet(nameof(GetAllCategories), GetAllCategories);
        group.MapGet(nameof(GetCategoryFeatures), GetCategoryFeatures);
        group.MapGet(nameof(GetCategory)+"/{id}", GetCategory);

        #endregion

        #region Tag

        group.MapPost(nameof(CreateTag), CreateTag);
        group.MapPut(nameof(UpdateTag), UpdateTag);
        group.MapGet(nameof(GetAllTags), GetAllTags);
        group.MapGet(nameof(GetTag)+"/{id}", GetTag);

        #endregion

        #region Feature

        group.MapPost(nameof(CreateFeature), CreateFeature);
        group.MapPut(nameof(UpdateFeature), UpdateFeature);
        group.MapGet(nameof(GetFeature)+"/{id}", GetFeature);
        group.MapGet(nameof(GetAllFeatures), GetAllFeatures);

        #endregion

        #region Seller

        group.MapGet(nameof(GetSeller)+"/{id}", GetSeller);

        #endregion

    }

    #region APIs Body

    #region User
    private static async Task<IResult> CreateUser(CreateUserCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> UpdateUser(UpdateUserCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    private static async Task<IResult> GetAllUsers(
        [FromBody]GetAllUsersQueryRequest request,
        IMediator mediator)
    {
        var response = await mediator.Send(request);
        return TypedResults.Ok(response);
    }
    private static async Task<IResult> GetUser(long id, IMediator mediator)
    {
        var response = await mediator.Send(new GetUserQueryRequest { Id = id });
        return TypedResults.Ok(response);
    }

    #endregion

    #region Category
    
    private static async Task<IResult> CreateCategory(CreateCategoryCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    private static async Task<IResult> UpdateCategory(UpdateCategoryCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> GetAllCategories(
        [FromBody]GetAllCategoryQueryRequest request,
        IMediator mediator)
    {
        var response = await mediator.Send(request);
        return TypedResults.Ok(response);
    }
    public async Task<IResult> GetCategory(long id, IMediator mediator)
    {
        var response = await mediator.Send(new GetCategoryQueryRequest() { Id = id });
        return TypedResults.Ok(response);
    }
    private static async Task<IResult> AddFeaturesToCategory(AddFeaturesToCategoryCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    private static async Task<IResult> GetCategoryFeatures(
        [FromBody]GetCategoryFeaturesQueryRequest request,
        IMediator mediator)
    {
        var response= await mediator.Send(request);
        return TypedResults.Ok(response);
    }
    #endregion

    #region Tag
    private static async Task<IResult> CreateTag(CreateTagCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    private static async Task<IResult> UpdateTag(UpdateTagCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    private static async Task<IResult> GetAllTags(
        [FromBody]GetAllTagsQueryRequest request,
        IMediator mediator)
    {
        var response = await mediator.Send(request);
        return TypedResults.Ok(response);
    }
    private static async Task<IResult> GetTag(long id, IMediator mediator)
    {
        var response = await mediator.Send(new GetTagQueryRequest { Id = id });
        return TypedResults.Ok(response);
    }

    #endregion

    #region Feature

    private static async Task<IResult> CreateFeature(CreateFeatureCommandRequest request, IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    private static async Task<IResult> GetFeature(long id, IMediator mediator)
    {
        var response = await mediator.Send(new GetFeatureQueryRequest { Id = id });
        return TypedResults.Ok(response);
    }
    private static async Task<IResult> UpdateFeature(UpdateFeatureCommandRequest request, IMediator mediator)
    { 
        await mediator.Send(request);
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> GetAllFeatures(
        [FromBody]GetAllFeaturesQueryRequest request,
        IMediator mediator)
    {
        var response = await mediator.Send(request);
        return TypedResults.Ok(response);
    }
    #endregion

    #region Seller

    private static async Task<IResult> GetSeller(long id, IMediator mediator)
    {
        var response = await mediator.Send(new GetSellerQueryRequest { Id = id });
        return TypedResults.Ok(response);
    }

    #endregion

    #endregion
}