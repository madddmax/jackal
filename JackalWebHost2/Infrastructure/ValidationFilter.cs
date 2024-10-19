using JackalWebHost2.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JackalWebHost2.Infrastructure;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var details = context.ModelState
            .Where(x => x.Value?.ValidationState == ModelValidationState.Invalid)
            .Select(x => new ValidationEntryModel
            {
                Property = x.Key,
                Errors = x.Value?.Errors.Select(y => y.ErrorMessage).ToArray() ?? []
            })
            .ToArray();
        
        context.Result = new JsonResult(new ValidationErrorModel("В запросе переданы некорректные данные", details));
        context.HttpContext.Response.StatusCode = 400;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}