using App.AuthDemo.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace App.AuthDemo.Services
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthApiClient(
            HttpClient httpClient, 
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:AuthApiBaseUrl"]!;
        }

        //curl -X 'POST' \
        //  'https://localhost:44361/api/auth/login' \
        //  -H 'accept: */*' \
        //  -H 'Content-Type: application/json' \
        //  -d '{
        //  "userName": "admin@authdemo.com",
        //  "password": "Admin123*"
        //}
        public async Task<(bool Success, LoginResponseDto? LoginResponse, string ErrorMessage)> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequestDto
                {
                    UserName = username,
                    Password = password
                };

                var jsonContent = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiResponse?.IsSuccess == true && apiResponse.Result != null)
                    {
                        var loginResponseJson = JsonSerializer.Serialize(apiResponse.Result);
                        var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(loginResponseJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return (true, loginResponse, string.Empty);
                    }
                    else
                    {
                        return (false, null, apiResponse?.Message ?? "Login failed");
                    }
                }
                else
                {
                    // Try to parse error response
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<ResponseDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return (false, null, errorResponse?.Message ?? $"HTTP Error: {response.StatusCode}");
                    }
                    catch
                    {
                        return (false, null, $"HTTP Error: {response.StatusCode} - {responseContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return (false, null, $"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Unexpected error: {ex.Message}");
            }
        }
    }
}