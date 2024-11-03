using EShop.Api.Attributes;
using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiResultFilter]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
