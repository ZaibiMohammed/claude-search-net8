using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ClaudeSearch.Application.Services;
using ClaudeSearch.Domain.Entities;

namespace ClaudeSearch.Infrastructure.Services
{
    public class ClaudeAIService : IClaudeAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<ClaudeAIService> _logger;

        public ClaudeAIService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ClaudeAIService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ClaudeAI:ApiKey"] 
                ?? throw new ArgumentNullException("Claude AI API key not found in configuration");
            _logger = logger;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(
            string query,
            CancellationToken cancellationToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var request = new
                {
                    model = "claude-3-opus-20240229",
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = query
                        }
                    },
                    max_tokens = 1000
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "https://api.anthropic.com/v1/messages",
                    request,
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ClaudeResponse>(cancellationToken);

                return new[]
                {
                    new SearchResult(
                        query,
                        result.Content,
                        1.0f)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while querying Claude AI");
                throw;
            }
        }
    }
}