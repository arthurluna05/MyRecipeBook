using Konscious.Security.Cryptography;
using MyRecipeBook.Domain.Security.PasswordHashing;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.PasswordHashing;

internal sealed class Argon2PasswordHasher : IPasswordHasher // as classes no projetos de infra vao ser todos internal, herda uma interface para garantir que a classe tenha os métodos necessários para realizar o hash e a verificação de senhas, e é selada para evitar que outras classes herdem dela, garantindo que a implementação seja única e não possa ser modificada por herança.
{
    private const int DEGREE_OF_PARALLELISM = 1; // define o grau de paralelismo para o algoritmo de hash Argon2id, que é a quantidade de threads que serão utilizadas para processar o hash da senha. Quanto maior o grau de paralelismo, mais rápido será o processamento do hash, mas também mais recursos do sistema serão utilizados.
    private const int ITERATIONS = 2; // numero de vezes que o algoritmo de hash sera executado, quanto mais iterações, mais seguro será o hash, mas também mais tempo levará para gerar o hash.
    private const int MEMORY_SIZE = 20 * 1024; // 20 MB, tamanho da memoria que o algoritmo de hash vai utilizar para processar o hash da senha em KB, multiplica por 1024 para ter o valor em MB.
    private const int SALT_SIZE = 16; // valor em bytes, define o tamanho do salt que será gerado para o hash da senha, quanto maior o salt, mais seguro será o hash, mas também mais espaço de armazenamento será necessário para armazenar o hash da senha.
    private const int HASH_SIZE = 32; // valor em bytes, define o tamanho do hash que será gerado para a senha, quanto maior o hash, mais seguro será o hash, mas também mais espaço de armazenamento será necessário para armazenar o hash da senha.
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE); // valores aleatorios que serao utilizado para gerar o hash da senha, garantindo que mesmo que duas senhas sejam iguais, os hashes vao ser diferentes.

        var hash = HashPassword(password, salt);

        var combinedBytes = new byte[hash.Length + salt.Length]; // cria um array de bytes para armazenar o salt e o hash da senha juntos, para que possamos armazenar ambos em um único campo no banco de dados, facilitando a verificação da senha posteriormente.

        salt.CopyTo(combinedBytes);
        hash.CopyTo(combinedBytes, index: salt.Length); // copia o salt para o início do array combinado e o hash para a posição logo após o salt, garantindo que ambos sejam armazenados juntos no mesmo array de bytes.

        return Convert.ToBase64String(combinedBytes); // converte o array de bytes combinado (salt + hash) para uma string em Base64, que é uma representação legível do hash da senha, e retorna essa string.
    }

    public bool VerifyPassword(string password, string passwordHash) // funcao para verificar se a senha(hash) do login é a mesma armazenada no bd
    {
        var combinedBytes = Convert.FromBase64String(passwordHash); // converte a string do hash da senha armazenada no banco de dados de volta para um array de bytes, para que possamos extrair o salt e o hash da senha para comparação.

        var salt = new byte[SALT_SIZE];
        var hash = new byte[HASH_SIZE];

        Array.Copy(combinedBytes, salt, SALT_SIZE); // copia os primeiros 16 elementos do combinedBytes e cola no array salt
        Array.Copy(combinedBytes, SALT_SIZE, hash, 0, HASH_SIZE); // copia os próximos 32 elementos do combinedBytes a partir da posicao 16 (após o salt(passando o SALT_SIZE(16))) e cola no array hash na posicao 0, garantindo que o salt e o hash sejam extraídos corretamente do array combinado.
        
        var newHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(hash, newHash); // faz a comparação entre o hash extraído do banco de dados e o hash gerado a partir da senha fornecida pelo usuário, utilizando uma comparação de tempo fixo para evitar ataques de timing, garantindo que a verificação da senha seja segura contra ataques de força bruta.
    }

    private byte[] HashPassword(string password, byte[] salt) // Overload de metodo para gerar o hash da senha utilizando o salt, esse método é utilizado tanto para gerar o hash da senha quanto para verificar a senha, garantindo que o mesmo processo de hash seja utilizado em ambos os casos, e que o salt seja aplicado corretamente ao gerar o hash da senha.
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password); // converte a senha de string para um array de bytes utilizandp uma funcao do proprio .NET chamada Encoding 

        var hashAlgorithm = new Argon2id(passwordBytes) // cria uma instância do algoritmo de hash Argon2id, que vai ser utilizado para gerar o hash da senha
        {
            DegreeOfParallelism = DEGREE_OF_PARALLELISM,
            Iterations = ITERATIONS,
            MemorySize = MEMORY_SIZE,
            Salt = salt
        };

        return hashAlgorithm.GetBytes(HASH_SIZE); // gera o hash da senha utilizando o algoritmo de hash Argon2id, passando o tamanho do hash que queremos gerar.
    }
}
