using Moq;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;

namespace CommomTestUtilities.Repositories;

public class IUserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build() // criando um mock do IUserWriteOnlyRepository
    {
        var mock = new Mock<IUserWriteOnlyRepository>(); // criando um mock do IUserWriteOnlyRepository

        return mock.Object; // retornando o objeto mockado do IUserWriteOnlyRepository
    }
}
