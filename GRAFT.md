# Service Architecture Folder Structure

## Product Service (Onion Architecture)

```
src/
├── ProductService.API/
│ ├── Controllers/
│ ├── Program.cs
│ └── appsettings.json
├── ProductService.Application/
│ ├── Features/
│ │ ├── Products/
│ │ │ ├── Commands/
│ │ │ └── Queries/
│ │ └── Categories/
│ │ ├── Commands/
│ │ └── Queries/
│ ├── Interfaces/
│ ├── DTOs/
│ ├── Mappings/
│ └── Behaviors/
├── ProductService.Domain/
│ ├── Entities/
│ ├── ValueObjects/
│ ├── Enums/
│ └── Events/
└── ProductService.Infrastructure/
├── Persistence/
│ ├── Context/
│ └── Configurations/
├── Services/
└── Consumers/
src/
├── AuthService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
├── Program.cs
└── appsettings.json
src/
├── NotificationService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── EmailService/
│ └── SMSService/
├── Program.cs
└── appsettings.json
src/
├── PaymentService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── PaymentProviders/
│ └── TransactionServices/
├── Program.cs
└── appsettings.json
src/
├── ProfileService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
├── Program.cs
└── appsettings.json
src/
├── StockService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── InventoryService/
│ └── WarehouseService/
├── Program.cs
└── appsettings.json
src/
├── StorageService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── FileService/
│ └── MediaService/
├── Program.cs
└── appsettings.json
src/
├── OrderService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── OrderProcessing/
│ └── ReturnService/
├── Program.cs
└── appsettings.json
src/
├── AnalyticsService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── ReportingService/
│ └── MetricsService/
├── Program.cs
└── appsettings.json
src/
├── SearchService/
├── Controllers/
├── Context/
├── Entities/
├── DTOs/
├── ModelConfiguration/
├── MapProfiles/
├── Consumers/
├── Services/
│ ├── SearchEngine/
│ └── RecommendationService/
├── Program.cs
└── appsettings.json

```
## Common Folder Structure Explanation

For all services (except Product Service):
- **Controllers/**: API endpoints
- **Context/**: Database context and configurations
- **Entities/**: Domain models
- **DTOs/**: Data transfer objects
- **ModelConfiguration/**: Entity type configurations
- **MapProfiles/**: AutoMapper profiles
- **Consumers/**: MassTransit message consumers
- **Services/**: Business logic implementation

For Product Service (Onion Architecture):
- **API Layer**: Controllers and configuration
- **Application Layer**: Business logic, interfaces, DTOs
- **Domain Layer**: Core business entities and logic
- **Infrastructure Layer**: External concerns (database, messaging)