using static EShop.Application.Constants.Common.Messages;

namespace Blog.Application.Common.Exceptions
{
    public class CustomValidationException(List<ValidationError> errors, string? message = Errors.Validation) : ApplicationException(message)
    {
        public List<ValidationError> Errors { get; set; }=errors;
    }
    public class ValidationError
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
