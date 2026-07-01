using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;
    public UserRepository(MyRecipeBookDbContext dbContext) // faz a injecao de dependencia do contexto do banco de dados
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user); // adiciona o usuario no banco de dados
                                                                               // Uma funcao assincrona é basicamente uma função que pode ser pausada e retomada, permitindo que outras operações sejam executadas enquanto a função está aguardando uma operação de I/O (entrada/saída)
   
}
