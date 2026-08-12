using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly MyRecipeBookDbContext _dbContext;
    public UnitOfWork(MyRecipeBookDbContext dbContext) // faz a injecao de dependencia do contexto do banco de dados
    {
        _dbContext = dbContext;
    }

    public async Task Commit() => await _dbContext.SaveChangesAsync(); // salva as alteracoes no banco de dados
}
