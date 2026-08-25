using CommomTestUtilities.Requests;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/authentication";

    private readonly UserIdentityManager _user1;

    public LoginWithEmailAndPasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success() 
    {
        var request = new RequestLoginJson
        { 
            Email = _user1.GetEmail(),                        
            Password = _user1.GetPassword()
        };

        var response = await Post(REQUEST_URI, request); // Envia uma solicitação POST para o endpoint "/users" com o corpo da solicitação em formato JSON

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync(); // Lê o corpo da resposta como um fluxo de dados, necessario pq o método JsonDocument.ParseAsync espera um Stream como entrada
                                                                                   //.Content serve para acessar o conteúdo da resposta HTTP, que é retornado como um objeto HttpContent. O método ReadAsStreamAsync() lê o conteúdo da resposta como um fluxo de dados (Stream) de forma assíncrona, permitindo que você processe os dados de forma eficiente, sem precisar carregá-los completamente na memória de uma só vez.
                                                                                   //  using var nada mais é uma forma de declarar uma variável que será descartada automaticamente quando sair do escopo, garantindo que os recursos associados a ela sejam liberados corretamente. O await é usado para aguardar a conclusão da operação assíncrona ReadAsStreamAsync(), permitindo que o código continue a execução apenas quando o fluxo de dados estiver pronto para ser processado.
        var responseData = await JsonDocument.ParseAsync(responseBody); // Analisa o corpo da resposta como um documento JSON

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.GetName()); // Verifica se o valor do campo "name" na resposta corresponde ao valor enviado na solicitação

        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty(); // Verifica se o valor do campo "accessToken" na resposta está vazio

    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_WhenUserDontExist(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();


        var response = await Post(REQUEST_URI, request, culture); // Envia uma solicitação POST para o endpoint "/users" com o corpo da solicitação em formato JSON, onde o campo "Name" está vazio, esperando que a resposta seja um erro de validação
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray(); // Obtém a lista de erros de validação retornados na resposta, que é um array JSON localizado na propriedade "errors" do objeto JSON da resposta

        var expectedErrorMessage = ResourceMessageException.ResourceManager.GetString("VALIDATION_LOGIN_INVALID", new CultureInfo(culture)); // Obtém a mensagem de erro esperada para o campo "Name" vazio, localizada no arquivo de recursos ResourceMessageException.resx, usando a chave "VALIDATION_NAME_REQUIRED" e especificando a cultura fornecida para obter a mensagem na cultura correta

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1); // Verifica se há apenas um erro de validação na resposta
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage)); // Verifica se o erro de validação retornado corresponde à mensagem de erro esperada para o campo "Name" vazio
        });
    }
}
