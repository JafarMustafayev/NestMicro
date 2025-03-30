# NestMicro E-Commerce Platform

## Overview
NestMicro is an enterprise-grade e-commerce platform built using a microservices architecture. The platform is designed for scalability, maintainability, and high availability, leveraging domain-driven design (DDD) principles and event-driven communication.

## Technology Stack

- **Backend Framework:** .NET 8
- **Programming Language:** C# 12.0
- **ORM:** Entity Framework Core 8.0.8
- **Message Broker:** EventBus with RabbitMQ 8.2.5
- **Database:** SQL Server
- **Object Mapping:** AutoMapper 12.0.1 & Mapster
- **API Documentation:** Swagger/OpenAPI
- **Containerization & Orchestration:** Docker & Kubernetes
- **Service Discovery & Health Check:** Consul
- **Caching:** Redis

## Core Features

### Base Entity Structure
All entities inherit from a shared `BaseEntity` class, which provides:

- **GUID-based ID** for unique identification
- **Creation & Modification Tracking** (timestamps and user IDs)
- **Soft delete functionality** for logical data removal
- **Active status management**
- **Audit trail for entity changes**

### Microservices

1. **Auth Service** - Handles authentication, authorization, and token management.
2. **Product Service** - Manages products and vendors using Onion Architecture.
3. **Stock Service** - Maintains product inventory and stock updates.
4. **Storage Service** - Handles file and media uploads (product images, logos, profile pictures, etc.).
5. **Order Service** - Manages order placement, processing, and status tracking.
6. **Payment Service** - Handles transactions and payment processing.
7. **Notification Service** - Sends email notifications (future plans for SMS and push notifications).
8. **Report Service** - Generates analytics and user invoices.
9. **Review Service** - Manages user reviews and ratings (Planed).
10. **API Gateway** - Manages request routing, authentication, and health checks.

## Setup Instructions

### Prerequisites

- .NET 9 SDK
- SQL Server
- RabbitMQ
- Redis for cashing 
- Docker Desktop
- Kubernetes (optional)
- Consul for service discovery

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
    - Update database connection strings and service URLs
    - Configure user secrets for sensitive data:
      ```bash
      dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string"
      ```

4. **Database Setup**
   ```bash
   # Apply migrations for each service that requires a database
   cd Services/ServiceName
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

```plaintext
NestMicro/
├── BuildingBlocks/
│    └── EventBus/
│        ├── EventBus.Abstractions/
│        └── EventBus.RabbitMq/
│
├── Services/
│   ├── ProductService/    # Onion Architecture
│   │   ├── API/
│   │   ├── Application/
│   │   ├── Domain/
│   │   └── Infrastructure/
│   ├── StockService/
│   ├── OrderService/
│   ├── PaymentService/
│   ├── NotificationService/
│   ├── ReportService/
│   ├── ReviewService/
│   ├── AuthService/
│   ├── StorageService/
│   └──API Gateway/
│ 
├── Shared/
│   └── Nest.Shared/
│       ├── Entities/
│       │   └── BaseEntity.cs
│       ├── Exceptions/
│       ├── Utils/
│ 
└── Dockerfiles/
   ├── Consul/
   │   └── docker-compose.yml
   ├── RabbitMq
   │   └── docker-compose.yml
   ├── Redis
   │   └── docker-compose.yml
   ├──Dockerfile
```

## Development Guidelines

### Entity Creation

- All entities must inherit from `BaseEntity`.
- Use the provided audit fields for tracking changes.
- Implement soft delete using `IsDeleted` flag.
- Maintain active status using `IsActive` flag.

### API Standards

- Follow RESTful API principles.
- Use proper HTTP status codes.
- Provide comprehensive Swagger documentation.
- Implement API versioning.
- Use DTOs for request/response objects.

### Database

- Use Entity Framework Core with SQL Server.
- Implement proper database migrations.
- Follow consistent naming conventions.
- Implement soft delete patterns.

## Contributing

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push to your branch.
5. Create a Pull Request.

### Commit Convention

- **feat:** New feature implementation
- **fix:** Bug fixes
- **docs:** Documentation updates
- **style:** Code formatting and style improvements
- **refactor:** Code restructuring without functional changes
- **test:** Adding or updating tests
- **chore:** Maintenance tasks

## License

This project is licensed under the MIT License. See LICENSE file for details.

## Support

For support, open an issue in the GitHub repository or contact the development team.

