using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase // interface para injecao de dependencia do RegisterUserAccountUseCase
{
    Task Execute(RequestRegisterUserAccountJson request); // assinatura do método Execute 
}
