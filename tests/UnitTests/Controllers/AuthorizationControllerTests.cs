using Blog.Application.Common.Exceptions;
using EShop.Api.Controllers;
using EShop.Application.Constants.Common;
using EShop.Application.Features.Authorize.Requests.Commands;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    public class AuthorizationControllerTests
    {
        private readonly IMediator _mediator;
        private readonly AuthorizationController _controller;

        public AuthorizationControllerTests()
        {
            _mediator = A.Fake<IMediator>();
            _controller = new AuthorizationController(_mediator);
        }

        [Fact]
        public async void Register_ShouldReturnOkObjectResult()
        {
            var request = A.Fake<RegisterCommandRequest>();
            var response = A.Fake<RegisterCommandResponse>();

            A.CallTo(() => _mediator.Send(request,default)).Returns(response);

            var result = await _controller.Register(request);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
        } 
    }
}
