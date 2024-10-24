using Blog.Application.Common.Exceptions;
using Blogger.Application.Common.Helpers;
using FluentValidation;
using MediatR;

namespace EShop.Application.Configs.MediatR;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures =
            await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError
            {
                ErrorMessage = validationFailure.ErrorMessage
                .Replace("{DisplayName}", GetCustomAttribute.GetDisplayName<TRequest>(validationFailure.PropertyName)
                ?? validationFailure.PropertyName),
                PropertyName = validationFailure.PropertyName,
            }).ToList();

        if (errors.Count != 0)
            throw new CustomValidationException(errors);
        var response = await next();
        return response;
    }
}
