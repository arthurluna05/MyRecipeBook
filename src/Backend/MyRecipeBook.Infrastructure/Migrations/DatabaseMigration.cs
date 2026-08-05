using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Infrastructure.Migrations;

public class DatabaseMigration
{
    public static void ExecuteMigrations(IServiceProvider serviceProvider) // funcao para executar as migrations do banco de dados
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>(); 

        runner.ListMigrations();

        runner.MigrateUp();
    } 
}
