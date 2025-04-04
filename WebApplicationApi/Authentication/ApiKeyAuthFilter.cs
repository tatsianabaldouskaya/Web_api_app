using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace WebApplicationApi.Authentication;

public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(Config.ApiKeyHeader, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing");
        }

        var apiKey = Config.ApiKey;

        if (apiKey.IsNullOrEmpty())
        {
            context.Result = new UnauthorizedObjectResult("Api key is missing in configuration");
        }

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        }
    }
}
