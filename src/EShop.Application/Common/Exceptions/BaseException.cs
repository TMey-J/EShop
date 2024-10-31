namespace EShop.Application.Common.Exceptions
{
    public class BaseException<T>(List<T> errors, string message): ApplicationException(message)
    {
        public List<T> Errors { get; set; } = errors;
    }

}
