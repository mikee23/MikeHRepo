using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        public AccountController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        [HttpPost]
        public async Task <ActionResult<AuthenticationResponse?>> Authenticate([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse = await _jwtTokenHandler.GenerateJwtToken(request);
            if (authenticationResponse == null)
            {
                return Unauthorized();
            }

            HttpContext.Session.SetString("Token", authenticationResponse.JwtToken);
            Response.Cookies.Append("AuthToken", authenticationResponse.JwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(20)
            });
            return authenticationResponse;
        }
    }
}
