# Claude Search .NET 8

A .NET 8 project that integrates Claude AI for intelligent search capabilities, implemented with Clean Architecture and CQRS pattern.

## Features

- Clean Architecture with CQRS pattern
- Integration with Claude AI
- SQL Server with Full-Text Search
- Caching of search results
- Async operations throughout
- Proper error handling and logging

## Prerequisites

- .NET 8 SDK
- SQL Server
- Claude AI API key

## Getting Started

1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Add your Claude AI API key to the configuration
4. Run Entity Framework migrations:

```bash
dotnet ef migrations add InitialCreate -p src/ClaudeSearch.Infrastructure -s src/ClaudeSearch.API
dotnet ef database update -p src/ClaudeSearch.Infrastructure -s src/ClaudeSearch.API
```

## Project Structure

- `ClaudeSearch.Domain`: Contains the domain entities and interfaces
- `ClaudeSearch.Application`: Contains application logic, CQRS handlers
- `ClaudeSearch.Infrastructure`: Contains external service implementations and data access
- `ClaudeSearch.API`: Web API project

## Architecture

This project follows Clean Architecture principles and implements the CQRS pattern using MediatR.