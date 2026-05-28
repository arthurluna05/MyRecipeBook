using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Api.Controllers;

[Route("api/[controller]")] // define a rota para este controlador, por exemplo: api/users
[ApiController]
public class UsersController : ControllerBase
{

    [HttpPost] // identifica que é um endpoint do tipo post
    public IActionResult Register([FromBody] RequestRegisterUserAccountJson request) // endpoint para registrar um novo user, recebe os dados do usuário no corpo da requisição
    {
        return Created(); // retorna status code 201
    }
}
