using EShop.Api.Attributes;
using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using EShop.Application.Features.AdminPanel.Requests.Queries.Category;

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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var response= await _mediator.Send(new GetAllCategoryQueryRequest());
            return Ok(response);
        }
    }
}
