namespace WebAppUI;

public class Config
{
    private static Lazy<IConfiguration> _configuration = new (() =>
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    });

    private static IConfiguration Configuration => _configuration.Value;
    public static string ApiKeyHeader => Configuration.GetValue<string>("Authentication:ApiKeyHeader");
    public static string ApiKey => Configuration.GetValue<string>("Authentication:ApiKey");
    public static string SecurityKey => Configuration.GetValue<string>("Authentication:SecurityKey");
}
