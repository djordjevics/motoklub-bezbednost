# Motoklub Bezbednost API

ASP.NET Core Web API for managing motorcycle club members, motorcycles, equipment, and training records.

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL database

## Setup

1. Update `appsettings.json` with your database connection string
2. Update AWS Cognito configuration in `appsettings.json`
3. Run database migrations (when implemented)

## Running the Application

```bash
dotnet restore
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`

## API Endpoints

- `/api/members` - Member CRUD operations
- `/api/motorcycles` - Motorcycle CRUD operations
- `/api/trainings` - Training sessions and records
- `/api/equipment` - Equipment CRUD operations

## Authentication

All endpoints require JWT authentication via AWS Cognito.

