# Banking App – System Architecture

## 1. Architecture Overview
The Banking App uses a layered architecture based on ASP.NET Core MVC.

## 2. Architecture Layers

### Presentation Layer
- ASP.NET Core MVC
- Razor Views

### Application Layer
- Controllers
- ViewModels

### Domain Layer
- Business logic
- Entities (Account, Transaction, User)

### Data Access Layer
- Entity Framework Core
- SQL Server Database

## 3. Data Flow
1. User interacts with UI
2. Request sent to Controller
3. Business logic executed
4. Database accessed via DbContext
5. Response returned to UI

## 4. Security Architecture
- Authentication using ASP.NET Identity
- Role-based authorization
- HTTPS communication

## 5. Deployment Architecture
- Web application hosted on IIS
- SQL Server database
