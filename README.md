# AuthDemo

Authentication \& Authorization demos

# Auth Demo Console Application



This console application demonstrates how to call the Api.WeatherForecast Get secure-endpoint using HttpClient.

The Api.Auth is used to authenticate users and obtain JWT tokens.



\## Usage



\### Running the Application

1\. Ensure that APIs are running before begin work with the console application.

2\. Use Api.Auth endpoint register to create a new user test credentials.

3\. Run the console application to authenticate the user and then fetch secure weather forecast data.



\### Test Credentials



The application includes test credentials automate login in app.Settings:TestCredentials section:



\## Used APIs Endpoints



The application calls the following endpoint:

\- `POST /api/auth/login` - Authenticates user and returns JWT token

\- `GET /api/weateherforecast` - get secure weather forecast data using JWT token



\## Features

\- \*\*.NET API\*\*: Uses .NET APIs as micro-services for authentication / consumption

\- \*\*HttpClient Integration\*\*: Uses HttpClient to make HTTP requests to the authentication API

\- \*\*JSON Serialization\*\*: Handles JSON serialization/deserialization for request/response objects  

\- \*\*Error Handling\*\*: Comprehensive error handling for network issues, timeouts, and API errors

\- \*\*Configurable Base URL\*\*: API base URL can be configured in the `AuthApiClient` constructor

\- \*\*Demo Mode\*\*: Includes predefined test credentials for demonstration

\- \*\*Interactive Mode\*\*: Optional interactive login (uncomment in `Program.cs`)

