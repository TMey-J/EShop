using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Application.Features.AdminPanel.User.Requests.Commands;

namespace EShop.Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[ApiResultFilter]
public class AdminPanelController(IMediator mediator):ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateUser(UpdateUserCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    #region Category

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCategories(GetAllCategoryQueryRequest request)
    {
        var response= await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCategory(long id)
    {
        var response= await _mediator.Send(new GetCategoryQueryRequest(){Id = id});
        return Ok(response);
    }

    #endregion

    #region Tag

    [HttpPost]
    public async Task<IActionResult> CreateTag(CreateTagCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> UpdateTag(UpdateTagCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags(GetAllTagQueryRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);

    }
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTag(long id)
    {
        var response = await _mediator.Send(new GetTagQueryRequest{Id = id});
        return Ok(response);

    }

    #endregion
}