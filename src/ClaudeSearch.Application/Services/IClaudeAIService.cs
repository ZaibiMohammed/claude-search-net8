using ClaudeSearch.Domain.Entities;

namespace ClaudeSearch.Application.Services
{
    public interface IClaudeAIService
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string query, CancellationToken cancellationToken);
    }
}