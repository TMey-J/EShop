﻿using EShop.Api.Attributes;
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
            var resonse= await _mediator.Send(request);
            return Ok(resonse);

        }
    }
}
