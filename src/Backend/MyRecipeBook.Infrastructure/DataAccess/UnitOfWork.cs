using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;
// unit of work é uma classe que gerencia a transação do banco de dados, garantindo que todas as operações sejam concluídas com sucesso antes de salvar as alterações no banco de dados. Se alguma operação falhar, todas as alterações são revertidas, garantindo a consistência dos dados.
internal class UnitOfWork : IUnitOfWork
{
    private readonly MyRecipeBookDbContext _dbContext;
    public UnitOfWork(MyRecipeBookDbContext dbContext) // faz a injecao de dependencia do contexto do banco de dados
    {
        _dbContext = dbContext;
    }

    public async Task Commit() => await _dbContext.SaveChangesAsync(); // salva as alteracoes no banco de dados
}
