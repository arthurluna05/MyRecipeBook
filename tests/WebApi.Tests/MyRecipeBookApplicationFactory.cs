using CommomTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using Testcontainers.MsSql;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserIdentityManager User1 { get; private set; } // propriedade para armazenar a identidade do usuário de teste 1

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

        await using var scope = Services.CreateAsyncScope(); // criando um escopo de serviço para acessar os serviços da aplicação
        
        var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>(); 

        var (user, password) = UserBuilder.Build(); // criando um usuário de teste usando o UserBuilder

        user.Password = passwordHasher.HashPassword(password); // gerando o hash da senha do usuário de teste

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        User1 = new UserIdentityManager(user, password); // armazenando a identidade do usuário de teste 1 na propriedade User1
    }

    Task IAsyncLifetime.DisposeAsync() // método chamado após cada teste para limpar o banco de dados de teste
    {
        return Task.CompletedTask;
    }
}
