using System.Net;

namespace MyRecipeBook.Exception.ExceptionBase;

public abstract class MyRecipeBookException : System.Exception // transforma em exception, e bloqueia novas instancias por colocar abstract, ou seja, só pode ser herdada, e não instanciada diretamente
{
    public abstract HttpStatusCode GetStatusCode(); // força a implementação do método GetStatusCode nas classes filhas, que retorna o status code da exception
    public abstract List<string> GetErrorMessages(); // força a implementação do método GetErrorMessages nas classes filhas, que retorna uma lista de mensagens de erro
    
}
