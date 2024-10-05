using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JackalWebHost2.Infrastructure;

public class BusinessExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled || context.Result != null || context.Exception is not BusinessException businessException)
        {
            return;
        }

        context.Result = new JsonResult(new ErrorModel(businessException));
        context.ExceptionHandled = true;
    }
}