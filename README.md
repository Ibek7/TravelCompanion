# TravelCompanion

TravelCompanion is a multi-project solution designed to provide a travel companion experience through a mobile application built using .NET MAUI. The solution includes API, database, SDK, and mobile app components, as well as unit tests.

## Projects

### 1. TravelCompanion.API
This project serves as the backend API for the TravelCompanion app. It is hosted in Azure and relies on Entity Framework Core for data access.

#### Requirements:
- **Environment Variables:**
  - `TravelCompanionAppInsights` - Application Insights connection string
  - `TravelCompanionContext` - SQL Database connection string
  - `AppleClientId`, `AppleKeyId`, `AppleTeamId` - For Apple OAuth
  - `TravelCompanionGoogleClientId`, `TravelCompanionGoogleClientSecret` - For Google OAuth

- **Apple OAuth Configuration:**
  - Add your `AuthKey_{keyId}.p8` file to the API publish directory for Apple OAuth functionality.

### 2. TravelCompanion.Database
This project is a SQL Database project that contains the database schema for the application. It includes tables and relationships required by the API and is built to run on Azure SQL Database.

### 3. TravelCompanion.Domain
The Domain project encapsulates the core business logic and models of the application. It includes:
- Models
- DTOs
- Shared extension methods
- Data access services

### 4. TravelCompanion.SDK
This project acts as an abstraction layer over the API, making it easier for clients, such as the MAUI app, to interact with the backend. It handles the `HttpClient` logic and provides clean methods for API calls without requiring the client to manage low-level details.

### 5. TravelCompanion.MAUI
This project is a .NET MAUI application that supports both Android and iOS platforms. It uses the TravelCompanion.SDK to communicate with the API and provides the front-end experience for users.

#### Configuration:
- **Constants.cs file:**
  - `ApiBaseAddress` - Base address for the hosted TravelCompanion.API
  - `OpenAI Api Key` - Key for OpenAI integration
  - `Default OpenAI Model` - The default model for OpenAI API usage
  - `Syncfusion License Key` - License key for Syncfusion controls

### 6. TravelCompanion.Tests
This project contains unit tests to ensure the reliability of both the SDK and Domain service methods.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Azure subscription (for hosting and database setup)
- Apple Developer Account (for iOS OAuth)
- Google Developer Account (for Google OAuth)
- Syncfusion License Key (for UI components)
- OpenAI API Key (for AI features)

### Setup

1. **API Hosting:**
   - Deploy the `TravelCompanion.API` project to an Azure App Service.
   - Ensure the required environment variables are set in the Azure App Service settings.

2. **Database Setup:**
   - Deploy the `TravelCompanion.Database` project to an Azure SQL Database.
   - Update the `TravelCompanionContext` environment variable with the connection string.

3. **MAUI App:**
   - Configure the `Constants.cs` file in the `TravelCompanion.MAUI` project with the necessary values (API base address, OpenAI API key, etc.).
   - Build and deploy the app to Android and iOS devices.

4. **SDK & Tests:**
   - Run the unit tests in the `TravelCompanion.Tests` project to ensure everything is functioning as expected.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue for any bugs or feature requests.

## License

This project is licensed under the MIT License.