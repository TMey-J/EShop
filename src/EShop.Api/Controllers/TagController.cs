using EShop.Api.Attributes;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

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

        [HttpGet]
        public async Task<IActionResult> GetAll(GetAllTagQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);

        }
        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var response = await _mediator.Send(new GetTagQueryRequest{Id = id});
            return Ok(response);

        }
    }
}
