using Microsoft.EntityFrameworkCore;

namespace RoleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /* Database Context Dependency Injection */
            /*var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD");*/
            var dbHost = "host.docker.internal"; //this work for localhost only
            //var dbName = "dms_role";
            //var dbPassword = "test123@"; //this works
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD");

            var connectionString = $"server={dbHost};port=3306;database={dbName};user=root;password={dbPassword}";
            builder.Services.AddDbContext<RoleDbContext>(o => o.UseMySQL(connectionString));
            /* ===================================== */
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
