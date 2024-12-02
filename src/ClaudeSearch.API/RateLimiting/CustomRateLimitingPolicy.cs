using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ClaudeSearch.API.RateLimiting
{
    public class CustomRateLimitingPolicy
    {
        public static RateLimiterOptions GetRateLimitingPolicy(IConfiguration configuration)
        {
            var rateLimitingOptions = new RateLimiterOptions
            {
                RejectionStatusCode = StatusCodes.Status429TooManyRequests
            };

            rateLimitingOptions.AddFixedWindowLimiter(policyName: "fixed", options =>
            {
                options.PermitLimit = configuration.GetValue<int>("RateLimiting:PermitLimit");
                options.Window = TimeSpan.Parse(configuration.GetValue<string>("RateLimiting:Window"));
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = configuration.GetValue<int>("RateLimiting:QueueLimit");
            });

            return rateLimitingOptions;
        }
    }
}