using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")] // define a rota para este controlador
[ApiController]
public class UsersController : ControllerBase
{

    [HttpPost] // identifica que é um endpoint do tipo post
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserAccountJson request, [FromServices] IRegisterUserAccountUseCase useCase) // endpoint para registrar um novo user, recebe os dados do usuário no corpo da requisição
    {                                                                             // traz do service o useCase implementado para registrar o usuário
        
        await useCase.Execute(request); // executa o caso de uso passando os dados do usuário para registrar a conta

        return Created(); // retorna status code 201
    }
}
