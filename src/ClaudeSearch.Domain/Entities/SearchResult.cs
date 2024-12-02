namespace ClaudeSearch.Domain.Entities
{
    public class SearchResult
    {
        public int Id { get; private set; }
        public string Query { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public float Relevance { get; private set; }

        private SearchResult() { } // For EF Core

        public SearchResult(string query, string content, float relevance)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Relevance = relevance;
            CreatedAt = DateTime.UtcNow;
        }
    }
}