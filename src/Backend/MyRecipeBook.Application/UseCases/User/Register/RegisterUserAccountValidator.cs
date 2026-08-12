using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson> // herdando validator do FluentValidation para para validar os dados recebidos pela request de register
{
    public RegisterUserAccountValidator() // é necessario construtor, pois é onde vamos definir as validacoes do fluent validation
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessageException.VALIDATION_NAME_REQUIRED);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessageException.VALIDATION_EMAIL_REQUIRED);
        RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessageException.VALIDATION_PASSWORD_REQUIRED);
        When(user => user.Email.IsNotEmpty(), () => // se o email não for vazio, when = quando,  executa uma funcao lambda validando se o email é válido
        { 
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessageException.VALIDATION_EMAIL_IS_VALID);
        });
    }
}
