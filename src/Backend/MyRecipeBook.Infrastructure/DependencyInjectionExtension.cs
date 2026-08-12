using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension // classe para configuracao do servico de injecao de dependencia 
{

    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration) // funcao para adicionar os servicos de infraestrutura na injecao de dependencia
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>(); // adiciona o servico de hash de senha Argon2 como implementacao da interface IPasswordHasher

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<MyRecipeBookDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection");

                config.UseSqlServer(connectionString);
            });

            services.AddFluentMigratorCore().ConfigureRunner(config =>
            {
                

                config.AddSqlServer().WithGlobalConnectionString(_ => // passando uma funcao como parametro, que recebe a configuracao e retorna a string de conexao do banco de dados
                {
                    var connectionString = configuration.GetConnectionString("DbConnection");

                    return connectionString;
                }).ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All(); // configura o FluentMigrator para usar o SQL Server como banco de dados e define a string de conexão global para todas as migrations. Em seguida, ele escaneia o assembly "MyRecipeBook.Infrastructure" em busca de todas as classes de migration e as registra no runner.
            });
        }

    }
}
