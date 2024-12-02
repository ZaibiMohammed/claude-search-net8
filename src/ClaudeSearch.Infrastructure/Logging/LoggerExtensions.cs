using Microsoft.Extensions.Logging;

namespace ClaudeSearch.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        public static void LogSearchAttempt(this ILogger logger, string query, int resultCount)
        {
            logger.LogInformation(
                "Search attempt - Query: {Query}, Results found: {ResultCount}",
                query,
                resultCount);
        }

        public static void LogSearchError(this ILogger logger, string query, Exception ex)
        {
            logger.LogError(
                ex,
                "Error occurred during search - Query: {Query}, Error: {Error}",
                query,
                ex.Message);
        }

        public static void LogClaudeAIRequest(this ILogger logger, string query)
        {
            logger.LogInformation(
                "Claude AI request initiated - Query: {Query}",
                query);
        }
    }
}