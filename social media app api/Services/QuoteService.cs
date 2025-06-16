using social_media_app_api.DTO;
using System.Text.Json;

namespace social_media_app_api.Services
{
    public class QuoteService
    {
        private readonly HttpClient _httpClient;

        public QuoteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<QuoteDto?>> GetRandomQuoteAsync()
        {
            var response = await _httpClient.GetAsync("https://zenquotes.io/api/random");

            if (!response.IsSuccessStatusCode)
            {
                return null; // eller throw, beroende på vad du föredrar
            }

            var json = await response.Content.ReadAsStringAsync();

            var quotes = JsonSerializer.Deserialize<List<QuoteDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return quotes;
        }
    }

}
