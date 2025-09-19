# E-StoreX

### Enterprise-Grade E-Commerce Backend API

E-StoreX is a **full-featured e-commerce Backend API** built with **ASP.NET Core Web API** and designed using **Clean Architecture** principles.  
The project is structured for **scalability, maintainability, and long-term evolution**, while strictly applying **SOLID principles** and **clean separation of concerns** across layers.

---

## 🔹 Overview

E-StoreX provides a robust backend platform for an e-commerce application that supports **multiple roles** (User, Admin, SuperAdmin) with **secure authentication and authorization**.  
It covers the complete e-commerce workflow, starting from **product management, browsing, and cart handling**, to **checkout, payments, notifications, background jobs, and order lifecycle tracking**.

---

## 🔹 Key Design Goals

This project was developed with a strong focus on:

### Architecture

- Clean Architecture with strict separation (Core, Infrastructure, API layers)
- Enterprise-level design patterns like **Generic Repository** and **Unit of Work**

### Data & Database

- EF Core **Code First approach** with migrations
- Strong domain-driven design influences in the Core layer

### Security

- Authentication & Authorization with **Identity + JWT**
- OAuth integration with **Google** and **GitHub**
- Role-based access control (User, Admin, SuperAdmin)

### Quality & Testing

- High test coverage through **unit and integration testing**
- Test stack includes **xUnit, Moq, FluentAssertions, AutoFixture, Coverlet**

### Documentation

- Interactive API documentation with **Swagger**
- User-friendly developer docs with **Redoc**
- API versioning with **v1, v2 live** and **v3 in progress**

### Monitoring & Logging

- Advanced **structured logging** with correlation IDs
- Centralized error handling & diagnostics
- Unified API response structure across controllers

---

## Features in Detail

### Product & Catalog

- Product creation, editing, deletion, and querying
- Category management with hierarchical support
- **Fuzzy search** for smarter and more user-friendly queries
- Filtering, sorting, and pagination of products
- Image and media file handling

### Cart & Checkout

- Cart persistence per user
- Add, update, and remove items from cart
- Quantity adjustment and discount application
- Checkout process with order creation and payment integration

### Orders

- Order placement, tracking, and history
- Order status updates (Pending, Paid, Shipped, Completed, Cancelled)
- Automatic email notifications on each state change
- Integration with Stripe for secure payment handling

### Authentication & Authorization

- User registration and login with ASP.NET Identity
- JWT-based authentication for APIs
- Role-based access (User, Admin, SuperAdmin)
- External login support via Google and GitHub OAuth
- Fine-grained access policies to secure admin endpoints

### Payments & Notifications

- Stripe integration for payment processing
- MailKit/MimeKit for transactional emails (order confirmation, password reset, etc.)
- Configurable notification templates

### Background Jobs

- **Hangfire** for background job scheduling and processing
- Supports delayed jobs, recurring tasks, and retries
- Used for sending emails, cleaning up old data, and scheduled maintenance

### Infrastructure & Architecture

- Clean separation into Core, Infrastructure, and API layers
- Generic Repository and Unit of Work for data access
- AutoMapper for DTO and entity mapping
- Redis caching for performance optimization
- EF Core Code First with migrations for database management

### Logging & Monitoring

- Structured logging with full request/response tracking
- Error handling middleware with unified API responses
- Log correlation IDs for request tracing
- Centralized logging configuration

---

## Technology Stack

- **Backend / APIs:** ASP.NET Core Web API, EF Core (SQL Server), AutoMapper
- **Architecture:** Clean Architecture (Core, Infrastructure, API)
- **Authentication & Authorization:** Identity, JWT, OAuth (Google & GitHub)
- **Caching & Search:** Redis (StackExchange.Redis), Fuzzy search implementation
- **Background Jobs:** Hangfire
- **Payments & Notifications:** Stripe.NET, MailKit, MimeKit
- **File & Reports:** EPPlus (Excel), iText7 (PDFs)
- **Documentation:** Swagger, Redoc
- **Logging & Monitoring:** Structured logging with correlation IDs
- **Testing:** xUnit, Moq, FluentAssertions, AutoFixture, Coverlet

---

## Documentation

- Swagger UI: [https://estorex.runasp.net/swagger/index.html](https://estorex.runasp.net/swagger/index.html)
- Redoc: [https://estorex.runasp.net/index.html](https://estorex.runasp.net/index.html)
- ERD: [Mermaid Chart](https://www.mermaidchart.com/app/projects/52696ff8-27e3-4df1-8322-4a77f3dbaf70/diagrams/e5581ebd-d802-4949-82d3-1bac34013ee6/version/v0.1/edit)

---

## Repository

- GitHub: [https://github.com/Ibrahim-Hassan74/EStoreX](https://github.com/Ibrahim-Hassan74/EStoreX)

---

## API Access

Access is restricted. Contact me to request an API key for testing and integration.

---

## About

E-StoreX demonstrates how to build a **production-ready, enterprise-grade backend** using modern best practices.  
It goes far beyond basic CRUD operations by addressing the real challenges faced in large-scale systems and implementing advanced concepts such as:

---

### Layered Architecture with Strict Boundaries

Every concern is isolated:

- **Core** → contains the business logic
- **Infrastructure** → manages external dependencies (database, file storage, email, caching)
- **API** → handles the request/response pipeline

This separation makes the system highly maintainable and easy to evolve.

---

### Asynchronous Programming with async/await

All I/O operations (database, caching, email, payments) are implemented asynchronously.  
This ensures scalability under high load, allowing the system to handle thousands of concurrent requests without blocking threads.

---

### Dependency Injection Throughout the Solution

The project fully embraces DI to achieve loose coupling and testability.  
Every service, repository, and external integration is registered and resolved through the built-in ASP.NET Core container, making the system flexible and easy to extend.

---

### Centralized Error Handling and Response Standardization

A global exception middleware captures all unhandled errors and converts them into a **unified response format**.  
Clients always receive consistent error messages and HTTP status codes, improving **Developer Experience (DX)** and reducing ambiguity in integrations.

---

### Unified API Response Structure

Regardless of the controller or endpoint, responses follow a standardized format that includes:

- Data
- Metadata
- Error information

This consistency simplifies client integration (web, mobile, or third-party apps).

---

### Robust Validation Layer

DTOs and requests are validated using **Data Annotations** (e.g., `[Required]`, `[MaxLength]`, `[EmailAddress]`).  
This ensures data integrity and prevents invalid operations early in the request pipeline, before the data reaches the domain logic.

---

### Strong Test Coverage for Reliability and Maintainability

Both unit tests and integration tests cover critical paths such as:

- Authentication flows
- Order lifecycle
- Payment operations

Mocking frameworks and automated test data generation make it possible to validate logic without depending on external services.

---

### API Versioning and Backward Compatibility

Versioned endpoints (**v1**, **v2 live**, **v3 in development**) allow the system to evolve without breaking existing clients.  
New features are introduced progressively while keeping legacy integrations functional.

---

### Scalability Considerations Built-In

Caching (Redis), async programming, and structured logging ensure that the system can **scale horizontally and vertically** with minimal changes.  
Adding new services or extending existing ones does not affect other layers.

---

### Security First Approach

Authentication and authorization are built on **Identity and JWT**, with external **OAuth providers** integrated.  
Role-based access ensures that sensitive endpoints (like product management or order processing) are only available to authorized roles.

---

## Conclusion

This philosophy ensures **E-StoreX** is not just a “project demo,” but a **blueprint** for building clean, scalable, and enterprise-ready applications.
