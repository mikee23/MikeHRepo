using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CallAnotherApiAsync()
        {
            // Retrieve the token from the cookie
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not available
                throw new InvalidOperationException("Token is not available.");
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.secondapi.com/endpoint");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                // Process the response data as needed
            }
            else
            {
                // Handle failure (e.g., log the error, throw an exception, etc.)
            }
        }
    }
}
