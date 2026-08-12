using CommomTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Infrastructure.DataAccess;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.User.Register;

public class RegisterUserAccountTests : IClassFixture<MyRecipeBookApplicationFactory> // Classe de teste de integração para o endpoint de registro de conta de usuário
{
    private const string REQUEST_URI = "/users"; // URI do endpoint de registro de conta de usuário

    private readonly HttpClient _httpclient; // Cliente HTTP para enviar solicitações para a aplicação web
    
    private readonly MyRecipeBookDbContext _dbContext; // Contexto do banco de dados para acessar os dados da aplicação 
    public RegisterUserAccountTests(MyRecipeBookApplicationFactory factory)
    {
        _httpclient = factory.CreateClient(); // Cria um cliente HTTP a partir da fábrica de aplicação web

        var scope = factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>(); // Obtém o contexto do banco de dados a partir do provedor de serviços da fábrica de aplicação web
    }

    [Fact]
    public async Task Succes()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var response = await _httpclient.PostAsJsonAsync(REQUEST_URI, request); // Envia uma solicitação POST para o endpoint "/users" com o corpo da solicitação em formato JSON

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync(); // Lê o corpo da resposta como um fluxo de dados, necessario pq o método JsonDocument.ParseAsync espera um Stream como entrada
                                                     //.Content serve para acessar o conteúdo da resposta HTTP, que é retornado como um objeto HttpContent. O método ReadAsStreamAsync() lê o conteúdo da resposta como um fluxo de dados (Stream) de forma assíncrona, permitindo que você processe os dados de forma eficiente, sem precisar carregá-los completamente na memória de uma só vez.
           //  using var nada mais é uma forma de declarar uma variável que será descartada automaticamente quando sair do escopo, garantindo que os recursos associados a ela sejam liberados corretamente. O await é usado para aguardar a conclusão da operação assíncrona ReadAsStreamAsync(), permitindo que o código continue a execução apenas quando o fluxo de dados estiver pronto para ser processado.
        var responseData = await JsonDocument.ParseAsync(responseBody); // Analisa o corpo da resposta como um documento JSON

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name); // Verifica se o valor do campo "name" na resposta corresponde ao valor enviado na solicitação

        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty(); // Verifica se o valor do campo "accessToken" na resposta está vazio

       var userExists = await _dbContext.Users.AnyAsync(user => user.Active && user.Name.Equals(request.Name) && user.Email.Equals(request.Email)); // Verifica se o usuário foi criado no banco de dados, consultando a tabela "Users" para verificar se existe algum registro com o mesmo nome e email enviados na solicitação, e que esteja ativo
        
       userExists.ShouldBeTrue(); // Verifica se o usuário foi criado com sucesso no banco de dados

    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    { 
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        _httpclient.DefaultRequestHeaders.AcceptLanguage.Clear(); // Limpa os cabeçalhos "Accept-Language" da solicitação HTTP
        _httpclient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture); // Define o cabeçalho "Accept-Language" da solicitação HTTP para a cultura especificada, indicando que a resposta esperada deve estar na cultura fornecida

        var response = await _httpclient.PostAsJsonAsync(REQUEST_URI, request); // Envia uma solicitação POST para o endpoint "/users" com o corpo da solicitação em formato JSON, onde o campo "Name" está vazio, esperando que a resposta seja um erro de validação
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray(); // Obtém a lista de erros de validação retornados na resposta, que é um array JSON localizado na propriedade "errors" do objeto JSON da resposta

        var expectedErrorMessage = ResourceMessageException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture)); // Obtém a mensagem de erro esperada para o campo "Name" vazio, localizada no arquivo de recursos ResourceMessageException.resx, usando a chave "VALIDATION_NAME_REQUIRED" e especificando a cultura fornecida para obter a mensagem na cultura correta

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1); // Verifica se há apenas um erro de validação na resposta
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage)); // Verifica se o erro de validação retornado corresponde à mensagem de erro esperada para o campo "Name" vazio
        });

        var userExists = await _dbContext.Users.AnyAsync(user => user.Active && user.Name.Equals(request.Name) && user.Email.Equals(request.Email)); // Verifica se o usuário foi criado no banco de dados, consultando a tabela "Users" para verificar se existe algum registro com o mesmo nome e email enviados na solicitação, e que esteja ativo

        userExists.ShouldBeFalse(); // Verifica se o usuário não foi criado no banco de dados
    }
}
