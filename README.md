# URL Shortener

A modern, high-performance URL shortening service built with **.NET 10** and **ASP.NET Core**.

This application allows users to generate short URLs for long links and seamlessly redirect users to the original destination. The project demonstrates clean architecture principles, REST API development, database integration, and scalable backend design.

## Features

* Create short URLs from long URLs
* Automatic redirection to original URLs
* Custom short codes support
* URL validation
* Click tracking and analytics
* RESTful API endpoints
* Database persistence
* Clean and maintainable architecture
* Built with .NET 10 and ASP.NET Core

## Technology Stack

* .NET 10
* ASP.NET Core
* Entity Framework Core
* SQL Server / PostgreSQL
* Minimal APIs / Controllers
* Swagger / OpenAPI
* Dependency Injection
* Logging and Exception Handling

## Architecture

The project follows modern software engineering practices:

* API Layer
* Application Layer
* Domain Layer
* Infrastructure Layer
* Database Layer

This separation of concerns improves maintainability, testability, and scalability.

## API Endpoints

### Create Short URL

POST `/api/urls`

Request:

```json
{
  "originalUrl": "https://example.com/very/long/url"
}
```

Response:

```json
{
  "shortCode": "Ab12Cd",
  "shortUrl": "https://yourdomain.com/Ab12Cd"
}
```

### Redirect

GET `/{shortCode}`

Automatically redirects to the original URL.

### Get URL Details

GET `/api/urls/{shortCode}`

Returns URL information and analytics.

## Getting Started

### Prerequisites

* .NET 10 SDK
* SQL Server or PostgreSQL
* Visual Studio 2022 / Rider / VS Code

### Installation

Clone the repository:

```bash
git clone https://github.com/yourusername/url-shortener.git
```

Navigate to the project:

```bash
cd url-shortener
```

Restore dependencies:

```bash
dotnet restore
```

Update database:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

Open Swagger UI:

```text
https://localhost:5001/swagger
```

## Future Enhancements

* User authentication and authorization
* QR code generation
* URL expiration dates
* Rate limiting
* Redis caching
* Docker support
* Kubernetes deployment
* Detailed analytics dashboard

## Learning Objectives

This project demonstrates:

* Backend API development
* Database design
* Entity Framework Core
* Clean Architecture
* REST API best practices
* Dependency Injection
* Production-ready application design

## Author

Nikhil Chauhan

Senior Software Developer | .NET Developer

GitHub: https://github.com/nikhilchauhan0134
