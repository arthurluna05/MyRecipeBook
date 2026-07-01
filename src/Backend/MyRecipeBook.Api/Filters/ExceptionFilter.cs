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
        if (context.Exception is ErrorOnValidationException errorOnValidationException) // verifica se a exceção lançada é do tipo ErrorOnValidationException
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // se for pode confiar e definir o status code para 400
            
            context.Result = new BadRequestObjectResult(new ResponseErrorJson(errorOnValidationException.GetErrorMessages())); // e retornar um BadRequestObjectResult com a lista de mensagens de erro obtida do método GetErrorMessages da exceção
        }
        else 
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessageException.UNKNOWN_ERROR));
        }
    }
}


