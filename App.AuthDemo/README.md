# Auth Demo Console Application

This console application demonstrates how to call the Api.Auth login endpoint using HttpClient.

## Features

- **HttpClient Integration**: Uses HttpClient to make HTTP requests to the authentication API
- **JSON Serialization**: Handles JSON serialization/deserialization for request/response objects  
- **Error Handling**: Comprehensive error handling for network issues, timeouts, and API errors
- **Configurable Base URL**: API base URL can be configured in the `AuthApiClient` constructor
- **Demo Mode**: Includes predefined test credentials for demonstration
- **Interactive Mode**: Optional interactive login (uncomment in `Program.cs`)

## Project Structure

```
App.AuthDemo/
├── Models/
│   ├── LoginRequestDto.cs     # Login request data structure
│   ├── LoginResponseDto.cs    # Login response data structure  
│   ├── UserDto.cs             # User information structure
│   └── ResponseDto.cs         # Generic API response wrapper
├── Services/
│   └── AuthApiClient.cs       # HTTP client service for auth API calls
├── Program.cs                 # Main application entry point
└── appsettings.json          # Configuration file
```

## Usage

### Running the Application

1. Ensure the `Api.Auth` project is running (typically on `https://localhost:7777`)
2. Run the console application:
   ```bash
   dotnet run
   ```

### Test Credentials

The application includes test credentials for demonstration:
- `admin@example.com` / `Admin123!`
- `user@example.com` / `User123!`
- `invalid@example.com` / `wrongpassword` (for testing error handling)

### Configuring the API URL

You can change the API base URL by:
1. Modifying the `AuthApiClient` constructor call in `Program.cs`
2. Or updating the `appsettings.json` file (if configuration is enabled)

## API Endpoints

The application calls the following endpoint:
- `POST /api/auth/login` - Authenticates user and returns JWT token

## Dependencies

- .NET 8.0
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json
- System.Text.Json (built-in)

## Example Output

```
=== Auth Demo Application ===
This application demonstrates calling the Api.Auth login endpoint.

API Base URL: https://localhost:7777

=== Login Demo ===

Attempting login with username: admin@example.com
✅ Login successful!
   User ID: 12345
   Name: Admin User
   Email: admin@example.com
   Phone: +1234567890
   Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

Attempting login with username: invalid@example.com
❌ Login failed: Username or password is incorrect
```

## Notes

- Make sure the Api.Auth service is running before running this application
- The application includes SSL certificate validation - ensure your API has a valid certificate or configure to ignore SSL errors for development
- JWT tokens are displayed truncated for security purposes