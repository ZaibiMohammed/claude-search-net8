namespace ClaudeSearch.Domain.Repositories
{
    public interface ISearchRepository
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string query, CancellationToken cancellationToken);
        Task AddAsync(SearchResult result, CancellationToken cancellationToken);
    }
}