using Microsoft.AspNetCore.Localization;
using MyRecipeBook.Api.Converters;
using MyRecipeBook.Api.Filters;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Migrations;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(); // adicionando o SwaggerGen para gerar a documentação do Swagger

builder.Services.AddInfrastructure(builder.Configuration); // adicionando os servicos de infraestrutura na injecao de dependencia
builder.Services.AddApplication(); // adicionando os servicos de aplicacao na injecao de dependencia

builder.Services.Configure<RequestLocalizationOptions>(options => // configurando as opções de localização para a aplicação por uma funcao lambda, onde options é o objeto de configuração que vamos configurar para definir as culturas suportadas pela aplicação e a cultura padrão. RequestLocalizationOptions é uma classe do ASP.NET Core que contém as opções de localização para a aplicação.
{
    var supportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("pt-BR"), new CultureInfo("es") }; // definindo as culturas suportadas pela aplicação

    options.DefaultRequestCulture = new RequestCulture("en"); // definindo ingles como a padrao

    options.SupportedCultures = supportedCultures; // definindo as culturas suportadas para formatação de dados, como datas e números
    options.SupportedUICultures = supportedCultures; // definindo as culturas suportadas para localização de recursos, como mensagens de erro e textos da interface do usuário

    options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()]; // definindo o provedor de cultura a partir do cabeçalho Accept-Language
});

builder.Services.AddMvc(option => option.Filters.Add<ExceptionFilter>());

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UseRequestLocalization();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // adicionando o middleware do Swagger para servir a documentação gerada
    app.UseSwaggerUI(); // adicionando o middleware do Swagger UI para fornecer uma interface de usuário para a documentação do Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await ExecuteMigrations(); // executando as migrations do banco de dados antes de iniciar a aplicação

app.Run();

async Task ExecuteMigrations() 
{
    await using var scope = app.Services.CreateAsyncScope();

    DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
}

public partial class Program { }