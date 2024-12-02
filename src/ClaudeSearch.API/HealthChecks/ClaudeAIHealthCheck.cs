using Microsoft.Extensions.Diagnostics.HealthChecks;
using ClaudeSearch.Application.Services;

namespace ClaudeSearch.API.HealthChecks
{
    public class ClaudeAIHealthCheck : IHealthCheck
    {
        private readonly IClaudeAIService _claudeService;

        public ClaudeAIHealthCheck(IClaudeAIService claudeService)
        {
            _claudeService = claudeService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var results = await _claudeService.SearchAsync("test", cancellationToken);
                return HealthCheckResult.Healthy("Claude AI service is healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Claude AI service is unhealthy", ex);
            }
        }
    }
}