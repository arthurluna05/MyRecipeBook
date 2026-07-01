namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> Errors { get; private set; } // propriedade que armazena a lista de mensagens de erros, e é privada para garantir que só possa ser modificada dentro da classe ResponseErrorJson, e não por código externo.

    public ResponseErrorJson(List<string> errorsMessage) => Errors = errorsMessage; // construtor que recebe uma lista de mensagens de erros e atribui a propriedade Errors, que é a lista de mensagens de erros que será retornada na resposta da API.

    public ResponseErrorJson(string errorMessage) => Errors = [errorMessage]; // construtor que cria uma lista de apenas uma mensagem

}
