namespace Blog.Application.Common.Exceptions
{
    public class CustomValidationException(List<ValidationError> errors, string message = Errors.Validation) : 
        BaseException<ValidationError>(errors, message);
    public class ValidationError
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
