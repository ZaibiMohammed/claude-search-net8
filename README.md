# Claude Search .NET 8

A .NET 8 project that integrates Claude AI for intelligent search capabilities, implemented with Clean Architecture and CQRS pattern.

## Features

- Clean Architecture with CQRS pattern
- Integration with Claude AI API
- SQL Server with Full-Text Search
- Caching of search results
- Rate limiting
- Health checks
- Structured logging with Serilog
- Comprehensive error handling
- Unit and Integration tests
- Docker support
- API authentication

## Prerequisites

- .NET 8 SDK
- SQL Server (or Docker for containerized database)
- Claude AI API key

## Project Structure

```
ClaudeSearch/
├── src/
│   ├── ClaudeSearch.Domain/        # Entities, interfaces, domain logic
│   ├── ClaudeSearch.Application/   # Use cases, CQRS handlers
│   ├── ClaudeSearch.Infrastructure/# External services, data access
│   └── ClaudeSearch.API/           # Web API, controllers
└── tests/
    ├── ClaudeSearch.UnitTests/
    └── ClaudeSearch.IntegrationTests/
```

## Getting Started

### Local Development

1. Clone the repository:
```bash
git clone https://github.com/YourUsername/claude-search-net8.git
cd claude-search-net8
```

2. Update the connection string in `src/ClaudeSearch.API/appsettings.json`

3. Add your Claude AI API key to the configuration

4. Run Entity Framework migrations:
```bash
dotnet ef migrations add InitialCreate -p src/ClaudeSearch.Infrastructure -s src/ClaudeSearch.API
dotnet ef database update -p src/ClaudeSearch.Infrastructure -s src/ClaudeSearch.API
```

5. Run the application:
```bash
cd src/ClaudeSearch.API
dotnet run
```

### Docker Deployment

1. Build and run with Docker Compose:
```bash
docker-compose up --build
```

This will start:
- The API on http://localhost:5000 (HTTP) and https://localhost:5001 (HTTPS)
- SQL Server on port 1433

## API Endpoints

### Search
```http
GET /api/search?query={searchTerm}
```

Parameters:
- `query` (required): The search term to query

Headers:
- `X-API-Key`: Your API key for authentication

Example Response:
```json
[
  {
    "query": "example search",
    "content": "Search result content",
    "createdAt": "2024-12-02T10:00:00Z",
    "relevance": 0.95
  }
]
```

### Health Check
```http
GET /health
```

## Rate Limiting

The API implements rate limiting with the following default configuration:
- 100 requests per minute per client
- Queue limit of 2 requests

Configuration can be adjusted in `appsettings.json`.

## Caching

Search results are cached for 10 minutes by default. The cache duration can be configured in the `CacheService`.

## Testing

Run unit tests:
```bash
dotnet test tests/ClaudeSearch.UnitTests
```

Run integration tests:
```bash
dotnet test tests/ClaudeSearch.IntegrationTests
```

## Development Guides

### Adding New Features

1. Add domain entities in `ClaudeSearch.Domain`
2. Create corresponding DTOs in `ClaudeSearch.Application`
3. Implement CQRS handlers in `ClaudeSearch.Application`
4. Add required infrastructure in `ClaudeSearch.Infrastructure`
5. Create API endpoints in `ClaudeSearch.API`
6. Add tests

### Database Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName -p src/ClaudeSearch.Infrastructure -s src/ClaudeSearch.API
```

Apply migrations:
```bash
dotnet ef database update -p src/ClaudeSearch.Infrastructure -s src/ClaudeSearch.API
```

## Error Handling

The application uses a global exception handling middleware that returns consistent error responses:

```json
{
  "statusCode": 400,
  "message": "Error message"
}
```

## Logging

Structured logging is implemented using Serilog. Logs are written to:
- Console
- Daily rolling file (logs/log-.txt)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
