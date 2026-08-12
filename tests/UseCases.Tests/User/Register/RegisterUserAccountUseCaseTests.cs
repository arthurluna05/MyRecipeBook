using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;

namespace UseCases.Tests.User.Register;

public class RegisterUserAccountUseCaseTests
{
    [Fact]
    public async Task Succes() 
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        var useCase = CreateUseCase();

        //Act
        var result = await useCase.Execute(request);

        //Assert
        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();

    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty; // Definindo o nome como vazio para simular a validação
        var useCase = CreateUseCase();

        //Act & Assert
        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>(); // Verifica se a exceção é lançada

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages => // Verifica se a lista de mensagens de erro contém a mensagem esperada
        { 
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessageException.VALIDATION_NAME_REQUIRED);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists() 
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        //Act
        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>(); // Verifica se a exceção é lançada

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages => // Verifica se a lista de mensagens de erro contém a mensagem esperada
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessageException.VALIDATION_EMAIL_ALREADY_EXISTS);
        });
    }

    private RegisterUserAccountUseCase CreateUseCase(string? emailThatAlreadyExists = null) 
    {
        // Criando instâncias dos repositórios e do unit of work usando os builders

        var unitOfWork = IUnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var passwordHasher = new IPasswordHasherBuilder().Build();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if(emailThatAlreadyExists.IsNotEmpty()) // Verifica se o e-mail que já existe foi fornecido
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(emailThatAlreadyExists); // Configurando o mock para simular que o e-mail já existe

        return new RegisterUserAccountUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
    }
}
