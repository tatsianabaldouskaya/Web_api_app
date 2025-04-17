using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;

namespace Taf.Core;

public class TestDbContext
{
    public static AppDbContext CreateDbContext()
    {
        var connectionString = Config.DbConnectionString;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }
}
