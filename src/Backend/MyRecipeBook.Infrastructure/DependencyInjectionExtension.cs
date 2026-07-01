using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension // classe para configuracao do servico de injecao de dependencia 
{

    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration) // funcao para adicionar os servicos de infraestrutura na injecao de dependencia
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>(); // adiciona o servico de hash de senha Argon2 como implementacao da interface IPasswordHasher

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<MyRecipeBookDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection");

                config.UseSqlServer(connectionString);
            });

        }

    }
}
