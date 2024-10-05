using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "YXNkZmFzZGZhc2RmYXNkZmFzZGZhc2RmYXNkZmFzZGZhc2Q=";
        private const int JWT_TOKEN_VALIDITY_MINUTES = 20;
        private readonly IMongoCollection<Member> _memberCollection;
        private string JwtSecurityKey = string.Empty;

        public JwtTokenHandler(IConfiguration configuration)
        {
            var dbHost = "host.docker.internal"; //this work for localhost only
            //var dbHost = "localhost"; //this work for localhost only
            var dbName = "dms_member";
            var connectionString = $"mongodb://{dbHost}:27017/{dbName}";

            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            _memberCollection = database.GetCollection<Member>("member");

            var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

            var root = builder.Build();

            // Retrieve JwtSecurityKey from configuration
            JwtSecurityKey = configuration.GetValue<string>("JwtSecurityKey");
        }

        public async Task<IEnumerable<Member>> GetMembers()
        {
            return await _memberCollection.Find(Builders<Member>.Filter.Empty).ToListAsync();
        }

        public async Task<AuthenticationResponse?> GenerateJwtToken(AuthenticationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            // Get members data
            var members = await GetMembers();


            // Validation from database
            var userAccount = members.FirstOrDefault(u => u.Email == request.UserName && u.Password == request.Password);
            if (userAccount == null)
            {
                return null;
            }

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINUTES);

            // Properly decode the Base64-encoded security key
            var tokenKey = Convert.FromBase64String(JwtSecurityKey);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName),
            };

            // Add claims for each role the user has
            foreach (var role in userAccount.RoleDetails)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Description!));
            }
            var claimsIdentity = new ClaimsIdentity(claims);

            //        var claimsIdentity = new ClaimsIdentity(new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Name, request.UserName),

            //    new Claim(ClaimTypes.Role, userAccount.Role)
            //});

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
            );

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = userAccount.Email!,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
