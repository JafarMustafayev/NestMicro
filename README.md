# NestMicro E-Commerce Platform

## Overview
NestMicro is an enterprise-grade e-commerce platform built with microservices architecture. The platform follows domain-driven design principles and implements a standardized approach to entity tracking and auditing through a shared BaseEntity structure.

## Technology Stack
- .NET 8
- C# 12.0
- Entity Framework Core 8.X.X
- MassTransit with RabbitMQ 8.2.5
- SQL Server
- AutoMapper 12.0.1
- Swagger/OpenAPI
- Docker & Kubernetes

## Core Features

### Base Entity Structure
All entities in the system inherit from BaseEntity which provides:
- Unique GUID-based identification
- Creation tracking (date and user)
- Modification tracking (date and user)
- Soft delete functionality
- Active status management
- Audit trail capabilities

### Microservices
- **Auth Service**: User authentication and authorization
- **Product Service**: Product and vendor management (Onion Architecture)
- **Profile Service**: User profile and preferences
- **Storage Service**: File and media management
- **Stock Service**: Inventory management
- **Payment Service**: Transaction processing
- **Notification Service**: Communication handling
- **Order Service**: Order processing
- **Analytics Service**: Business intelligence
- **Search Service**: Advanced search capabilities

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server
- RabbitMQ
- Docker Desktop
- Kubernetes (optional)

### Development Setup

1. **Clone the Repository**
```bash
git clone https://github.com/JafarMustafayev/NestMicro.git
cd NestMicro
```

2. **Restore Dependencies**
```bash
dotnet restore
```

3. **Configure Environment**
- Copy `appsettings.example.json` to `appsettings.Development.json`
- Update connection strings and configuration settings
- Set up user secrets for sensitive data:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string"
```

4. **Database Setup**
```bash
# For each service that requires a database:
cd src/ServiceName
dotnet ef database update
```

5. **Run with Docker**
```bash
docker-compose up --build
```

### Production Deployment

1. **Build Docker Images**
```bash
docker-compose -f docker-compose.prod.yml build
```

2. **Deploy to Kubernetes**
```bash
kubectl apply -f k8s/
```

## Project Structure

```
NestMicro/
├── src/
│   ├── Services/
│   │   ├── ProductService/    # Onion Architecture
│   │   │   ├── API/
│   │   │   ├── Application/
│   │   │   ├── Domain/
│   │   │   └── Infrastructure/
│   │   └── OtherServices/     # Standard Structure
│   │       ├── Controllers/
│   │       ├── Services/
│   │       ├── DTOs/
│   │       └── etc...
│   └── Shared/
│       └── Nest.Shared/
│           └── Entities/
│               └── BaseEntity.cs
├── tests/
├── docs/
│   ├── RFCs/
│   └── PRD/
└── docker/
```

## Development Guidelines

### Entity Creation
- All entities must inherit from BaseEntity
- Use the provided audit fields for tracking
- Implement soft delete using IsDelete flag
- Maintain active status using IsActive

### API Standards
- RESTful endpoints
- Proper HTTP status codes
- Comprehensive Swagger documentation
- API versioning
- DTOs for request/response

### Database
- Use EF Core with SQL Server
- Implement proper migrations
- Follow naming conventions
- Respect soft delete pattern

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

### Commit Convention
- feat: New feature
- fix: Bug fix
- docs: Documentation
- style: Formatting
- refactor: Code restructuring
- test: Adding tests
- chore: Maintenance

## License
This project is licensed under the MIT License. See LICENSE file for details.

## Support
For support, please open an issue in the GitHub repository or contact the development team.