using Azure;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests;

public abstract class BaseIntegrationTest: IClassFixture<MyRecipeBookApplicationFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _httpClient; // Cliente HTTP para enviar solicitações para a aplicação web

    internal readonly MyRecipeBookDbContext DbContext; // Contexto do banco de dados para acessar os dados da aplicação 
    public BaseIntegrationTest(MyRecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient(); // Cria um cliente HTTP a partir da fábrica de aplicação web

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>(); // Obtém o contexto do banco de dados a partir do provedor de serviços da fábrica de aplicação web
    }

    protected async Task<HttpResponseMessage> Post(string requestUri, object request, string culture = "en-US")
    {
        ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request); // Envia uma solicitação POST para o endpoint "/users" com o corpo da solicitação em formato JSON, onde o campo "Name" está vazio, esperando que a resposta seja um erro de validação
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear(); // Limpa os cabeçalhos "Accept-Language" da solicitação HTTP
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture); // Define o cabeçalho "Accept-Language" da solicitação HTTP para a cultura especificada, indicando que a resposta esperada deve estar na cultura fornecida
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
