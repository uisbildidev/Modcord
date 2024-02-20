using System.Text;
using Newtonsoft.Json;

namespace Modcord
{
    public struct TokenJsonReader
    {
        public string Token { get; private set; }

        public static async Task<TokenJsonStructure> ReadTokenFromJsonAsync()
        {
            var json = string.Empty;

            using (var fileStream = File.OpenRead("token.json"))
            {
                using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                {
                    json = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            var tokenJson = JsonConvert.DeserializeObject<TokenJsonStructure>(json);
            return tokenJson!;
        }
    }

    public sealed class TokenJsonStructure
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
    }
}
