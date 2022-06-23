using FluentValidation;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Behaviors
{

    public class BehaviorValidation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : class
        where TRequest : IValidateable
    {
        private readonly IValidator<TRequest> _compositeValidator;

        public BehaviorValidation(IValidator<TRequest> compositeValidator)
        {
            _compositeValidator = compositeValidator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = await _compositeValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {
                    var resultType = responseType.GetGenericArguments()[0];
                    var invalidResponseType = typeof(ValidateableResponse<>).MakeGenericType(resultType);

                    var errorsGrouped = result.Errors.GroupBy(
                    s => s.ErrorMessage,
                    s => s.PropertyName,
                    (errorMessage, fieldName) => new
                    {
                        ErrorMessage = errorMessage,
                        FieldName = fieldName.First()

                    }).ToList();

                    var errorResponses = new List<ErrorResponse>();

                    foreach(var error in errorsGrouped)
                    {
                        errorResponses.Add(new ErrorResponse { FieldName = error.FieldName, Error = error.ErrorMessage });
                    };

                    var invalidResponse =
                        Activator.CreateInstance(invalidResponseType, null, errorResponses) as TResponse;

                    return invalidResponse;
                }
            }

            var response = await next();

            return response;

        }

    }


}

