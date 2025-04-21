using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.API.Common.Https;

namespace Web.API.Controllers
{
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<ErrorOr.Error> errors) // Changed return type to ObjectResult
        {
            if (errors != null && errors.Count == 0) return Problem();

            if (errors != null && errors.All(e => e.Type == ErrorOr.ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            if (errors != null && errors.Count > 0) return Problem(errors[0]);

            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An unknown error occurred.");
        }

        private IActionResult Problem(ErrorOr.Error error) // Changed return type to ObjectResult
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        private IActionResult ValidationProblem(List<ErrorOr.Error> errors) // No changes here
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
