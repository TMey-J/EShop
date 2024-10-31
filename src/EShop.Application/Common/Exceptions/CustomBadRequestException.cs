using static EShop.Application.Constants.Common.Messages;

namespace EShop.Application.Common.Exceptions
{
    public class CustomBadRequestException(List<string> errors, string message = Errors.BadRequest)
        : BaseException<string>(errors, message);
}
