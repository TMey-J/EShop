namespace Blogger.Application.Common.Exceptions
{
    public class CustomInternalServerException(
         string message) : ApplicationException(message)
    {
    }
}
