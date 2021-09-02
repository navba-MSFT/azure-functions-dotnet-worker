using System.Text.Json;

namespace FunctionApp
{
    internal static class SharedJsonSettings
    {
        public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };
    }
}
