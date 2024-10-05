using PhotoWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace PhotoWebApi
{
    public class PhotoDbContext : DbContext
    {

        public PhotoDbContext(DbContextOptions<PhotoDbContext> dbContextOptions, ILogger<PhotoDbContext> logger)
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

        public DbSet<Photo> Photos { get; set; }
    }
}
