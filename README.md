<p align="center">
  <img src="https://raw.githubusercontent.com/Megjafari/kaizen-frontend/main/public/pwa-512x512.png" alt="Kaizen Logo" width="120" />
</p>

<h1 align="center">Kaizen API</h1>

<p align="center">
  <strong>A fitness tracking REST API built with ASP.NET Core</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET" />
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core" />
  <img src="https://img.shields.io/badge/Auth0-EB5424?style=for-the-badge&logo=auth0&logoColor=white" alt="Auth0" />
  <img src="https://img.shields.io/badge/Cloudinary-3448C5?style=for-the-badge&logo=cloudinary&logoColor=white" alt="Cloudinary" />
  <img src="https://img.shields.io/badge/Railway-0B0D0E?style=for-the-badge&logo=railway&logoColor=white" alt="Railway" />
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker" />
</p>

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Authentication](#authentication)
- [Deployment](#deployment)
- [Admin Access](#admin-access)
- [License](#license)

---

## Overview

Kaizen API is the backend service for the Kaizen fitness tracking application. It provides RESTful endpoints for managing user profiles, workout logs, food tracking, weight measurements, and progress photos. The API is designed with security, scalability, and clean architecture in mind.

**Live API:** [https://kaizen-api-production-ce18.up.railway.app](https://kaizen-api-production-ce18.up.railway.app)

**Live app:** [https://kaizen.meghdadjafari.dev](https://kaizen.meghdadjafari.dev)

**Frontend repo:** [https://github.com/Megjafari/kaizen-frontend](https://github.com/Megjafari/kaizen-frontend)

---

## Features

- **User Profile Management** - Create and update user profiles with fitness goals
- **Workout Tracking** - Log workouts with exercises, sets, reps, and weights
- **Workout Templates** - Pre-defined workout templates for quick logging
- **Food Logging** - Track daily food intake with macro calculations
- **Ingredient Database** - Searchable ingredient database with nutritional info
- **Weight Tracking** - Daily weight logging with progress tracking
- **Progress Photos** - Upload and manage transformation photos with Cloudinary
- **Weekly Summaries** - Aggregated weekly statistics
- **Admin Panel** - Manage users, ingredients, and templates
- **JWT Authentication** - Secure authentication via Auth0

---

## Tech Stack

| Category | Technology |
|----------|------------|
| **Framework** | ASP.NET Core 10.0 |
| **ORM** | Entity Framework Core |
| **Database** | PostgreSQL (Neon) |
| **Authentication** | Auth0 + JWT Bearer |
| **Image Storage** | Cloudinary |
| **Deployment** | Railway + Docker |
| **CI/CD** | GitHub Actions |

---

## Architecture

```
Kaizen.API/
├── Controllers/          # API endpoints
│   ├── ProfileController.cs
│   ├── WorkoutController.cs
│   ├── FoodController.cs
│   ├── WeightController.cs
│   ├── WeeklySummaryController.cs
│   ├── ProgressController.cs
│   └── AdminController.cs
├── Services/             # Business logic
│   ├── IProfileService.cs / ProfileService.cs
│   ├── IWorkoutService.cs / WorkoutService.cs
│   ├── IFoodService.cs / FoodService.cs
│   ├── IWeightService.cs / WeightService.cs
│   ├── IWeeklySummaryService.cs / WeeklySummaryService.cs
│   ├── IProgressService.cs / ProgressService.cs
│   └── IImageService.cs / ImageService.cs
├── Models/               # Domain entities
│   ├── UserProfile.cs
│   ├── WorkoutLog.cs
│   ├── WorkoutTemplate.cs
│   ├── ExerciseLog.cs
│   ├── TemplateExercise.cs
│   ├── FoodLog.cs
│   ├── Ingredient.cs
│   ├── WeightLog.cs
│   └── ProgressPhoto.cs
├── DTOs/                 # Data transfer objects
│   ├── CreateWorkoutLogDto.cs
│   ├── CreateFoodLogDto.cs
│   └── ImageUploadDto.cs
├── Data/                 # Database context and seeding
│   ├── KaizenDbContext.cs
│   └── SeedData.cs
├── Migrations/           # EF Core migrations
└── Program.cs            # Application entry point
```

---

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) or [Neon](https://neon.tech/) account
- [Auth0](https://auth0.com/) account
- [Cloudinary](https://cloudinary.com/) account

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Megjafari/kaizen-API.git
   cd kaizen-API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Set up the database**
   ```bash
   cd Kaizen.API
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5164`

### Configuration

Create `appsettings.Development.json` in the `Kaizen.API` folder:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-host;Database=kaizen;Username=your-user;Password=your-password;SSL Mode=Require"
  },
  "Auth0": {
    "Domain": "https://your-tenant.auth0.com",
    "Audience": "https://kaizen-api"
  },
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

---

## API Endpoints

### Profile

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Profile` | Get current user profile |
| `POST` | `/api/Profile` | Create user profile |
| `PUT` | `/api/Profile` | Update user profile |
| `DELETE` | `/api/Profile` | Delete user profile |
| `POST` | `/api/Profile/image` | Upload profile image |

### Workouts

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Workout/logs` | Get all workout logs |
| `GET` | `/api/Workout/logs?date={date}` | Get logs for specific date |
| `POST` | `/api/Workout/logs` | Create workout log |
| `DELETE` | `/api/Workout/logs/{id}` | Delete workout log |
| `GET` | `/api/Workout/templates` | Get workout templates |

### Food

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Food/logs?date={date}` | Get food logs for date |
| `POST` | `/api/Food/logs` | Create food log |
| `DELETE` | `/api/Food/logs/{id}` | Delete food log |
| `GET` | `/api/Food/ingredients?q={query}` | Search ingredients |
| `POST` | `/api/Food/ingredients` | Create ingredient |
| `GET` | `/api/Food/summary?date={date}` | Get daily nutrition summary |

### Weight

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Weight` | Get all weight logs |
| `POST` | `/api/Weight` | Create weight log |
| `PUT` | `/api/Weight/{id}` | Update weight log |
| `DELETE` | `/api/Weight/{id}` | Delete weight log |

### Progress Photos

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Progress` | Get all progress photos |
| `POST` | `/api/Progress` | Upload progress photo |
| `DELETE` | `/api/Progress/{id}` | Delete progress photo |

### Weekly Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/WeeklySummary` | Get current week summary |
| `GET` | `/api/WeeklySummary?date={date}` | Get summary for specific week |

### Admin

Requires `IsAdmin = true` in the database.

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Admin/check` | Check admin status |
| `GET` | `/api/Admin/users` | Get all users |
| `DELETE` | `/api/Admin/users/{id}` | Delete user |
| `GET` | `/api/Admin/ingredients` | Get all ingredients |
| `POST` | `/api/Admin/ingredients` | Create ingredient |
| `PUT` | `/api/Admin/ingredients/{id}` | Update ingredient |
| `DELETE` | `/api/Admin/ingredients/{id}` | Delete ingredient |
| `GET` | `/api/Admin/templates` | Get all templates |
| `POST` | `/api/Admin/templates` | Create template |
| `DELETE` | `/api/Admin/templates/{id}` | Delete template |

---

## Database Schema

```
UserProfiles
├── Id (PK)
├── UserId (Auth0 ID)
├── Height (decimal)
├── Weight (decimal)
├── Age (int)
├── Gender (string)
├── Goal (string)
├── ProfileImageUrl (string?)
└── IsAdmin (bool)

WorkoutLogs
├── Id (PK)
├── UserId
├── Date
├── Name
├── Notes
└── Exercises[] -> ExerciseLogs

ExerciseLogs
├── Id (PK)
├── WorkoutLogId (FK)
├── ExerciseName
├── Sets
├── Reps
└── Weight

WorkoutTemplates
├── Id (PK)
├── Name
├── Description
├── Level
└── Exercises[] -> TemplateExercises

FoodLogs
├── Id (PK)
├── UserId
├── Date
├── IngredientId (FK)
└── AmountGrams

Ingredients
├── Id (PK)
├── Name
├── Calories
├── Protein
├── Carbs
└── Fat

WeightLogs
├── Id (PK)
├── UserId
├── Date
└── Weight

ProgressPhotos
├── Id (PK)
├── UserId
├── Date
├── ImageUrl
├── Note
└── Weight (auto-filled from WeightLogs)
```

---

## Authentication

This API uses **Auth0** for authentication with JWT Bearer tokens.

### Setup

1. Create an Auth0 API with identifier `https://kaizen-api`
2. Configure the Auth0 domain in `appsettings.json`
3. All endpoints require a valid JWT token in the Authorization header:
   ```
   Authorization: Bearer <token>
   ```

### CORS

Allowed origins:
- `http://localhost:5173` (development)
- `https://kaizen-frontend-pi.vercel.app`
- `https://kaizen.meghdadjafari.dev`

---

## Deployment

### Railway (Production)

1. Connect your GitHub repository to Railway
2. Railway auto-detects the Dockerfile
3. Add environment variables:
   ```
   ConnectionStrings__DefaultConnection=<neon-connection-string>
   Auth0__Domain=https://your-tenant.auth0.com
   Auth0__Audience=https://kaizen-api
   Cloudinary__CloudName=<cloud-name>
   Cloudinary__ApiKey=<api-key>
   Cloudinary__ApiSecret=<api-secret>
   ```
4. Deploy

### Docker

```bash
docker build -t kaizen-api .
docker run -p 8080:8080 kaizen-api
```

---

## Admin Access

To grant admin access to a user:

1. Open your database console (Neon Console for production)
2. Find the user's ID:
   ```sql
   SELECT * FROM "UserProfiles";
   ```
3. Grant admin access:
   ```sql
   UPDATE "UserProfiles" SET "IsAdmin" = true WHERE "Id" = <user-id>;
   ```
4. Access admin panel at `/admin` in the frontend

---


<p align="center">
  Made by <a href="https://meghdadjafari.dev">Meg Jafari</a>
</p>
