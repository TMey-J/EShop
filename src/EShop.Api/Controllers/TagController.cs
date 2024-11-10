using EShop.Api.Attributes;
using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using EShop.Application.Features.AdminPanel.Requests.Commands.Tag;
using EShop.Application.Features.AdminPanel.Requests.Queries.Category;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiResultFilter]
    public class TagController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTagCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
        
    }
}
