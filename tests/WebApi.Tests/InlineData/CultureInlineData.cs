using System.Collections;

namespace WebApi.Tests.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Retorna diferentes culturas para serem usadas como dados de teste
        yield return new object[] { "pt-BR" }; // yield return é usado para retornar um valor de forma iterativa, permitindo que o método seja chamado várias vezes e retorne diferentes valores a cada chamada. Nesse caso, ele retorna um array de objetos contendo a cultura "pt-BR" como primeiro valor.
        yield return new object[] { "en-US" }; // new object é necessario pois o método GetEnumerator() espera um array de objetos como retorno, e o yield return retorna um array de objetos contendo a cultura "en-US" como primeiro valor.
        yield return new object[] { "es" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}