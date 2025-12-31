
# Document Processor Application

## Introduction

**Document Processor** is a distributed application designed to analyze text files by performing two key operations:

- **1. Word Position Detection**: Identify the exact positions of words within the file.
- **2. Pattern Matching**: Search for and match regular expression patterns in the file content.

## How It Works
You can initiate processing in two ways:
- **Swagger UI**  
Submit requests through the hosted API’s Swagger interface for quick testing and integration.
Access [Swagger UI](https://doc-processor-api.azurewebsites.net/swagger/index.html)

- **Console Application**
Execute the console app to read files from a specified folder path and process them sequentially in a single operation.
Download Console App [DocumentProcessor.Console.exe](doc-processor-consoleapp/exe/DocumentProcessor.Console.exe)

---

## Prerequisites
- .NET 8 or later
- FluentValidation for input validation
- Serilog for structured logging
- AutoMapper for DTO mapping
- Swagger for Api testing
- Moq for unit testing and mocking
- Azure Service Bus for messaging
- Azure SQL Database for data storage (can use Azure Data Studio instead of Sql Server Managment tool)
- Azure Functions for serverless processing
- Azure Key Vault to keep credentials

---

## Features
- Distributed architecture for scalability.
- Prompt the user to enter a folder path, then process all file(s) within that folder.
- Detects word positions within text files.
- Matches user-defined regular expression patterns.
- Stores processed results in an Azure SQL Database.

---


# Document Processor System

## Console Application Flow

- The Console application reads files from a directory. The file path is **prompt from user**.
- Files are processed **one by one**.
- For each file:
  - A request body is constructed.
  - The request is sent to the **Document Processor API**.

---

## API Layer Flow

- The **Document Processor API** receives the request.
- The request body is validated using **FluentValidation**.
- After successful validation:
  - The API maps the **API Request DTO** to an **Application DTO (record type)** using **AutoMapper**.
- The API invokes the **Application layer** to execute business logic.

---

## Application Layer Flow

- The Application layer:
  - Orchestrates the business workflow.
  - Coordinates interactions between layers.
  - Calls the **Infrastructure layer** when persistence or Azure service bus.
- The Application layer does **not** directly access the database.

---

## Infrastructure Layer Flow

- The Infrastructure layer:
  - Binds the **DbContext**.
  - Performs all database-related operations.
  - Executes actual business data operations using **Domain Entities**.
- The Infrastructure layer returns results back to the Application layer.

---

## Result Handling Flow

- The Application layer:
  - Wraps responses using the **Result Pattern** (Success / Failure).
  - Returns the result to the API layer.
- The API layer:
  - Validates the Result pattern.
  - Maps the Application result to an **API Response DTO**.
  - Returns the final response to the client.

---

## Distributed Processing Flow

### Document Creation (Asynchronous Processing)

- When a **Create Document** request is processed:
  - The Application layer publishes a message to an **Azure Service Bus Queue**.

---

### Scanner Azure Function

- The **Scanner Azure Function** is triggered when a message is received in the queue.
- It performs the following steps:
  - Scans the document for **dangerous keywords**.
  - Persists the scan results into the database via the Application layer.
  - Publishes the same message to the **Extract Pattern Queue**.

---

### Extract Pattern Azure Function

- The **Extract Pattern Azure Function** is triggered by messages from the Extract Pattern Queue.
- It performs:
  - **Regular expression pattern matching**.
  - Inserts extracted pattern data into the database.
  - Updates the document status from **Processing** to **Available**.

---

## Status Flow

```
Unknown → Processing → Available
```

---

## Design Patterns
- **Factory Pattern** – For object creation.
- **Repository Pattern** – For data access abstraction.
- **Unit of Work** – For managing transactions.
- **Dependency Injection** – For loose coupling and testability.
- **Result Pattern** – For consistent operation results.
- **DDD Pattern** – For maintaining clear boundaries, and improving maintainability.
- **Event-Driven** – For decoupling components and enabling asynchronous communication.
- **Microservice** - For building independently deployable, loosely coupled services.
  
---

## Design Principles
- **SOLID** – Maintainable and scalable code.
- **KISS** – Keep it simple and straightforward.
- **DRY** – Avoid code duplication.
- **YAGNI** – Implement only what is necessary.

---

## Architecture

The **Document Processor** application is designed with a clean and maintainable architecture that promotes separation of concerns and scalability. It follows a layered approach with the following components:

- **Domain**  
  Contains the core business logic, entities, and value objects that represent the problem statement.

- **Infrastructure**  
  Handles external concerns such as database access, messaging (Azure Service Bus).

- **Application**  
  Implements use cases and orchestrates interactions between the domain and infrastructure layers. Includes services, validators, and business workflows.

- **API**  
  Exposes endpoints for client interaction, leveraging the application layer for processing requests and returning responses.

This architecture ensures clarity, testability, and adherence to best practices like **SOLID principles** and **clean architecture**.

Additionaly created **Common** and **Conatants** layer to supporting maintain clean architecture. 

---

## Usage

### Clone the Repository
```bash
git clone https://github.com/kumarvshc/document-processor-app.git
cd document-processor-app

# For ExtractPattern Function
cd DocumentProcessor.Functions.ExtractPattern
dotnet build
func start --port <port-number>

# For KeyScanner Function
cd DocumentProcessor.Functions.KeyScanner
dotnet build
func start --port <port-number>

# For .Net application
dotnet build
dotnet run

```

## Improvements Needed

- Remove hardcoded values and maintain them in a centralized constant class or xml file.
- Add comprehensive unit tests for all components.
- Bicep has not yet been tested (it has been created through Azure portal Automation), so do not rely on this script to create a neccessary Azure services.
- This project should be validated using any one of the Static Code Analysis tools (eg: SonarQube).
- Currently the projects are referenced as project. Going forward, they should reference NuGet packages.

---

## Technology Selection

### .NET Core (ASP.NET Core)
Chosen for its high performance, cross-platform support, and LTS. It fits well with Clean Architecture and integrates seamlessly with Azure services.

### Azure SQL Database
Selected as a fully managed, reliable relational database with strong transactional support, and native compatibility with Entity Framework Core.

### Azure Service Bus
Used for reliable, asynchronous messaging to decouple components and support distributed document processing with guaranteed message delivery.

### Azure Functions
Adopted for serverless, event-driven background processing. Functions automatically scale, and efficiently process Service Bus messages.

---

## Design Trade-Off - Document Id and Content

Instead of passing only the **Document Id** to the Azure Service Bus queue (since the document content already exists in the `Documents` table), I am currently passing the **document content** itself.

### Reason
- The content size is small (maximum **1 KB**), so including it in the message reduces the need for an additional database lookup.
- This approach minimizes latency and simplifies processing at the consumer end.

---

## Design Trade-Off - Transaction Design

Currently, transactions are **not used anywhere else in the application**.  
However, I have included a **basic transaction implementation** in this module to **demonstrate how it could be integrated in the future**.

---

## Design Trade-Off: Direct Application Layer Calls vs. API Calls

### Current Approach
Azure Functions (`KeyScannerFunction` and `ExtractPatternFunction`) invoke the application layer directly using **dependency injection** (`IUnitOfWork` and `IMessageService`) instead of making HTTP calls to the API.

### Reason

1. **Shared Application Boundary**  
   Both Azure Functions and the application layer belong to the same logical boundary, making direct calls a natural fit.

2. **Lower Latency**  
   Avoiding HTTP overhead reduces round-trip time, resulting in faster execution.

---
