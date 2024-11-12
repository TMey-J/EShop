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
}