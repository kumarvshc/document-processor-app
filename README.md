
# Document Processor Application

## Introduction

**Document Processor** is a distributed application designed to analyze text files by performing two key operations:

- **Word Position Detection**: Identify the exact positions of words within the file.
- **Pattern Matching**: Search for and match regular expression patterns in the file content.

The application reads files from a specified folder path and processes them sequentially, ensuring accurate and efficient text analysis.

---

## Features
- Distributed architecture for scalability.
- Supports custom folder paths for file input.
- Detects word positions within text files.
- Matches user-defined regular expression patterns.
- Stores processed results in an Azure SQL Database.

---

## How It Works
1. Specify the folder path containing the files.
2. The processor reads each file one by one.
3. Performs word position detection and regex pattern matching.
4. Captures and stores the results in an SQL database for further processing.

---

## Design Patterns
- **Factory Pattern** – For object creation.
- **Repository Pattern** – For data access abstraction.
- **Unit of Work** – For managing transactions.
- **Dependency Injection** – For loose coupling and testability.
- **Result Pattern** – For consistent operation results.

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

# Document Processor System

This project implements a scalable **Document Processing System** following **Clean Architecture** principles with synchronous and asynchronous (distributed) processing.

---

## Application Execution Options

The application can be executed using either of the following approaches:

- **Console Application**
- **Swagger (API UI)**

---

## Console Application Flow

- The Console application reads files from a predefined directory.
- The file path is **hardcoded in `Program.cs`**.
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

```text
Created → Processing → Available
```

---

## Requirements
- .NET 8 or later
- FluentValidation for input validation
- Serilog for structured logging
- AutoMapper for DTO mapping
- Swagger for Api testing
- Moq for unit testing and mocking
- Azure Service Bus for messaging
- Azure SQL Database for data storage
- Azure Functions for serverless processing

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
- Replace hardcoded SQL and Azure Service Bus connection strings with Managed Identity or Azure Key Vault.
- Add comprehensive unit tests for all components.
- A CI/CD pipeline should be created.
- Optionally, create an Azure App Service and host the API.

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

# Design Trade-Off

Instead of passing only the **Document ID** to the Azure Service Bus queue (since the document content already exists in the `Documents` table), we are currently passing the **document content** itself.

## Reason
- The content size is small (maximum **1 KB**), so including it in the message reduces the need for an additional database lookup.
- This approach minimizes latency and simplifies processing at the consumer end.

---
