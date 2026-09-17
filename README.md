# 🛒 E-Commerce API

A production-oriented **E-Commerce REST API** built with **ASP.NET Core 8**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

The project implements **CQRS**, **MediatR**, **Entity Framework Core**, **ASP.NET Core Identity**, **JWT Authentication**, **Redis**, **Repository & Specification Patterns**, **Domain Events**, the **Outbox Pattern**, and **Paymob payment integration**.

The goal of this project is to demonstrate how a real-world E-Commerce backend can be designed using clean separation of responsibilities, domain-driven business rules, and scalable backend patterns rather than building a traditional CRUD API.

---

# 📑 Table of Contents

* [Project Overview](#-project-overview)
* [What Does the System Do?](#-what-does-the-system-do)
* [Main User Journey](#-main-user-journey)
* [Features](#-features)
* [Architecture](#-architecture)
* [Project Structure](#-project-structure)
* [Layer Responsibilities](#-layer-responsibilities)
* [Domain Model](#-domain-model)
* [Aggregates and Business Rules](#-aggregates-and-business-rules)
* [CQRS](#-cqrs)
* [Repository Pattern](#-repository-pattern)
* [Specification Pattern](#-specification-pattern)
* [Validation Pipeline](#-validation-pipeline)
* [Authentication & Authorization](#-authentication--authorization)
* [Basket Flow](#-basket-flow)
* [Checkout Flow](#-checkout-flow)
* [Cash Payment Flow](#-cash-payment-flow)
* [Visa / Paymob Flow](#-visa--paymob-flow)
* [Paymob Webhook](#-paymob-webhook)
* [Outbox Pattern](#-outbox-pattern)
* [Database](#-database)
* [Redis](#-redis)
* [Error Handling](#-error-handling)
* [Configuration](#-configuration)
* [Prerequisites](#-prerequisites)
* [Installation](#-installation)
* [Database Migration](#-database-migration)
* [Running the Application](#-running-the-application)
* [Swagger](#-swagger)
* [API Endpoints](#-api-endpoints)
* [Complete API Usage Example](#-complete-api-usage-example)
* [Testing](#-testing)
* [Security Notes](#-security-notes)
* [Known Improvements](#-known-improvements)
* [Future Improvements](#-future-improvements)
* [Author](#-author)

---

# 📖 Project Overview

This project is the backend of an E-Commerce platform.

It provides the main backend operations required by an online store:

```text
Authentication
      ↓
Product Catalog
      ↓
Shopping Basket
      ↓
Checkout
      ↓
Order
      ↓
Payment
      ↓
Order Processing
      ↓
Shipping
      ↓
Delivery
```

Customers can browse products, manage their baskets, create orders, and pay using Cash or Visa.

Administrators can manage the catalog and process customer orders.

The project also demonstrates backend architecture patterns that are commonly used in larger applications.

---

# 🧠 What Does the System Do?

The system contains two main types of users.

## 👤 Customer

A customer can:

* Register
* Login
* Browse products
* Search products
* Filter products
* Sort products
* View product details
* Add products to a basket
* Update basket quantities
* Remove basket items
* Clear the basket
* Checkout
* Select Cash or Visa payment
* View their orders
* View order details

---

## 👨‍💼 Administrator

An administrator can:

* Create products
* Update products
* Deactivate products
* Manage categories
* Manage brands
* View/manage orders
* Start processing orders
* Ship orders
* Deliver orders
* Cancel orders where the domain rules allow cancellation

---

# 🔄 Main User Journey

A typical customer journey looks like this:

```text
                    Customer
                       │
                       ▼
                  Registration
                       │
                       ▼
                     Login
                       │
                       ▼
                Receive JWT Token
                       │
                       ▼
                Browse Products
                       │
                       ▼
                 Add To Basket
                       │
                       ▼
                  View Basket
                       │
                       ▼
                    Checkout
                       │
              ┌────────┴────────┐
              │                 │
             Cash             Visa
              │                 │
              ▼                 ▼
        Confirm Order        Paymob
                                │
                                ▼
                       Payment Checkout
                                │
                                ▼
                         Paymob Webhook
                                │
                                ▼
                       Verify HMAC
                                │
                        ┌───────┴───────┐
                        │               │
                     Success         Failure
                        │               │
                        ▼               ▼
                  Paid Payment     Failed Payment
                        │
                        ▼
                  Confirm Order
```

---

# ✨ Features

## 🔐 Authentication

* User registration
* User login
* JWT authentication
* ASP.NET Core Identity
* Password hashing
* Role-based authorization
* Admin role
* Customer role

---

## 📦 Product Catalog

* Create products
* Update products
* Deactivate products
* Get product by ID
* Get paginated products
* Search products
* Filter by category
* Filter by brand
* Filter by price
* Sort products
* Product stock management

---

## 🗂️ Categories

* Create category
* Update category
* Deactivate category
* Search categories
* Pagination

---

## 🏷️ Brands

* Create brand
* Update brand
* Deactivate brand
* Search brands
* Pagination

---

## 🛒 Basket

* Add item
* Update item quantity
* Remove item
* Clear basket
* Get current user's basket
* Validate product availability

---

## 📦 Orders

* Checkout
* Create order from basket
* Create order items
* Copy product price into order item
* Stock validation
* Stock deduction
* Order state management
* Customer order history
* Order details
* Order cancellation
* Start processing
* Shipping
* Delivery

---

## 💳 Payments

* Cash payment
* Visa payment
* Paymob integration
* Paymob Unified Checkout
* Payment status management
* Paymob webhook
* HMAC verification
* Payment success/failure handling

---

## ⚙️ Infrastructure

* Entity Framework Core
* SQL Server
* Redis
* ASP.NET Core Identity
* JWT
* Repository Pattern
* Specification Pattern
* Outbox Pattern
* Background Service

---

# 🏗️ Architecture

The project follows **Clean Architecture**.

The actual project dependency structure is:

```text
                         ┌────────────────────┐
                         │        API         │
                         │                    │
                         │ Controllers        │
                         │ Middleware         │
                         │ Composition Root   │
                         └─────────┬──────────┘
                                   │
                    ┌──────────────┴──────────────┐
                    │                             │
                    ▼                             ▼
          ┌──────────────────┐          ┌──────────────────┐
          │   Application    │          │ Infrastructure   │
          │                  │          │                  │
          │ CQRS             │          │ EF Core          │
          │ MediatR          │          │ SQL Server       │
          │ Commands         │          │ Redis            │
          │ Queries          │          │ Identity         │
          │ Handlers         │          │ Repositories     │
          │ DTOs             │          │ Paymob           │
          │ Validators       │          │ Background Jobs  │
          └────────┬─────────┘          └────────┬─────────┘
                   │                             │
                   │                             │
                   └──────────────┬──────────────┘
                                  ▼
                         ┌──────────────────┐
                         │      Domain      │
                         │                  │
                         │ Entities         │
                         │ Aggregates       │
                         │ Value Objects    │
                         │ Domain Events    │
                         │ Business Rules   │
                         └──────────────────┘
```

## Dependency Direction

```text
API
 ├── Application
 └── Infrastructure

Application
 └── Domain

Infrastructure
 ├── Application
 └── Domain
```

The API references Infrastructure because the API is the **composition root** where Infrastructure services are registered.

For example:

```csharp
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

---

# 📁 Project Structure

```text
ECommerce.Solution
│
├── ECommerce
│   │
│   ├── Controllers
│   │   ├── AuthenticationController.cs
│   │   ├── ProductsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── BrandsController.cs
│   │   ├── BasketsController.cs
│   │   ├── OrdersController.cs
│   │   └── PaymentController.cs
│   │
│   ├── Exceptions
│   │   └── GlobalExceptionHandler.cs
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── Properties
│
├── ECommerce.Application
│   │
│   ├── Abstractions
│   │   ├── Repositories
│   │   ├── Services
│   │   └── Specifications
│   │
│   ├── Behaviors
│   │   └── ValidationBehavior.cs
│   │
│   ├── Common
│   │   └── PagedResult.cs
│   │
│   └── Features
│       ├── Authentication
│       ├── Baskets
│       ├── Brands
│       ├── Categories
│       ├── Orders
│       ├── Payments
│       └── Products
│
├── ECommerce.Domain
│   │
│   ├── Common
│   ├── Entities
│   ├── Enums
│   ├── Events
│   ├── Exceptions
│   ├── IRepositories
│   └── ValueObjects
│
├── ECommerce.Infrastructure
│   │
│   ├── BackgroundServices
│   ├── Identity
│   ├── Migrations
│   ├── Persistence
│   │   ├── Configurations
│   │   ├── Interceptors
│   │   └── Specifications
│   │
│   ├── ReadRepositories
│   ├── Repositories
│   └── Services
│
└── ECommerce.Solution.sln
```

---

# 🧩 Layer Responsibilities

## 1. Domain

The Domain is the core of the application.

It contains:

* Entities
* Aggregates
* Value Objects
* Domain Events
* Enums
* Business Rules
* Domain Exceptions
* Repository abstractions

The Domain should contain the business rules rather than database or HTTP concerns.

Example:

```csharp
order.Confirm();
order.StartProcessing();
order.Ship();
order.Deliver();
```

The domain decides whether each transition is valid.

---

# 2. Application

The Application layer contains the application's use cases.

Examples:

```text
Create Product
Update Product
Get Products
Add Item To Basket
Checkout
Cancel Order
Ship Order
Login
Register
```

It contains:

* Commands
* Queries
* Handlers
* DTOs
* Validators
* MediatR behaviors
* Application abstractions

The Application layer coordinates the use case without containing infrastructure implementation details.

---

# 3. Infrastructure

Infrastructure contains implementations that communicate with external systems.

Examples:

```text
Entity Framework Core
SQL Server
ASP.NET Core Identity
Redis
Paymob
Repositories
Read Repositories
Background Services
```

Infrastructure implements abstractions required by Application and Domain.

---

# 4. API

The API layer is responsible for HTTP communication.

It contains:

* Controllers
* HTTP configuration
* Dependency Injection composition
* Middleware
* Global exception handling
* Swagger configuration
* Authentication configuration

Controllers are intentionally thin.

The controller sends commands/queries through MediatR instead of implementing business logic itself.

---

# 🧠 Domain Model

The main domain entities are:

```text
ApplicationUser

Category
Brand
Product

Basket
 └── BasketItem

Order
 ├── OrderItem
 └── Payment

OutboxMessage
```

Relationships:

```text
Category
    │
    └──────────< Product >────────── Brand


User
 │
 ├──────────── Basket
 │                  │
 │                  └────< BasketItem >──── Product
 │
 └────────────< Order
                     │
                     ├────< OrderItem >──── Product
                     │
                     └──────── Payment
```

---

# 💰 Product & Money

Products have a price represented using the `Money` Value Object.

```text
Money
 ├── Amount
 └── Currency
```

Instead of treating price as just a primitive value, the Value Object keeps monetary information together.

Example concept:

```text
Amount: 1500
Currency: EGP
```

---

# 🛒 Basket Aggregate

The Basket contains BasketItems.

```text
Basket
 └── BasketItem
       ├── ProductId
       └── Quantity
```

Basket operations are controlled by the aggregate:

```csharp
basket.AddItem(...);
basket.RemoveItem(...);
basket.UpdateItemQuantity(...);
basket.Clear();
```

The basket belongs to the authenticated user.

---

# 📦 Order Aggregate

An Order contains OrderItems.

```text
Order
 ├── Id
 ├── UserId
 ├── Status
 └── OrderItems
```

Each OrderItem contains:

```text
ProductId
Quantity
UnitPrice
```

The product price is copied into the OrderItem during checkout.

This is important because product prices can change later.

Example:

```text
Product price:
1000 EGP

Customer checks out
       ↓
OrderItem.UnitPrice = 1000 EGP

Product price later:
1200 EGP

Existing order:
1000 EGP
```

The historical order price is therefore preserved.

---

# 🔄 Order Lifecycle

The Order has controlled state transitions.

Normal lifecycle:

```text
Pending
   │
   ▼
Confirmed
   │
   ▼
Processing
   │
   ▼
Shipped
   │
   ▼
Delivered
```

Cancellation is possible only when allowed by the domain rules.

Conceptually:

```text
Pending ────────► Cancelled
Confirmed ──────► Cancelled
Processing ─────► Cancelled
Shipped ────────► Cancelled
```

A delivered order cannot be cancelled.

The important point is that the Application layer does not simply assign:

```csharp
order.Status = ...
```

Instead, domain methods enforce the business rules:

```csharp
order.Confirm();
order.StartProcessing();
order.Ship();
order.Deliver();
order.Cancel();
```

---

# 💳 Payment Model

A Payment belongs to an Order.

```text
Order
 │
 └──── Payment
```

Payment methods:

```text
Cash
Visa
```

Payment status represents the payment lifecycle.

The initial payment state is:

```text
Pending
```

and it is updated when the payment result becomes known.

---

# 🔀 CQRS

The project follows CQRS:

> Command Query Responsibility Segregation

Commands change application state.

Queries retrieve information.

---

## Commands

Examples:

```text
CreateProductCommand

UpdateCategoryCommand

```

---

## Queries

Examples:

```text
GetProductsQuery
GetProductByIdQuery
```

MediatR dispatches commands and queries to their corresponding handlers.

---

# 🔎 Repository Pattern

The project abstracts persistence behind repositories.

Examples:

```text
IProductRepository
```

Infrastructure provides their implementations using Entity Framework Core.

This keeps persistence details out of the Application use cases.

---

# 📖 Read Repositories

The project separates read operations from write operations.

Read repositories are optimized for retrieving DTOs.

Examples:

```text
ProductReadRepository
```
---

# 🔎 Specification Pattern

The Specification Pattern is used to encapsulate query conditions.

Product queries can support:

```text
Search
Category
Brand
Minimum Price
Maximum Price
Sorting
Pagination
Active Status
```

Example:

```http
GET /api/Products?Search=phone&MinPrice=1000&MaxPrice=10000
```

This keeps filtering and sorting logic out of controllers.

---

# 📄 Pagination

Collection endpoints support pagination.

Typical parameters:

```text
PageNumber
PageSize
```

Example:

```http
GET /api/Products?PageNumber=1&PageSize=10
```

The result contains pagination information such as:

```text
Items
PageNumber
PageSize
TotalCount
TotalPages
```

---

# ✅ Validation Pipeline

The project uses FluentValidation with a MediatR Pipeline Behavior.

Request flow:

```text
HTTP Request
     │
     ▼
Controller
     │
     ▼
MediatR
     │
     ▼
ValidationBehavior
     │
     ▼
Validator
     │
     ▼
Command / Query Handler
```

This prevents validation logic from being duplicated inside controllers.

---

# 🔐 Authentication & Authorization

Authentication uses:

```text
ASP.NET Core Identity
+
JWT Bearer Authentication
```

The application supports:

```text
Customer
Admin
```

After login, the API returns authentication information containing a JWT.

Protected requests must include:

```http
Authorization: Bearer YOUR_TOKEN
```

Administrative operations require the appropriate role.

---

# 🛒 Basket Flow

The basket belongs to the currently authenticated user.

The client does not need to send a user ID.

The backend obtains the current user from the authenticated JWT.

Example:

```text
JWT
 │
 ▼
Current User ID
 │
 ▼
Find Basket
 │
 ▼
Add / Remove / Update Items
```

When adding an item, the application validates the product and quantity.

---

# 🛍️ Checkout Flow

Checkout is the most important business workflow.

The general flow is:

```text
Customer
   │
   ▼
Checkout
   │
   ▼
Get Current Basket
   │
   ▼
Validate Basket
   │
   ├── Empty? ───────► Error
   │
   ├── Product missing? ─► Error
   │
   └── Stock insufficient? ─► Error
   │
   ▼
Create Order
   │
   ▼
Create Order Items
   │
   ▼
Copy Product Prices
   │
   ▼
Decrease Stock
   │
   ▼
Create Payment
   │
   ▼
Cash / Visa
```

The checkout operation is performed using a database transaction so that related database changes can be committed or rolled back together.

---

# 💵 Cash Payment Flow

For Cash:

```text
Checkout
   │
   ▼
Create Order
   │
   ▼
Create Order Items
   │
   ▼
Decrease Stock
   │
   ▼
Create Payment
   │
   ▼
Confirm Order
   │
   ▼
Clear Basket
```

No external payment provider is required.

---

# 💳 Visa / Paymob Flow

Visa checkout uses Paymob.

The flow is:

```text
Client
  │
  ▼
Checkout API
  │
  ▼
Create Order
  │
  ▼
Create Order Items
  │
  ▼
Decrease Stock
  │
  ▼
Create Pending Payment
  │
  ▼
Paymob Intention API
  │
  ▼
Client Secret
  │
  ▼
Unified Checkout URL
  │
  ▼
Return URL
  │
  ▼
Client Opens Paymob
```

The backend does not require the frontend to handle payment credentials.

---

# 🔗 Paymob Checkout URL

After creating the Paymob intention, the application receives a client secret.

The backend creates the Unified Checkout URL using the configured Paymob public key and client secret.

The frontend receives the URL.

Example response concept:

```json
{
  "orderId": "ORDER_GUID",
  "paymentId": "PAYMENT_GUID",
  "paymentUrl": "https://accept.paymob.com/unifiedcheckout/..."
}
```

The frontend should redirect the user to `paymentUrl`.

---

# 🔔 Paymob Webhook

The frontend must **not** be trusted to determine whether payment succeeded.

Paymob sends the payment result to the backend through the webhook endpoint.

```text
Paymob
   │
   ▼
POST /api/Payment/webhook
   │
   ▼
Receive Transaction
   │
   ▼
Verify HMAC
   │
   ▼
Find Order
   │
   ▼
Find Payment
   │
   ├───────────────┐
   │               │
 Success          Failure
   │               │
   ▼               ▼
Paid            Failed
   │
   ▼
Confirm Order
```

---

# 🔐 HMAC Verification

The webhook is protected using Paymob HMAC verification.

The backend:

1. Receives Paymob's webhook data.
2. Builds the expected HMAC payload.
3. Calculates HMAC-SHA512.
4. Compares the calculated value with the received HMAC.
5. Rejects invalid requests.
6. Processes only verified notifications.

A constant-time comparison is used to reduce timing-attack risks.

---

# 📬 Outbox Pattern

The application implements the Outbox Pattern for reliable domain event processing.

Instead of immediately depending on an external operation when a domain event occurs, the event can be persisted as an Outbox Message.

Conceptually:

```text
Domain Entity
      │
      ▼
Domain Event
      │
      ▼
SaveChanges Interceptor
      │
      ▼
OutboxMessages Table
      │
      ▼
Background Service
      │
      ▼
Publish Event
```

The important benefit is that the domain event is stored as part of the database operation.

---

# ⚙️ Outbox Background Service

A hosted background service periodically checks for unprocessed messages.

The processing flow is:

```text
OutboxMessages
      │
      ▼
Load Unprocessed Messages
      │
      ▼
Resolve Event Type
      │
      ▼
Deserialize Event
      │
      ▼
Publish Event
      │
      ▼
Mark As Processed
```

This separates event persistence from event processing.

---

# 🗄️ Database

The project uses:

```text
Microsoft SQL Server
+
Entity Framework Core
```

Main application entities include:

```text
Products
Categories
Brands

Baskets
BasketItems

Orders
OrderItems

Payments

OutboxMessages
```

ASP.NET Core Identity also creates the required identity tables.

---

# 🔗 Database Relationships

Simplified relationship model:

```text
Category 1 ──────── * Product

Brand 1 ─────────── * Product

Basket 1 ────────── * BasketItem

Product 1 ───────── * BasketItem

Order 1 ─────────── * OrderItem

Product 1 ───────── * OrderItem

Order 1 ─────────── 1 Payment

User 1 ───────────── 1 Basket

User 1 ───────────── * Order
```

---

# 🧱 EF Core Configuration

Entity configurations are separated from the Domain entities.

Examples include configurations for:

```text
Product
Category
```

This keeps database-specific configuration inside Infrastructure.

---

# 🆔 IDs

Business entities use `Guid` identifiers.

Examples:

```text
Product.Id
Category.Id
```

IDs are generated by the application/domain rather than waiting for SQL Server to generate them.

This is useful when identifiers are required before persistence, including domain events and external integrations.

---

# ⚡ Redis

Redis is integrated into the Infrastructure layer.

The project uses a cache abstraction:

```text
ICacheService
```

with Redis as the infrastructure implementation.

Configuration:

```json
{
  "Redis": {
    "ConnectionString": "YOUR_REDIS_CONNECTION"
  }
}
```

For local development, a typical Redis endpoint is:

```text
127.0.0.1:6379
```

---

# ⚠️ Error Handling

The API uses centralized exception handling.

```text
Controller
    │
    ▼
Application
    │
    ▼
Exception
    │
    ▼
Global Exception Handler
    │
    ▼
ProblemDetails
    │
    ▼
HTTP Response
```

This avoids repeating try/catch blocks inside every controller.

The system handles application/domain errors such as:

```text
Resource not found
Invalid business operation
Invalid order transition
Invalid payment state
Insufficient stock
Invalid basket
Validation failures
Authentication failures
External payment failures
Invalid Paymob webhook
```

---

# ⚙️ Configuration

The repository contains an `appsettings.json` template.

Example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=EcommerceDDD;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "Jwt": {
    "Key": "YOUR_JWT_SECRET_KEY",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "ExpirationInMinutes": 15
  },

  "Redis": {
    "ConnectionString": "YOUR_REDIS_CONNECTION"
  },

  "Paymob": {
    "SecretKey": "YOUR_PAYMOB_SECRET_KEY",
    "PublicKey": "YOUR_PAYMOB_PUBLIC_KEY",
    "HmacSecret": "YOUR_PAYMOB_HMAC_SECRET",
    "IntegrationId": "YOUR_PAYMOB_INTEGRATION_ID",
    "notification_url": "YOUR_PAYMOB_NOTIFICATION_URL"
  },

  "AllowedHosts": "*"
}
```

These are placeholders.

They must be replaced with real local/development configuration.

---

# 🔐 Recommended Configuration Method

For local development, use **.NET User Secrets** instead of putting secrets directly into `appsettings.json`.

From the API project:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
```

```bash
dotnet user-secrets set "Jwt:Key" "YOUR_JWT_SECRET"
```

```bash
dotnet user-secrets set "Redis:ConnectionString" "YOUR_REDIS_CONNECTION"
```

Paymob:

```bash
dotnet user-secrets set "Paymob:SecretKey" "YOUR_PAYMOB_SECRET"
```

```bash
dotnet user-secrets set "Paymob:PublicKey" "YOUR_PAYMOB_PUBLIC_KEY"
```

```bash
dotnet user-secrets set "Paymob:HmacSecret" "YOUR_PAYMOB_HMAC_SECRET"
```

```bash
dotnet user-secrets set "Paymob:IntegrationId" "YOUR_PAYMOB_INTEGRATION_ID"
```

```bash
dotnet user-secrets set "Paymob:notification_url" "YOUR_PUBLIC_WEBHOOK_URL"
```

---

# 💻 Prerequisites

Before running the project, install:

## .NET 8 SDK

Verify:

```bash
dotnet --version
```

The project targets:

```text
net8.0
```

---

## SQL Server

You can use:

* SQL Server Developer
* SQL Server Express
* SQL Server LocalDB

---

## Redis

Redis should be available for the configured Redis connection.

Default local example:

```text
127.0.0.1:6379
```

---

## Git

Verify:

```bash
git --version
```

---

## Paymob

Paymob is required only if you want to test Visa payments.

You can run and test the rest of the application without configuring an online payment account if you only use Cash payment.

---

# 📥 Installation

Clone the repository:

```bash
git clone https://github.com/Sheref17/ECommerce.Solution.git
```

Move into the project:

```bash
cd ECommerce.Solution
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

---

# 🗄️ Database Migration

The project contains EF Core migrations under:

```text
ECommerce.Infrastructure/Migrations
```

Install Entity Framework CLI if necessary:

```bash
dotnet tool install --global dotnet-ef
```

Apply migrations:

```bash
dotnet ef database update \
  --project ECommerce.Infrastructure \
  --startup-project ECommerce
```

This creates/updates the configured SQL Server database.

---

# 🆕 Creating a New Migration

After modifying the database model:

```bash
dotnet ef migrations add MigrationName \
  --project ECommerce.Infrastructure \
  --startup-project ECommerce
```

Then apply it:

```bash
dotnet ef database update \
  --project ECommerce.Infrastructure \
  --startup-project ECommerce
```

---

# ▶️ Running the Application

From the solution root:

```bash
dotnet run --project ECommerce
```

Or:

```bash
cd ECommerce
dotnet run
```

The application will display its listening URLs in the terminal.

---

# 🌐 API Endpoints

The API contains the following main controllers:

```text
/api/Authentication
/api/Products
/api/Categories
/api/Brands
/api/Baskets
/api/Orders
/api/Payment
```

---

# 🔐 Authentication API

## Register

```http
POST /api/Authentication/register
```

Example:

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password@123"
}
```

---

## Login

```http
POST /api/Authentication/login
```

Example:

```json
{
  "email": "john@example.com",
  "password": "Password@123"
}
```

The response provides the JWT authentication information.

---

# 📦 Products API

Base route:

```text
/api/Products
```

## Get Products

```http
GET /api/Products
```

Example:

```http
GET /api/Products?Search=phone&MinPrice=1000&MaxPrice=10000&PageNumber=1&PageSize=10
```

---

## Get Product

```http
GET /api/Products/{id}
```

---

## Create Product

Admin only:

```http
POST /api/Products
```

Example:

```json
{
  "name": "iPhone 15",
  "description": "Apple smartphone",
  "price": 50000,
  "currency": "EGP",
  "stock": 20,
  "categoryId": "CATEGORY_GUID",
  "brandId": "BRAND_GUID"
}
```

---

## Update Product

Admin only:

```http
PUT /api/Products/{id}
```

---

## Delete Product

Admin only:

```http
DELETE /api/Products/{id}
```

The application uses deactivation/soft-delete behavior rather than simply exposing physical deletion as a business operation.

---

# 🗂️ Categories API

Base route:

```text
/api/Categories
```

```http
GET /api/Categories
```

```http
GET /api/Categories/{id}
```

Admin operations:

```http
POST /api/Categories
PUT /api/Categories/{id}
DELETE /api/Categories/{id}
```

---

# 🏷️ Brands API

Base route:

```text
/api/Brands
```

```http
GET /api/Brands
```

```http
GET /api/Brands/{id}
```

Admin operations:

```http
POST /api/Brands
PUT /api/Brands/{id}
DELETE /api/Brands/{id}
```

---

# 🛒 Basket API

Basket operations require authentication.

## Get Basket

```http
GET /api/Baskets/basket
```

---

## Add Item

```http
POST /api/Baskets/items
```

Example:

```json
{
  "productId": "PRODUCT_GUID",
  "quantity": 2
}
```

---

## Update Quantity

```http
PUT /api/Baskets/items
```

Example:

```json
{
  "productId": "PRODUCT_GUID",
  "quantity": 3
}
```

---

## Remove Item

```http
DELETE /api/Baskets/items
```

Example:

```json
{
  "productId": "PRODUCT_GUID"
}
```

---

## Clear Basket

```http
DELETE /api/Baskets
```

---

# 📦 Orders API

Customer endpoints require authentication.

## Checkout

```http
POST /api/Orders/checkout
```

Example:

```json
{
  "paymentMethod": 1,
  "phoneNumber": "01000000000",
  "apartment": "10",
  "floor": "3",
  "building": "25",
  "street": "Main Street",
  "postalCode": "11835",
  "city": "Cairo",
  "state": "Cairo",
  "country": "EG"
}
```

Payment methods:

```text
1 = Cash
2 = Visa
```

---

## My Orders

```http
GET /api/Orders/myOrders
```

Returns orders belonging to the authenticated customer.

---

## Order Details

```http
GET /api/Orders/{id}
```

---

# 👨‍💼 Admin Order Management

Administrative order operations require the Admin role.

## Start Processing

```http
PUT /api/Orders/{id}/start-processing
```

```text
Confirmed → Processing
```

---

## Ship

```http
PUT /api/Orders/{id}/ship
```

```text
Processing → Shipped
```

---

## Deliver

```http
PUT /api/Orders/{id}/deliver
```

```text
Shipped → Delivered
```

---

## Cancel

```http
PUT /api/Orders/{id}/cancel
```

Cancellation is validated by the domain.

---

# 🔔 Payment Webhook API

Paymob sends payment notifications to:

```http
POST /api/Payment/webhook
```

The endpoint:

```text
Receive Paymob Notification
          ↓
Verify HMAC
          ↓
Find Order
          ↓
Find Payment
          ↓
Update Payment
          ↓
Update Order
```

---

# 🌍 Local Paymob Webhook

Paymob cannot normally access:

```text
localhost
```

directly.

For local development, expose the API through a public HTTPS tunnel such as ngrok.

Example:

```text
https://YOUR-NGROK-DOMAIN/api/Payment/webhook
```

Configure this URL as:

```text
Paymob:notification_url
```

---

# 🧪 Complete API Usage Example

A complete customer scenario:

## Step 1 — Register

```http
POST /api/Authentication/register
```

---

## Step 2 — Login

```http
POST /api/Authentication/login
```

Save the returned JWT.

---

## Step 3 — Authorize

```text
Bearer YOUR_JWT_TOKEN
```

---

## Step 4 — Browse Products

```http
GET /api/Products
```

---

## Step 5 — Add Product

```http
POST /api/Baskets/items
```

```json
{
  "productId": "PRODUCT_GUID",
  "quantity": 2
}
```

---

## Step 6 — View Basket

```http
GET /api/Baskets/basket
```

---

## Step 7 — Checkout With Cash

```http
POST /api/Orders/checkout
```

```json
{
  "paymentMethod": 1,
  "phoneNumber": "01000000000",
  "apartment": "10",
  "floor": "3",
  "building": "25",
  "street": "Main Street",
  "postalCode": "11835",
  "city": "Cairo",
  "state": "Cairo",
  "country": "EG"
}
```

---

## Step 8 — Order Created

The system:

```text
Validates Basket
       ↓
Validates Stock
       ↓
Creates Order
       ↓
Creates Order Items
       ↓
Copies Product Prices
       ↓
Decreases Stock
       ↓
Creates Payment
       ↓
Confirms Order
       ↓
Clears Basket
```

---

# 💳 Complete Visa Scenario

For Visa:

```json
{
  "paymentMethod": 2,
  "phoneNumber": "01000000000",
  "apartment": "10",
  "floor": "3",
  "building": "25",
  "street": "Main Street",
  "postalCode": "11835",
  "city": "Cairo",
  "state": "Cairo",
  "country": "EG"
}
```

The backend:

```text
Create Order
      ↓
Create Order Items
      ↓
Decrease Stock
      ↓
Create Pending Payment
      ↓
Call Paymob
      ↓
Get Client Secret
      ↓
Generate Checkout URL
      ↓
Return Payment URL
```

The client opens the returned URL.

After payment:

```text
Paymob
   ↓
Webhook
   ↓
HMAC Verification
   ↓
Payment Result
   ↓
Update Payment
   ↓
Update Order
```

---

# 🧪 Testing

The project can be tested through Swagger.

Recommended business rules for automated tests include:

```text
Product stock cannot become negative.

Basket cannot accept invalid quantities.

Checkout cannot proceed with an empty basket.

Checkout cannot proceed when stock is insufficient.

Order cannot transition to an invalid state.

Delivered orders cannot be cancelled.

Payment state transitions must be valid.

Invalid Paymob webhook signatures must be rejected.
```

A dedicated unit/integration test project can be added to automate these scenarios.

---

# 🔒 Security Notes

Never commit real secrets to GitHub.

The repository's `appsettings.json` should contain placeholders only:

```text
YOUR_SQL_SERVER
YOUR_JWT_SECRET_KEY
YOUR_REDIS_CONNECTION
YOUR_PAYMOB_SECRET_KEY
YOUR_PAYMOB_PUBLIC_KEY
YOUR_PAYMOB_HMAC_SECRET
YOUR_PAYMOB_INTEGRATION_ID
YOUR_PAYMOB_NOTIFICATION_URL
```

Use:

```text
.NET User Secrets
```

for local development.

For production:

```text
Environment Variables
```

or a dedicated secret-management solution should be used.

---

# ⚠️ Important Development Notes

## Paymob

Visa payments require:

* Paymob account
* Paymob API credentials
* Integration ID
* HMAC secret
* Public webhook URL

Cash checkout does not require Paymob.

---

## Redis

Redis must be reachable using the configured:

```text
Redis:ConnectionString
```

---

## SQL Server

The connection string must point to a SQL Server instance accessible from the development machine.

Example:

```text
Server=YOUR_SQL_SERVER;
Database=EcommerceDDD;
Trusted_Connection=True;
TrustServerCertificate=True;
```

---

# 🛠️ Known Improvements

The current project can be improved further in several areas.

### 1. Automated Tests

Add:

```text
Unit Tests
Integration Tests
API Tests
```

especially around checkout, order state transitions, payments, and stock.

### 2. Payment Transaction Boundary

External Paymob HTTP calls should ideally be separated from long-running database transactions.

A more resilient architecture would be:

```text
Create Order
   ↓
Create Pending Payment
   ↓
Commit Database
   ↓
Call Paymob
   ↓
Return Checkout URL
```

This avoids keeping a database transaction open while waiting for an external service.

### 3. Stock Reservation

For online payments, a future version could introduce temporary stock reservations:

```text
Stock Available
      ↓
Reserve Stock
      ↓
Payment Pending
      ↓
 ┌────┴────┐
 ▼         ▼
Paid     Expired
 │         │
 ▼         ▼
Confirm   Release
Order     Stock
```

### 4. Payment Idempotency

Payment processing can be further strengthened by making webhook handling explicitly idempotent.

This ensures that receiving the same Paymob notification more than once does not apply the business operation twice.

---

# 🚀 Future Improvements

Potential future additions:

## Authentication

* Refresh Tokens
* Password Reset
* Email Verification
* Token Revocation

## Payments

* Payment expiration
* Payment retry
* Refunds
* Idempotency keys
* Payment transaction records

## Orders

* Stock reservations
* Order expiration
* Shipping address entity
* Order history pagination

## Infrastructure

* Docker
* CI/CD
* Health Checks
* Serilog
* Monitoring
* Distributed Tracing

## Performance

* Redis caching
* Cache invalidation
* Database indexes
* Query optimization

## Testing

* Unit Testing
* Integration Testing
* End-to-End API Testing

---

# 📌 Architectural Summary

The main architectural idea of the project is:

```text
API
 │
 ├── Handles HTTP
 │
 ▼
Application
 │
 ├── Executes Use Cases
 ├── Commands
 ├── Queries
 ├── Validation
 └── MediatR
 │
 ▼
Domain
 │
 ├── Business Rules
 ├── Entities
 ├── Aggregates
 ├── Value Objects
 └── Domain Events

Infrastructure
 │
 ├── EF Core
 ├── SQL Server
 ├── Identity
 ├── Redis
 ├── Paymob
 ├── Repositories
 └── Background Services
```

The architecture separates:

```text
Business Logic
      ≠
Application Logic
      ≠
Infrastructure
      ≠
HTTP
```

This makes the system easier to maintain, test, and extend.

---

# 🎯 What This Project Demonstrates

This project goes beyond a traditional CRUD API.

It demonstrates practical backend concepts including:

```text
ASP.NET Core 8
C#
Clean Architecture
Domain-Driven Design
CQRS
MediatR
SOLID
Entities
Aggregates
Value Objects
Domain Events
Repository Pattern
Specification Pattern
EF Core
SQL Server
ASP.NET Core Identity
JWT Authentication
Role-Based Authorization
FluentValidation
MediatR Pipeline Behaviors
Global Exception Handling
ProblemDetails
Redis
External API Integration
Paymob
Payment Webhooks
HMAC Verification
Database Transactions
Outbox Pattern
Background Services
Pagination
Filtering
Sorting
```

---

# 👨‍💻 Author

**Sherif Gamal**

### GitHub

`https://github.com/Sheref17`

### LinkedIn

`www.linkedin.com/in/sherif-gamal-58b6bb334`

---

# 📄 License

This project is developed for educational, learning, and portfolio purposes.
