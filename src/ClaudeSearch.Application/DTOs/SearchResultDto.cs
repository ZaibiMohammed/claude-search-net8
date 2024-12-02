namespace ClaudeSearch.Application.DTOs
{
    public class SearchResultDto
    {
        public string Query { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public float Relevance { get; set; }
    }
}