using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using ClaudeSearch.Application.DTOs;

namespace ClaudeSearch.IntegrationTests.Controllers
{
    public class SearchControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public SearchControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_SearchEndpoint_ReturnsSuccessStatusCode()
        {
            // Arrange
            var query = "test";

            // Act
            var response = await _client.GetAsync($"/api/search?query={query}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_SearchEndpoint_ReturnsExpectedResults()
        {
            // Arrange
            var query = "test";

            // Act
            var response = await _client.GetAsync($"/api/search?query={query}");
            var results = await response.Content.ReadFromJsonAsync<IEnumerable<SearchResultDto>>();

            // Assert
            results.Should().NotBeNull();
            results.Should().BeAssignableTo<IEnumerable<SearchResultDto>>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Get_SearchEndpoint_WithInvalidQuery_ReturnsBadRequest(string invalidQuery)
        {
            // Act
            var response = await _client.GetAsync($"/api/search?query={invalidQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}