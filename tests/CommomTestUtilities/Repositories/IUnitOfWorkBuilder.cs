using Moq;
using MyRecipeBook.Domain.Repositories;

namespace CommomTestUtilities.Repositories;
// mock nada mais é que uma simulação de um objeto real, que pode ser usado para testar o comportamento de um sistema sem depender de implementações reais. No caso do IUnitOfWork, estamos criando um mock para simular o comportamento do repositório de unidade de trabalho, permitindo que possamos testar o código que depende dele sem precisar de uma implementação real.
public class IUnitOfWorkBuilder
{
    public static IUnitOfWork Build() // criando um mock do IUnitOfWork
    {
        var mock = new Mock<IUnitOfWork>(); // criando um mock do IUnitOfWork

        return mock.Object; // retornando o objeto mockado do IUnitOfWork
    }
}
