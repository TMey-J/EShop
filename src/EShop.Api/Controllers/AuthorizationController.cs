using EShop.Api.Attributes;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiResultFilter]
    public class AuthorizationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommandRequest request)
        {
            var resonse = await _mediator.Send(request);
            return Ok(resonse);
        }
        [HttpGet]
        public async Task<IActionResult> VerifyEmail([FromQuery]string token, [FromQuery] string email)
        {
            await _mediator.Send(new VerifyEmailCommandRequest { Email = email, Token = token });
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost()]
        public async Task<IActionResult> ReSendVerificationCode(ReSendVerificationCideCommandRequest request)
        {
            var response= await _mediator.Send(request);
            return Ok(response);
        }
    }
}
