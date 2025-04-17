using Microsoft.Extensions.Configuration;

namespace Taf.Core;
public static class Config
{
    private static Lazy<IConfiguration> _configuration = new(() =>
    {
        var basePath = AppContext.BaseDirectory;
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    });

    private static IConfiguration Configuration => _configuration.Value;
    public static string DbConnectionString => Configuration.GetValue<string>("ConnectionStrings:BookshopDatabase");
    public static string BaseUrl => Configuration.GetValue<string>("BaseUrl");
}
