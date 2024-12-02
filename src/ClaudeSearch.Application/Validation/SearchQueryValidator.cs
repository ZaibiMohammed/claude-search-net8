using FluentValidation;
using ClaudeSearch.Application.Queries;

namespace ClaudeSearch.Application.Validation
{
    public class SearchQueryValidator : AbstractValidator<SearchQuery>
    {
        public SearchQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .NotEmpty()
                .WithMessage("Search term cannot be empty")
                .MinimumLength(2)
                .WithMessage("Search term must be at least 2 characters long")
                .MaximumLength(100)
                .WithMessage("Search term cannot exceed 100 characters");
        }
    }
}