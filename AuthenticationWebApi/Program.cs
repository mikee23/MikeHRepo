using JwtAuthenticationManager;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<JwtTokenHandler>();
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(); // Add session services

            // Add custom JWT authentication services
            builder.Services.AddCustomJwtAuthentication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSession(); // Ensure session is used before custom JWT bearer
            app.UseCustomJwtBearer(); // Ensure this comes after UseSession

            app.UseAuthentication(); // Ensure this comes after UseCustomJwtBearer
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
