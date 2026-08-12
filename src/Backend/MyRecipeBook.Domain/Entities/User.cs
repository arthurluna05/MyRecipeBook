namespace MyRecipeBook.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public bool Active { get; set; } = true; // propeiedade se user esta ativo
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
