using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplicationApi.Authentication;

public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
{
    private readonly IConfiguration _config;

    public ApiKeyAuthFilter(IConfiguration config)
    {
        _config = config;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing");
        }

        var apiKey = _config.GetValue<string>(AuthConstants.ApiKeySectionName);

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        }
    }
}
