using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly string _connectionString = "Data Source=localhost;Initial Catalog=meulivrodereceitas_test;Integrated Security=True;TrustServerCertificate=True;"; // string de conexão para o banco de dados de teste

    protected override void ConfigureWebHost(IWebHostBuilder builder) // sobrescrevendo o método ConfigureWebHost para configurar o ambiente de teste e a configuração da aplicação
    {
        builder.UseEnvironment("Tests").ConfigureAppConfiguration((_, configuration) =>  // definindo o ambiente de teste e adicionando a configuração da aplicação
        {
            var parameters = new Dictionary<string, string?> // criando um dicionário de parâmetros para adicionar à configuração da aplicação
            {
                ["ConnectionStrings:DbConnection"] = _connectionString // adicionando a string de conexão para o banco de dados de teste à configuração da aplicação
            };

            configuration.AddInMemoryCollection(parameters); // adicionando os parâmetros à configuração da aplicação
        });
    }

    public async Task InitializeAsync() // método chamado antes de cada teste para inicializar o banco de dados de teste
    {
        await Task.CompletedTask;

    }

    Task IAsyncLifetime.DisposeAsync() // método chamado após cada teste para limpar o banco de dados de teste
    {
        return Task.CompletedTask;
    }
}
