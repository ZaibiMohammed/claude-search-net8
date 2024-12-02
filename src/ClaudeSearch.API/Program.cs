using Microsoft.EntityFrameworkCore;
using ClaudeSearch.Infrastructure.Persistence;
using ClaudeSearch.Infrastructure.Services;
using ClaudeSearch.Application.Services;
using ClaudeSearch.Domain.Repositories;
using ClaudeSearch.Infrastructure.Repositories;
using ClaudeSearch.Application.Queries;

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

// Add DbContext
builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ClaudeSearch.Infrastructure")));

// Add HTTP client for Claude AI
builder.Services.AddHttpClient<IClaudeAIService, ClaudeAIService>();

// Register repositories and services
builder.Services.AddScoped<ISearchRepository, SearchRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
