using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;
    public UserRepository(MyRecipeBookDbContext dbContext) // faz a injecao de dependencia do contexto do banco de dados
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user); // adiciona o usuario no banco de dados
                                                                               // Uma funcao assincrona é basicamente uma função que pode ser pausada e retomada, permitindo que outras operações sejam executadas enquanto a função está aguardando uma operação de I/O (entrada/saída)

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email)); // verifica se existe algum usuário ativo com o email informado, utilizando a função AnyAsync do Entity Framework Core, que retorna true se existir algum usuário que atenda a condição especificada no predicado (user => user.Active && user.Email.Equals(email)), caso contrário retorna false.
    }

    public async Task<User?> GetByEmail(string email) // funcao para buscar um usuario pelo email, para o login, caso nao encontre o usuario, retorna null
    {
        return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email)); // busca o primeiro usuario que atenda a condição especificada no predicado (user => user.Email.Equals(email)), caso não encontre, retorna null 
    }
}
