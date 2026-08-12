using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MyRecipeBook.Api.Converters;

public class StringConverter : JsonConverter<string> // classe para sobrescerver o conversor padrao do .NET para strings, permitindo-nos a tratar a string por conta propria
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()?.Trim();
        if(value is null)
            return value;

        return Regex.Replace(value, @"\s+", " "); // regex para remover todos os espacoes em brancos em apenas um espaco em branco, ou seja, se houver mais de um espaco em branco entre as palavras, ele vai substituir por apenas um espaco em branco
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
