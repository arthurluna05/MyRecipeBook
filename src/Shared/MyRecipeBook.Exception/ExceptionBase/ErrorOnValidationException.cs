namespace MyRecipeBook.Exception.ExceptionBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    private readonly List<string> _errors; // readonly é uma propriedade que só pode ser atribuída no momento da declaração ou dentro do construtor da classe, garantindo que a lista de erros seja imutável após a criação da exceção.
    
    public ErrorOnValidationException(List<string> errorMessages) // cria um construtor obrigando a toda vez que essa classe for instanciada, seja necessário passar uma lista de mensagens de erro.
    {
        _errors = errorMessages;
    }

    public List<string> GetErrorMessages() => _errors; // método público que retorna a lista de mensagens de erro armazenada na propriedade _errors.
}
