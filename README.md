# Tamweely HR Management System

Tamweely is a full-stack HR management application built with **.NET 8 (Clean Architecture)** on the backend and **Angular 17** on the frontend. It provides features for managing employees, departments, job titles, and authentication with role-based access control.

---

## Table of Contents

* [Features](#features)
* [Technology Stack](#technology-stack)
* [Architecture](#architecture)
* [Getting Started](#getting-started)
* [Backend Structure](#backend-structure)
* [Frontend Structure](#frontend-structure)
* [Authentication & Authorization](#authentication--authorization)
* [API Endpoints](#api-endpoints)
* [Contributing](#contributing)

---

## Features

* Employee CRUD operations
* Department and Job Title management
* Role-based authentication and authorization
* JWT-based authentication
* Dynamic validation using FluentValidation
* Excel export of employee data
* Middleware for global exception handling
* Signal-based reactive authentication in Angular
* Search and filtering with pagination

---

## Technology Stack

**Backend:**

* .NET 8
* C#
* Entity Framework Core
* SQL Server
* JWT Authentication

**Frontend:**

* Angular 17
* TypeScript
* Signals for reactive state
* PrimeNG components
* RxJS

---

## Architecture

The backend follows **Clean Architecture** principles:

* **Application:** DTOs, Features (use cases), Validators, Interfaces
* **Domain:** Entities, Common logic
* **Infrastructure:** Database persistence, services, JWT generation, repository implementations
* **WebAPI:** Controllers, Middlewares

The frontend is modular:

* **Core:** Guards, Interceptors, Services
* **Features:** Auth, Employees, Departments, Job Titles
* **Shared:** Models, utilities, and navbar

---

## Getting Started

### Backend

1. Clone the repository:

```bash
git clone <repo-url>
cd Tamweely
```

2. Update `appsettings.json` with your SQL Server connection string and JWT secret:

```json
"ConnectionStrings": {
    "DefaultConnection": "<your_connection_string>"
},
"JwtOptions": {
    "Secret": "<your_secret_key>",
    "Issuer": "Tamweely",
    "Audience": "TamweelyUsers",
    "ExpireDays": 7
}
```

3. Apply EF Core migrations:

```bash
cd Tamweely.Infrastructure
dotnet ef database update
```

4. Run the WebAPI:

```bash
cd Tamweely.WebAPI
dotnet run
```

---

### Frontend

1. Install dependencies:

```bash
cd FrontEnd
npm install
```

2. Update API URL in `app.config.ts` if needed:

```ts
export const API_URL = 'https://localhost:7005';
```

3. Run the Angular app:

```bash
ng serve
```

4. Open in browser: `http://localhost:4200`

---

## Backend Structure

```
Tamweely.Application
 ├─ DTOs
 ├─ Features
 ├─ Interfaces
 ├─ Mapping
 └─ Validators

Tamweely.Domain
 ├─ Entities
 └─ Common

Tamweely.Infrastructure
 ├─ Persistence
 ├─ Repositories
 ├─ Services
 └─ Migrations

Tamweely.WebAPI
 ├─ Controllers
 ├─ Middlewares
 └─ Program.cs
```

---

## Frontend Structure

```
src/app
 ├─ core
 │   ├─ guards
 │   ├─ interceptors
 │   └─ services
 ├─ features
 │   ├─ auth
 │   ├─ employees
 │   ├─ department
 │   └─ jobTitle
 └─ shared
     ├─ models
     ├─ navbar
     └─ utils
```

---

## Authentication & Authorization

* JWT-based authentication with role claims
* Angular signals are used to update UI reactively on login/logout
* Roles determine visibility of buttons and navigation elements
* Backend middleware handles exceptions globally, including unique constraint violations

---

## API Endpoints

**Employees**

* `GET /api/employees` – Get list of employees with search, filter, pagination
* `GET /api/employees/{id}` – Get single employee
* `POST /api/employees` – Create employee
* `PUT /api/employees/{id}` – Update employee
* `DELETE /api/employees/{id}` – Soft delete employee
* `GET /api/employees/export` – Export employees to Excel

**Auth**

* `POST /api/auth/login` – Login and receive JWT

---

## Contributing

1. Fork the repository
2. Create a new branch
3. Implement features or fixes
4. Submit a pull request
