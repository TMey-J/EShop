using Carter;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Api.Endpoints
{
    public class AuthorizationEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Authorization").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Register), Register);
            group.MapPatch(nameof(VerifyPhoneNumber) + "/{phoneNumber}", VerifyPhoneNumber);
            group.MapPost(nameof(ReSendVerificationCode), ReSendVerificationCode);
            group.MapPost(nameof(Login), Login);
            group.MapGet(nameof(VerifyEmail) + "/{email}/{token}", VerifyEmail);
        }

        #region Api Bodies

        private static async Task<IResult> Register(
            RegisterCommandRequest request
            , IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> VerifyEmail(
            [FromQuery] string token,
            [FromQuery] string email,
            IMediator mediator)
        {
            await mediator.Send(new VerifyEmailCommandRequest { Email = email, Token = token });
            return TypedResults.Ok();
        }

        private static async Task<IResult> VerifyPhoneNumber(string phoneNumber,
            [FromBody] VerifyPhoneNumberCommandRequest request,
            IMediator mediator)
        {
            request.PhoneNumber = phoneNumber;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> ReSendVerificationCode(
            ReSendVerificationCideCommandRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> Login(
            LoginCommandRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        #endregion
    }
}