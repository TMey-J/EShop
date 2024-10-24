using static EShop.Application.Constants.Common.Messages;

namespace EShop.Application.Common.Exceptions
{
    public class CustomBadRequestException(List<string> errors, string? message = Errors.BadRequest) : ApplicationException(message)
    {
        public List<string> Errors { get; set; } = errors;
    }
}
