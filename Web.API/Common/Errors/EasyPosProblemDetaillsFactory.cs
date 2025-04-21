using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using Web.API.Common.Https;

namespace Web.API.Common.Errors
{
    public class EasyPosProblemDetaillsFactory(ApiBehaviorOptions options) : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options = options ?? throw new ArgumentNullException(nameof(options));

        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
                Extensions =
                {
                    ["traceId"] = httpContext.TraceIdentifier
                }
            };

            ApplyProblemDetailsDefaults(problemDetails, httpContext, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            ArgumentNullException.ThrowIfNull(modelStateDictionary);

            statusCode ??= StatusCodes.Status400BadRequest;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            if (title != null) problemDetails.Title = title;

            ApplyProblemDetailsDefaults(problemDetails, httpContext, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(ProblemDetails problemDetails, HttpContext httpContext, int statusCode)
        {
            problemDetails.Status = statusCode;

            if(_options.ClientErrorMapping.TryGetValue(statusCode, out var clientError))
            {
                problemDetails.Title = clientError.Title;
                problemDetails.Type = clientError.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }

            if (httpContext.Items[HttpContextItemKeys.Errors] is List<Error> errors && errors.Count > 0)
            {
                problemDetails.Extensions.Add("ErrorCodes", errors.Select(e => e.Code));
            }
        }
    }
}
