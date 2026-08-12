using Moq;
using MyRecipeBook.Domain.Repositories.User;
using System.Threading.Tasks.Sources;

namespace CommomTestUtilities.Repositories;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock;
    public IUserReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email) 
    {
        _mock.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true); // Configura o comportamento do método ExistActiveUserWithEmail para retornar true quando chamado com o email especificado
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}
