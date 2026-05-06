# 🚀 HelpDesk Multi-Tenant Enterprise System

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue)
![Kubernetes](https://img.shields.io/badge/Kubernetes-Ready-blueviolet)
![Architecture](https://img.shields.io/badge/Architecture-DDD%20%7C%20Event--Driven-green)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

A complete **enterprise-grade HelpDesk platform** built with modern .NET technologies, cloud-native architecture, and distributed systems patterns.

---

## 🧠 Overview

This project demonstrates a **multi-tenant SaaS system** designed with:

- Clean Architecture (DDD + SOLID)
- Event-driven microservices
- Azure cloud integration
- Containerized infrastructure (Docker + Kubernetes)
- Multiple UI paradigms (MVC, MVVM, MVP)

---

## 🧪 Testing Strategy

The project includes **unit tests across all layers**, ensuring reliability and maintainability.

### 📁 Test Projects

tests/
├── HelpDesk.API.Tests
├── HelpDesk.Application.Tests
├── HelpDesk.Domain.Tests
├── HelpDesk.Infrastructure.Tests
├── HelpDesk.Admin.Web.Tests
├── HelpDesk.Desktop.Tests

### ✔ Coverage

- Domain (business rules)
- Application (use cases)
- API (controllers/endpoints)
- Infrastructure (mocked integrations)
- UI logic (basic scenarios)

---

## 🏗 Architecture

```
MVC / WPF / WinForms
        ↓
ASP.NET Core API (JWT / Multi-tenant)
        ↓
SQL Server + Service Bus + Event Hub
        ↓
Workers → Cosmos DB (Audit)
```

---

## ⚙️ Tech Stack

### Backend
- .NET 8 / ASP.NET Core
- Entity Framework Core
- Dapper / ADO.NET
- SQL Server

### Cloud
- Azure Service Bus
- Azure Event Hub
- Azure Cosmos DB

### DevOps
- Docker / Docker Compose
- Kubernetes (AKS-ready)

### Frontend
- ASP.NET Core MVC
- WPF (MVVM)
- WinForms (MVP)

---

## 🔄 System Flow

1. Ticket created via API/UI  
2. Stored in SQL Server  
3. Event sent to Service Bus  
4. Worker processes notification  
5. Event streamed to Event Hub  
6. Audit Worker logs in Cosmos DB  

---

## 🧩 Services

- **HelpDesk.API**
- **Notification Worker**
- **Audit Worker**

---

## 🐳 Run with Docker

```bash
docker-compose up --build
```

Swagger:
```
http://localhost:5000/swagger
```

---

## ☸️ Run with Kubernetes

```bash
kubectl apply -f k8s/
kubectl port-forward service/helpdesk-api-service 8080:80
```

Swagger:
```
http://localhost:8080/swagger
```

---

## 📊 Features

- Multi-tenancy
- JWT Authentication
- Distributed messaging
- Event streaming
- Cloud integration
- Health checks

---

## 🧠 Patterns

- DDD
- SOLID
- Repository Pattern
- Dependency Injection
- Event-driven architecture

---

## 📈 Scalability

- Stateless API
- Horizontal scaling (K8s)
- Async processing
- Decoupled services

---

## 🔐 Security

- JWT Auth
- Tenant isolation

---

## 📌 Future Improvements

- CI/CD pipeline
- Observability (OpenTelemetry)
- Distributed tracing
- Redis caching

---

## 👨‍💻 Author

Raphael Ribeiro  
