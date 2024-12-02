using AutoMapper;
using MediatR;
using ClaudeSearch.Domain.Repositories;
using ClaudeSearch.Application.Services;

namespace ClaudeSearch.Application.Queries
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, IEnumerable<SearchResultDto>>
    {
        private readonly ISearchRepository _searchRepository;
        private readonly IClaudeAIService _claudeService;
        private readonly IMapper _mapper;

        public SearchQueryHandler(
            ISearchRepository searchRepository,
            IClaudeAIService claudeService,
            IMapper mapper)
        {
            _searchRepository = searchRepository;
            _claudeService = claudeService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchResultDto>> Handle(
            SearchQuery request,
            CancellationToken cancellationToken)
        {
            var dbResults = await _searchRepository.SearchAsync(request.SearchTerm, cancellationToken);

            if (!dbResults.Any())
            {
                var claudeResults = await _claudeService.SearchAsync(request.SearchTerm, cancellationToken);
                
                foreach (var result in claudeResults)
                {
                    await _searchRepository.AddAsync(result, cancellationToken);
                }

                dbResults = claudeResults;
            }

            return _mapper.Map<IEnumerable<SearchResultDto>>(dbResults);
        }
    }
}