# CoffeeShop API - N-Tier Architecture

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework-0078D4?style=for-the-badge&logo=dotnet&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)

## Overview
CoffeeShop API is a robust and scalable RESTful API built for managing coffee shop operations. Developed with **.NET Core** and **C#**, this project demonstrates a strong understanding of backend development fundamentals, clean code practices, and system design.

The system is strictly structured using **N-Tier Architecture**, ensuring clear separation of concerns, easy maintainability, and readiness for future scaling or testing.

##  Key Features
* ** N-Tier Architecture:** Clean separation into API (Presentation), BLL (Business Logic Layer), and DAL (Data Access Layer).
* ** Authentication & Authorization:** Secure JWT-based authentication with Role-based access control (RBAC).
* ** Security Enhancements:** Integrated Rate Limiting to prevent Brute-force attacks and robust Exception Handling.
* ** Database Management:** Implemented Entity Framework Core (Code-First approach) with **PostgreSQL**.
* ** Dependency Injection:** Extensively used DI for loose coupling across Repositories and Services.
* ** Business Logic:** Features order processing, user management, and inventory logic using custom DTOs.

##  Tech Stack
* **Framework:** .NET (C#)
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core
* **Authentication:** JSON Web Token (JWT)
* **API Documentation:** Swagger / OpenAPI

## Project Structure
The solution is divided into three main layers to strictly follow the Dependency Inversion Principle:

```text
CoffeeShop.Solution/
│
├── Frontend
│   └── CoffeeShop.FrontEnd/ # UI layer with portals for Admin, Manager, and Staff
│
├── Backend (N-Tier)
│   ├── CoffeeShop.API/      # Presentation Layer (Controllers, DI Container, Middlewares)
│   ├── CoffeeShop.BLL/      # Business Logic Layer (Services, DTOs, JWT Generation)
│   ├── CoffeeShop.DAL/      # Data Access Layer (Repositories, DbContext, Migrations)
│   └── CoffeeShop.Models/   # Shared Domain Layer (Entities: Auth, Catalog, Sales, System)
│
└── Testing
    └── CoffeeShop.Tests/    # Unit Tests targeting Business Services (Auth, Order)

Getting Started
Prerequisites
 + .NET SDK installed.
 + Node.js & npm installed for the frontend.
 + PostgreSQL running on your local machine.

Setup Backend
1. Configure Database: Update the DefaultConnection string in CoffeeShop.API/appsettings.json.
2. Apply Migrations:
cd CoffeeShop.API
dotnet ef database update --project ../CoffeeShop.DAL
3. Run the API & Tests:
dotnet run --project CoffeeShop.API
dotnet test ../CoffeeShop.Tests
4. Setup Frontend:
cd CoffeeShop.FrontEnd
npm install
npm run dev

Author
Dương Tiến Chiến * Backend Developer

GitHub: @ChinChin2k5
