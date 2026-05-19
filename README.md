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

tests/ \
├── HelpDesk.API.Tests \
├── HelpDesk.Application.Tests \
├── HelpDesk.Domain.Tests \
├── HelpDesk.Infrastructure.Tests \
├── HelpDesk.Admin.Web.Tests \
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

## 🔐 Configuration & Secrets

No secrets are committed to this repository. Every project's `appsettings.json` ships with **empty placeholders** for sensitive values — they must be supplied at runtime via one of the layered providers below.

### Configuration sources (order of precedence, last wins)

1. `appsettings.json` — committed; safe defaults only (queue names, container names, etc.)
2. `appsettings.{Environment}.json` — gitignored
3. **dotnet user-secrets** — local development
4. Environment variables — used by Docker Compose via `.env`
5. **Azure Key Vault** — production; loaded only when `KeyVault:Uri` is set

### Local development (user-secrets)

Run the interactive helper from the repo root to populate user-secrets for the three runtime projects:

```powershell
pwsh ./scripts/setup-user-secrets.ps1
```

Inspect what is stored for a project:

```powershell
dotnet user-secrets list --project src/HelpDesk.API/HelpDesk.API.csproj
```

User-secrets live outside the repo under `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json` and are never committed.

### Docker Compose

Copy the env template and fill in the values:

```bash
cp src/.env.example src/.env
```

`src/.env` is gitignored. `docker-compose.yml` reads from it via `${VAR}` interpolation.

### Production (Azure Key Vault)

Set `KeyVault__Uri` (env var) or `KeyVault:Uri` (JSON) to your vault URI:

```
KeyVault__Uri=https://<your-vault>.vault.azure.net/
```

The hosts use `DefaultAzureCredential`, so on Azure they authenticate via Managed Identity; locally they fall back to your `az login` session. Key Vault secret names map to config paths with a double-dash separator (e.g. `ConnectionStrings--Default`, `Jwt--Key`).

---

## 🐳 Run with Docker

> Make sure `src/.env` exists first — see [Configuration & Secrets](#-configuration--secrets).

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

- JWT authentication
- Tenant isolation
- Layered secret management: `appsettings.json` → user-secrets → environment variables → Azure Key Vault
- No secrets in version control (see [Configuration & Secrets](#-configuration--secrets))

---

## 📌 Future Improvements

- CI/CD pipeline
- Observability (OpenTelemetry)
- Distributed tracing
- Redis caching

---

## 👨‍💻 Author

Raphael Ribeiro  
