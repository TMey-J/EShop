namespace Blogger.Application.Common.Exceptions
{
    public class CustomInternalServerException(List<string> errors,
         string message=Errors.InternalServer) : BaseException<string>(errors, message);
}
