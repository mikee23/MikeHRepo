using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using RoleWebApi.Models;

namespace RoleWebApi
{
    public class RoleDbContext : DbContext
    {
        public RoleDbContext(DbContextOptions<RoleDbContext> dbContextOptions, ILogger<RoleDbContext> logger)
        : base(dbContextOptions)
        {
            try
            {
                logger.LogInformation("Checking if the database can connect.");
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect())
                    {
                        logger.LogInformation("Database does not exist. Creating database...");
                        databaseCreator.Create();
                        logger.LogInformation("Database created successfully.");
                    }

                    if (!databaseCreator.HasTables())
                    {
                        logger.LogInformation("No tables found. Creating tables...");
                        databaseCreator.CreateTables();
                        logger.LogInformation("Tables created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error: " + ex.Message);
            }
        }

        public DbSet<Role> Roles { get; set; }
    }
}
