using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using ClaudeSearch.Domain.Entities;
using ClaudeSearch.Domain.Repositories;
using ClaudeSearch.Application.Services;
using ClaudeSearch.Application.Queries;

namespace ClaudeSearch.UnitTests.Queries
{
    public class SearchQueryHandlerTests
    {
        private readonly Mock<ISearchRepository> _mockSearchRepository;
        private readonly Mock<IClaudeAIService> _mockClaudeService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SearchQueryHandler _handler;

        public SearchQueryHandlerTests()
        {
            _mockSearchRepository = new Mock<ISearchRepository>();
            _mockClaudeService = new Mock<IClaudeAIService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new SearchQueryHandler(
                _mockSearchRepository.Object,
                _mockClaudeService.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenDbResultsExist_ShouldReturnMappedResults()
        {
            // Arrange
            var query = new SearchQuery("test");
            var dbResults = new[] { new SearchResult("test", "content", 1.0f) };
            var expectedDtos = new[] { new SearchResultDto { Query = "test" } };

            _mockSearchRepository.Setup(x => x.SearchAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbResults);

            _mockMapper.Setup(x => x.Map<IEnumerable<SearchResultDto>>(dbResults))
                .Returns(expectedDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDtos);
            _mockClaudeService.Verify(
                x => x.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNoDbResults_ShouldQueryClaudeAI()
        {
            // Arrange
            var query = new SearchQuery("test");
            var claudeResults = new[] { new SearchResult("test", "AI response", 1.0f) };
            var expectedDtos = new[] { new SearchResultDto { Query = "test" } };

            _mockSearchRepository.Setup(x => x.SearchAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<SearchResult>());

            _mockClaudeService.Setup(x => x.SearchAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(claudeResults);

            _mockMapper.Setup(x => x.Map<IEnumerable<SearchResultDto>>(claudeResults))
                .Returns(expectedDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDtos);
            _mockSearchRepository.Verify(
                x => x.AddAsync(It.IsAny<SearchResult>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}