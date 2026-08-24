using System.Net;

namespace MyRecipeBook.Exception.ExceptionBase;

public class InvalidLoginException : MyRecipeBookException
{
    public override List<string> GetErrorMessages() => [ResourceMessageException.VALIDATION_LOGIN_INVALID];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
