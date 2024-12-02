using Microsoft.EntityFrameworkCore;
using ClaudeSearch.Infrastructure.Persistence;
using ClaudeSearch.Infrastructure.Services;
using ClaudeSearch.Application.Services;
using ClaudeSearch.Domain.Repositories;
using ClaudeSearch.Infrastructure.Repositories;
using ClaudeSearch.Application.Queries;
using ClaudeSearch.API.Middleware;
using ClaudeSearch.API.HealthChecks;
using ClaudeSearch.Infrastructure.Caching;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(SearchQuery).Assembly));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(SearchQuery).Assembly);

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<SearchQueryValidator>();

// Add Caching
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SearchDbContext>()
    .AddCheck<ClaudeAIHealthCheck>("ClaudeAI");

// Add DbContext
builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ClaudeSearch.Infrastructure")));

// Add HTTP client for Claude AI
builder.Services.AddHttpClient<IClaudeAIService, ClaudeAIService>();

// Register repositories and services
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

// Configure health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SearchDbContext>();
    db.Database.Migrate();
}

app.Run();