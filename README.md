# Claude Search .NET 8

A .NET 8 project that integrates Claude AI for intelligent search capabilities, implemented with Clean Architecture and CQRS pattern.

## Project Overview
This project creates a search system that combines traditional database searching with Claude AI's capabilities to provide intelligent, context-aware search results. It's built with .NET 8 and follows a clean architecture approach with CQRS pattern.

## Core Functionality

### 1. Search Process Flow
When a user makes a search query:

1. **Initial Database Search**
   - The system first checks the local SQL Server database using full-text search
   - Looks for previously cached search results matching the query

2. **Claude AI Integration**
   - If no relevant results are found in the database, the system calls the Claude AI API
   - Claude AI analyzes the query and provides intelligent, contextual responses
   - These responses are then stored in the database for future use

3. **Result Caching**
   - All search results (both from database and Claude AI) are cached
   - Reduces API calls and improves response times for repeated queries

### 2. Key Features

#### Search Capabilities
- Full-text search in SQL Server
- Semantic search through Claude AI
- Result ranking based on relevance
- Support for complex queries
- Contextual understanding of search terms

#### Data Management
- Automatic caching of search results
- Database storage of historical searches
- Full-text indexing for efficient queries
- Automatic clean-up of old cached data

#### API Security
- API key authentication
- Rate limiting to prevent abuse
- Request validation
- Secure error handling

### 3. Use Cases

The project is particularly useful for:

1. **Enterprise Search Solutions**
   - Internal knowledge base searching
   - Document and content discovery
   - Customer support systems

2. **Content Management Systems**
   - Smart content retrieval
   - Related content suggestions
   - Content categorization

3. **Customer Service Applications**
   - FAQ automation
   - Support ticket classification
   - Knowledge base integration

### 4. Technical Implementation

#### Architecture Layers
1. **Domain Layer**
   - Core business logic
   - Entity definitions
   - Business rules

2. **Application Layer**
   - Use case implementation
   - CQRS commands and queries
   - DTOs and mappings

3. **Infrastructure Layer**
   - Database access
   - Claude AI integration
   - External service connections

4. **API Layer**
   - REST endpoints
   - Request/response handling
   - Authentication/authorization

### 5. Performance Features

1. **Caching System**
   - In-memory caching for frequent queries
   - Configurable cache duration
   - Cache invalidation strategies

2. **Rate Limiting**
   - Prevents API abuse
   - Configurable limits per client
   - Queue management for high-load scenarios

3. **Database Optimization**
   - Full-text search indexing
   - Efficient query patterns
   - Performance-optimized schema

### 6. Monitoring and Maintenance

1. **Health Checks**
   - Database connectivity monitoring
   - Claude AI API availability checks
   - System resource monitoring

2. **Logging**
   - Structured logging with Serilog
   - Query performance tracking
   - Error tracking and reporting

3. **Diagnostics**
   - Performance metrics
   - Usage statistics
   - Error rate monitoring

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or Docker for containerized database)
- Claude AI API key

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

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.