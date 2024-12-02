using Microsoft.EntityFrameworkCore;
using ClaudeSearch.Domain.Entities;
using ClaudeSearch.Domain.Repositories;
using ClaudeSearch.Infrastructure.Persistence;

namespace ClaudeSearch.Infrastructure.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly SearchDbContext _context;

        public SearchRepository(SearchDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(
            string query,
            CancellationToken cancellationToken)
        {
            return await _context.SearchResults
                .Where(r => EF.Functions.FreeText(r.Query, query))
                .OrderByDescending(r => r.Relevance)
                .ThenByDescending(r => r.CreatedAt)
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(SearchResult result, CancellationToken cancellationToken)
        {
            await _context.SearchResults.AddAsync(result, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}