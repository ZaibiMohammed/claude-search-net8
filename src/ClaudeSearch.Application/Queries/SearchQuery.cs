using MediatR;

namespace ClaudeSearch.Application.Queries
{
    public record SearchQuery(string SearchTerm) : IRequest<IEnumerable<SearchResultDto>>;
}