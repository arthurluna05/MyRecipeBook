using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using System.Net;
using System.Security.AccessControl;

namespace MyRecipeBook.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context) // implementacao obrigatória do método OnException da interface IExceptionFilter
    {
        if (context.Exception is MyRecipeBookException myRecipeBookException) // verifica se a exceção lançada é do tipo MyRecipeBookException
        {
            context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();

            context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.GetErrorMessages())); 
        }
        else 
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessageException.UNKNOWN_ERROR));
        }
    }
}


