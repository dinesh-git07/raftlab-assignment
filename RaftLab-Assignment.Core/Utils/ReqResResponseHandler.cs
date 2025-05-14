using System.Net.Http;
using System.Text.Json;

namespace RaftLab_Assignment.Utils
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> ReadFromJsonDataFieldAsync<T>(this HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
                throw new JsonException("Empty response received from the server");

            try
            {
                using JsonDocument doc = JsonDocument.Parse(content);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<T>(dataElement.GetRawText(), options);
                }

                throw new JsonException("Could not find 'data' field in the response");
            }
            catch (JsonException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new JsonException("Failed to process the JSON response", ex);
            }
        }
    }
}
