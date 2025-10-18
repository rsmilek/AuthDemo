using App.AuthDemo.Models;
using App.AuthDemo.Services;
using Microsoft.Extensions.Configuration;

namespace App.AuthDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Auth Demo Application ===");
            Console.WriteLine("This application demonstrates calling the Authorize secured WeatherForecast endpoint.\n");

            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Create HttpClient with a timeout
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Create the Auth API client with configuration
            var authApiClient = new AuthApiClient(httpClient, configuration);

            Console.WriteLine("\nPress Enter key to start Authorization API call demo with auto credentials otherwise any key for custom ...");
            var keyInfo = Console.ReadKey();

            // Demo Authorization API login
            var loginResponse = keyInfo.Key == ConsoleKey.Enter
                ? await AuthApiLoginAsync(
                    authApiClient,
                    configuration["TestCredentials:Username"]!,
                    configuration["TestCredentials:Password"]!)
                : await AuthApiLoginInteractiveAsync(authApiClient);

            // Demo secure API call if login was successful
            if (loginResponse != null)
            {
                await GetWeatherForecastAsync(httpClient, configuration, loginResponse.Token);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static async Task<WeatherForecastDto[]?> GetWeatherForecastAsync(
            HttpClient httpClient, 
            IConfiguration configuration, 
            string jwtToken)
        {
            Console.WriteLine("\n=== Weather Forecast API Call ===");
            
            var weatherForecastApiClient = new WeatherForecastApiClient(httpClient, configuration);
            var (success, weatherForecasts, errorMessage) = await weatherForecastApiClient.GetWeatherForecastAsync(jwtToken);
            
            if (success && weatherForecasts != null)
            {
                Console.WriteLine("✅ Weather forecast retrieved successfully!");
                Console.WriteLine($"   Found {weatherForecasts.Length} forecast(s):");
                
                foreach (var forecast in weatherForecasts)
                {
                    Console.WriteLine($"   📅 {forecast.Date}: {forecast.TemperatureC}°C ({forecast.TemperatureF}°F) - {forecast.Summary}");
                }
            }
            else
            {
                Console.WriteLine($"❌ Failed to retrieve weather forecast: {errorMessage}");
            }

            return weatherForecasts;
        }

        private static async Task<LoginResponseDto?> AuthApiLoginAsync(
            AuthApiClient authClient,
            string username,
            string password)
        {
            Console.WriteLine($"\nAttempting login with username: {username}");
                
            var (success, loginResponse, errorMessage) = await authClient.LoginAsync(username, password);
            if (success && loginResponse != null)
            {
                Console.WriteLine("✅ Login successful!");
                Console.WriteLine($"   User ID: {loginResponse.User?.ID}");
                Console.WriteLine($"   Name: {loginResponse.User?.Name}");
                Console.WriteLine($"   Username/Email: {loginResponse.User?.Email}");
                Console.WriteLine($"   Phone: {loginResponse.User?.PhoneNumber}");
                Console.WriteLine($"   Token: {loginResponse.Token}");
            }
            else
            {
                Console.WriteLine($"❌ Login failed: {errorMessage}");
            }

            return loginResponse;
        }

        private static async Task<LoginResponseDto?> AuthApiLoginInteractiveAsync(AuthApiClient authClient)
        {
            Console.WriteLine("\n=== Interactive Login ===");
            
            Console.Write("Enter username (email): ");
            var username = Console.ReadLine() ?? "";
            
            Console.Write("Enter password: ");
            var password = ReadPassword();
            
            Console.WriteLine("\nAttempting login...");

            return await AuthApiLoginAsync(authClient, username, password);
        }

        private static string ReadPassword()
        {
            var password = "";
            ConsoleKeyInfo keyInfo;
            
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);
            
            return password;
        }
    }
}