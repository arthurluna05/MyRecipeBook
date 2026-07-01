using System.Diagnostics.CodeAnalysis;

namespace MyRecipeBook.Domain.Extensions;

// funcao de extensao para verificar se uma string é nula ou vazia, e se não é nula ou vazia para evitar o uso de string.IsNullOrWhiteSpace() em todo o código, e para deixar o código mais legível
public static class StringExtension
{
    public static bool IsEmpty([NotNullWhen(false)] this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
    public static bool IsNotEmpty([NotNullWhen(true)] this string? value) 
    {
        return !string.IsNullOrWhiteSpace(value);

    }
}
