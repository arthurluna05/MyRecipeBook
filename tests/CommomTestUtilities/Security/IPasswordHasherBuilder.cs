using Moq;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;

namespace CommomTestUtilities.Security;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock; // Criando um mock do IPasswordHasher
    public IPasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();

        _mock.Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>())).Returns("hashed-password"); // Configura o comportamento do método HashPassword para retornar uma senha hash simulada
    }

    public void VerifyPassword(string password)
    {
        _mock.Setup(repository => repository.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build() => _mock.Object;
}
