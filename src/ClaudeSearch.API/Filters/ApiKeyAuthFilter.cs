using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.Filters;

namespace ClaudeSearch.API.Filters
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private const string API_KEY_HEADER_NAME = "X-API-Key";
        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key missing");
                return;
            }

            var apiKey = _configuration["ApiKey"];
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key");
                return;
            }
        }
    }

    public class ApiKeyAuthAttribute : ServiceFilterAttribute
    {
        public ApiKeyAuthAttribute()
            : base(typeof(ApiKeyAuthFilter))
        {
        }
    }
}