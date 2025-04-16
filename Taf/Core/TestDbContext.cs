using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;

namespace Taf.Core;

public class TestDbContext
{
    public static AppDbContext CreateDbContext()
    {
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BookshopDb;Trusted_Connection=True;";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }
}
