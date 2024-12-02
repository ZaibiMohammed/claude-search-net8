FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/ClaudeSearch.Domain/ClaudeSearch.Domain.csproj", "src/ClaudeSearch.Domain/"]
COPY ["src/ClaudeSearch.Application/ClaudeSearch.Application.csproj", "src/ClaudeSearch.Application/"]
COPY ["src/ClaudeSearch.Infrastructure/ClaudeSearch.Infrastructure.csproj", "src/ClaudeSearch.Infrastructure/"]
COPY ["src/ClaudeSearch.API/ClaudeSearch.API.csproj", "src/ClaudeSearch.API/"]

RUN dotnet restore "src/ClaudeSearch.API/ClaudeSearch.API.csproj"

# Copy all files and build
COPY . .
WORKDIR "/src/src/ClaudeSearch.API"
RUN dotnet build "ClaudeSearch.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClaudeSearch.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClaudeSearch.API.dll"]