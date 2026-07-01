using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess;
internal class MyRecipeBookDbContext : DbContext // classe que representa o contexto do banco de dados, qual vamos usar etc
{
    public MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { } // configuracao para passar os dados de configuracao do banco de dados
    
    public DbSet<User> Users { get; set; } //fazendo a relacao de classe(codigo) para tabela(banco de dados)

    
}