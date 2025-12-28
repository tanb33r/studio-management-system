# Studio Booking Management - Backend Architecture

## Architecture Diagram

```mermaid
graph TB
    subgraph "Frontend Layer"
        AngularApp["Angular Frontend<br/>Components & Services"]
    end

    subgraph "WebApi Layer"
        AuthController["AuthController<br/>Register/Login"]
        StudiosController["StudiosController<br/>CRUD Operations"]
        BookingsController["BookingsController<br/>Booking Management"]
    end

    subgraph "Application Layer"
        AuthService["AuthService<br/>User Authentication"]
        StudioService["StudioService<br/>Studio Management"]
        BookingService["BookingService<br/>Booking Logic"]
        JwtService["JwtService<br/>Token Management"]
    end

    subgraph "Domain Layer"
        IUserRepo["IUserRepository"]
        IStudioRepo["IStudioRepository"]
        IBookingRepo["IBookingRepository"]
        User["User Entity"]
        Studio["Studio Entity"]
        Booking["Booking Entity"]
    end

    subgraph "Infrastructure Layer"
        UserRepo["UserRepository<br/>EF Core Implementation"]
        StudioRepo["StudioRepository<br/>EF Core Implementation"]
        BookingRepo["BookingRepository<br/>EF Core Implementation"]
        DbContext["StudioBookingManagementDbContext<br/>Entity Framework"]
    end

    subgraph "Database Layer"
        SqlServer[("SQL Server<br/>Database")]
    end

    %% Frontend to API connections
    AngularApp -.->|HTTP| AuthController
    AngularApp -.->|HTTP| StudiosController
    AngularApp -.->|HTTP| BookingsController

    %% API to Application connections
    AuthController --> AuthService
    StudiosController --> StudioService
    BookingsController --> BookingService

    %% Application to Domain connections
    AuthService --> IUserRepo
    AuthService --> JwtService
    StudioService --> IStudioRepo
    BookingService --> IBookingRepo

    %% Domain entities
    IUserRepo -.-> User
    IStudioRepo -.-> Studio
    IBookingRepo -.-> Booking

    %% Infrastructure implementations
    IUserRepo --> UserRepo
    IStudioRepo --> StudioRepo
    IBookingRepo --> BookingRepo

    %% Infrastructure to Database
    UserRepo --> DbContext
    StudioRepo --> DbContext
    BookingRepo --> DbContext
    DbContext --> SqlServer

    %% Styling for dark theme
    classDef frontend fill:#2d3748,stroke:#4a5568,stroke-width:2px,color:#e2e8f0
    classDef webapi fill:#2b2d42,stroke:#3c3f58,stroke-width:2px,color:#e2e8f0
    classDef application fill:#1a202c,stroke:#2d3748,stroke-width:2px,color:#e2e8f0
    classDef domain fill:#171923,stroke:#2d3748,stroke-width:2px,color:#e2e8f0
    classDef infrastructure fill:#0f1419,stroke:#2d3748,stroke-width:2px,color:#e2e8f0
    classDef database fill:#0d1117,stroke:#21262d,stroke-width:3px,color:#f0f6fc

    class AngularApp frontend
    class AuthController,StudiosController,BookingsController webapi
    class AuthService,StudioService,BookingService,JwtService application
    class IUserRepo,IStudioRepo,IBookingRepo,User,Studio,Booking domain
    class UserRepo,StudioRepo,BookingRepo,DbContext infrastructure
    class SqlServer database
```

## 🏗️ Architecture Overview

Your Studio Booking Management system follows a **Clean Architecture** pattern with clear separation of concerns across four main layers.

### 📋 Architecture Layers

#### 1. **WebApi Layer (Presentation)**
- **Controllers**: `AuthController`, `StudiosController`, `BookingsController`
- **Configuration**: JWT authentication, CORS setup, dependency injection
- **Middleware**: Authentication, authorization, and cross-origin handling

#### 2. **Application Layer (Use Cases)**
- **Services**: Business logic implementations (`AuthService`, `StudioService`, `BookingService`)
- **Interfaces**: Service contracts defining business operations
- **DTOs**: Data transfer objects for API communication
- **JWT Service Interface**: Token management abstraction

#### 3. **Domain Layer (Business Logic)**
- **Entities**: Core business objects (`User`, `Studio`, `Booking`) with business rules
- **Value Objects**: `Email` with validation logic
- **Repository Interfaces**: Data access contracts
- **Base Entity**: Common properties (Id, timestamps)

#### 4. **Infrastructure Layer (Data & External Services)**
- **Repository Implementations**: Data access logic with EF Core
- **DbContext**: Entity Framework configuration and mappings
- **JWT Service**: Token generation and validation
- **Migrations**: Database schema evolution

### 🔄 Key Architectural Benefits

1. **Dependency Inversion**: Dependencies flow inward toward the domain
2. **Testability**: Each layer can be unit tested independently
3. **Maintainability**: Clear separation makes changes easier
4. **Scalability**: Layers can be scaled independently
5. **Technology Independence**: Business logic is isolated from frameworks

### 🗄️ Database Design

- **Users Table**: Authentication and user profile data
- **Studios Table**: Studio information with location and pricing
- **Bookings Table**: Booking records with time slots and status tracking

### 🔐 Security Features

- JWT-based authentication
- Password hashing with BCrypt
- Email validation through value objects
- Role-based authorization setup

### 📊 Project Structure

```
StudioBookingManagement/
├── StudioBookingManagement.WebApi/          # Presentation Layer
│   ├── Controllers/                         # API Controllers
│   ├── Program.cs                          # App configuration
│   └── appsettings.json                    # Configuration
├── StudioBookingManagement.Application/     # Application Layer
│   ├── Services/                           # Business services
│   ├── Interfaces/                         # Service contracts
│   └── DTOs/                              # Data transfer objects
├── StudioBookingManagement.Domain/          # Domain Layer
│   ├── Entities/                           # Core business entities
│   ├── Repositories/                       # Repository interfaces
│   ├── ValueObjects/                       # Value objects
│   └── Common/                            # Shared domain logic
└── StudioBookingManagement.Infrastructure/  # Infrastructure Layer
    ├── Repositories/                       # Repository implementations
    ├── Data/                              # DbContext
    ├── Services/                          # External services
    └── Migrations/                        # Database migrations
```

This architecture ensures your application is maintainable, testable, and follows SOLID principles while providing a robust foundation for the studio booking management system.