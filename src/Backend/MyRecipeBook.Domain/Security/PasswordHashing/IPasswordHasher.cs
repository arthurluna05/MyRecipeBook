using System.Globalization;

namespace MyRecipeBook.Domain.Security.PasswordHashing;

public interface IPasswordHasher // interface para definir os métodos necessários para realizar o hash e a verificação de senhas, garantindo que qualquer implementação de hash de senha siga a mesma estrutura e possa ser facilmente substituída ou modificada sem afetar o restante do código que depende dela.
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
