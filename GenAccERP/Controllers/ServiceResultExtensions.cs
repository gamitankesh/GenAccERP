using GenAccERP.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    internal static class ServiceResultExtensions
    {
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, ServiceResult<T> result) =>
            result.IsSuccess ? controller.Ok(result.Value) : controller.Failure<T>(result);

        public static ActionResult<T> ToCreatedResult<T>(this ControllerBase controller, ServiceResult<T> result, string actionName, Func<T, object> routeValues) =>
            result.IsSuccess ? controller.CreatedAtAction(actionName, routeValues(result.Value!), result.Value) : controller.Failure<T>(result);

        public static IActionResult ToNoContentResult(this ControllerBase controller, ServiceResult<bool> result) =>
            result.IsSuccess ? controller.NoContent() : controller.Failure<bool>(result).Result!;

        private static ActionResult<T> Failure<T>(this ControllerBase controller, ServiceResult<T> result) => result.Status switch
        {
            ServiceStatus.NotFound => controller.NotFound(),
            ServiceStatus.Conflict => controller.Conflict(result.Error),
            _ => controller.BadRequest(result.Error)
        };
    }
}
