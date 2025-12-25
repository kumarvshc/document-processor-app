
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

---

## Requirements
- .NET 8 or later
- FluentValidation for input validation
- Serilog for structured logging
- AutoMapper for DTO mapping
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