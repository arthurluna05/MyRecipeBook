using CommomTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exception;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Tests.User.Register;

public class RegisterUserAccountValidatorTests // teste de unidade para o validator RegisterUserAccountValidator
{
    [Fact] // é necessário para que o xUnit reconheça o método como um teste
    public void Succes() 
    {
        //Arange
        var request = RequestRegisterUserAccountJsonBuilder.Build(); // instancia necessaria para a request 

        var validator = new RegisterUserAccountValidator(); // instancia necessaria para o validator
        
        //Act
        var result = validator.Validate(request); // pega a request e valida ela com o validator e armazena em result

        //Assert

        result.IsValid.ShouldBeTrue();// verifica se o resultado da validação é verdadeiro, ou seja, se a request é válida
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("          ")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intetional because is a Unit Test")]
    public void Validate_ShouldHaveError_WhenNameOrEmailEmpty(string name)
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = name;
        
        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors => // verifica se a lista de erros contém apenas um erro
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessageException.VALIDATION_NAME_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("          ")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intetional because is a Unit Test")]
    public void Validate_ShouldHaveError_WhenEmailIsEmpty(string email)
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = email;

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors => // verifica se a lista de erros contém apenas um erro
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessageException.VALIDATION_EMAIL_REQUIRED));
        });
    }
    
    [Fact]
    public void Validate_ShouldHaveError_WhenPasswordIsEmpty()
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Password = string.Empty; // setando a senha como vazio para testar a validação

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors => // verifica se a lista de erros contém apenas um erro
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessageException.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = "invalid-email"; // setando o email como inválido para testar a validação

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors => // verifica se a lista de erros contém apenas um erro
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessageException.VALIDATION_EMAIL_IS_VALID));
        });
    }
}
