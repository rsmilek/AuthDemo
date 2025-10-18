using App.AuthDemo.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace App.AuthDemo.Services
{
    public class WeatherForecastApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public WeatherForecastApiClient(
            HttpClient httpClient, 
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:WeatherForecastApiBaseUrl"]!;
        }

        // curl -X 'GET' \
        // 'https://localhost:44397/api/weatherforecast' \
        // -H 'accept: text/plain' \
        // -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFkbWluQGF1dGhkZW1vLmNvbSIsInN1YiI6IjZlNmQ2MjJhLTFhNDktNGU2MC1iZmZiLTk3ZWQ2MWQyMGEwZSIsIm5hbWUiOiJhZG1pbkBhdXRoZGVtby5jb20iLCJuYmYiOjE3NjA4MDA3OTksImV4cCI6MTc2MTQwNTU5OSwiaWF0IjoxNzYwODAwNzk5LCJpc3MiOiJhdXRoLWRlbW8tYXBpIiwiYXVkIjoiYXV0aC1kZW1vLWNsaWVudCJ9.xIF2JgLLJZcVJHZaSMzkZ3McpWx0yfo2P6kFPvU_W-c'
        public async Task<(bool Success, WeatherForecastDto[]? WeatherForecasts, string ErrorMessage)> GetWeatherForecastAsync(string jwtToken)
        {
            try
            {
                // Add JWT token to Authorization header
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

                var response = await _httpClient.GetAsync($"{_baseUrl}/api/weatherforecast");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var weatherForecasts = JsonSerializer.Deserialize<WeatherForecastDto[]>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, weatherForecasts, string.Empty);
                }
                else
                {
                    return (false, null, $"HTTP Error: {response.StatusCode} - {responseContent}");
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