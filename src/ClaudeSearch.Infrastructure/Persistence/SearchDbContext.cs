using Microsoft.EntityFrameworkCore;
using ClaudeSearch.Domain.Entities;

namespace ClaudeSearch.Infrastructure.Persistence
{
    public class SearchDbContext : DbContext
    {
        public DbSet<SearchResult> SearchResults { get; set; }

        public SearchDbContext(DbContextOptions<SearchDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchResult>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Query).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Relevance).IsRequired();

                entity.HasIndex(e => e.Query)
                    .IsFullText();
            });
        }
    }
}