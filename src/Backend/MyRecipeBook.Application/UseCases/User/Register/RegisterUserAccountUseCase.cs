using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase// classe reponsável pela regra de negócio para registrar um novo usuário
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserAccountUseCase(IPasswordHasher passwordHasher, IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(RequestRegisterUserAccountJson request) // funcao que recebe a requisição para registrar um novo usuário, e deve conter a lógica para validar os dados e criar o usuário no sistema
    {
        ValidateAndThrowOnFailures(request); // chama a funcao para validar

        var user = request.Adapt<Domain.Entities.User>(); // utiliza a biblioteca Mapster (funcao .Adapt) para mapear os dados recebidos na request com os da entidade, passa o caminho da entidade que queremos mapear.
    
        user.Password = _passwordHasher.HashPassword(request.Password); // chama a funcao de hash da senha para gerar o hash da senha recebida na request e atribui ao objeto user.Password

        await _userWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit(); // chama a funcao de commit para salvar as alterações no banco de dados
    }

    private void ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request); // .Validate(request) é um método que RegisterUserAccountValidator herda e deve implementar a lógica de validação dos dados do request.

        if (result.IsValid == false) // verifica se todas as regras foram atendidas com sucesso na validacao, se for falsa lanca exception
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList(); // para cada elemento dessa lista, vou selecionar apenas a mensagem de erro e cria uma lista

            throw new ErrorOnValidationException(errorMessages); // lanca nossa exception personalizada para erro de validacao
        }

    }
}