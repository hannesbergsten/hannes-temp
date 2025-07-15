# ThreadPilot Integration Layer

## Overview
This solution implements an integration layer between the new core system (ThreadPilot) and legacy systems, split into two microservices:
- **Insurance API**: Provides insurance information for a person, including monthly costs and car details if applicable.
- **Vehicle API**: Provides vehicle information based on registration number.

## Architecture & Design Decisions
- **Microservices**: Two separate ASP.NET Core APIs for clear separation of concerns and future extensibility.
- **RESTful Endpoints**: Both APIs expose REST endpoints with versioning (`/api/v1/...`).
- **Integration**: Insurance API calls Vehicle API to enrich car insurance data.
- **Abstraction**: External/legacy dependencies are abstracted via interfaces for easy mocking and testing.
- **Error Handling**: Consistent error responses for invalid input and missing data.
- **Extensibility**: Insurance types are modeled with inheritance, making it easy to add new products.
- **Performance**: Vehicle API is called in batch for all relevant insurances, avoiding multiple calls per insurance.

## Running Locally
1. **Prerequisites**: .NET 8 SDK
2. **HTTP Configuration**: Choose the appropriate HTTP configuration profiles for each API in your IDE before starting.
3. **Start Vehicle API**:
   ```bash
   dotnet run --project ThreadPilot.Vehicle.Api
   ```
3. **Start Insurance API**:
   ```bash
   dotnet run --project ThreadPilot.Insurance.Api
   ```
4. **Test Endpoints**:
    - Vehicle: `GET /api/v1/vehicle/{registrationNumber}`
    - Insurance: `GET /api/v1/insurances/{personalId}`

## Testing & Test Strategy
- **Unit Tests**: Located in `ThreadPilot.Insurance.Api.Test/Unit`. Cover business logic, edge cases, and error handling.
- **Integration Tests**: Located in `ThreadPilot.Insurance.Api.Test/Integration`. Cover endpoint behavior and integration between services.
- **Wide Coverage**: Tests include valid/invalid input, missing data, and mixed insurance types. Vehicle enrichment logic is tested.
- **Run All Tests**:
   ```bash
   dotnet test
   ```

## Error Handling
- Invalid or missing input returns `400 Bad Request` with details.
- No data found returns `404 Not Found`.
- All errors are handled gracefully and consistently.

## Extensibility & Patterns
- **Open/Closed Principle**: Insurance types can be extended without modifying existing code.
- **Dependency Injection**: Used throughout for testability and flexibility.
- **API Versioning**: Implemented via FastEndpoints; documented in this README.

## Security (Approach)
- For production, add authentication/authorization middleware (e.g., JWT, OAuth).
- Validate and sanitize all inputs.

## API Versioning & Future Extensibility
- All endpoints are versioned (`/api/v1/...`).
- New versions can be added without breaking existing clients.
- New insurance types or vehicle attributes can be added by extending models and repositories.

## Onboarding Developers
- **Project Structure**: Each API is in its own folder; shared models in `ThreadPilot.Common`.
- **Key Files**: Endpoints, Models, Repository, ApiClients.
- **How to Run**: See above. Use Swagger UI for exploration.
- **How to Test**: Run `dotnet test`.
- **How to Extend**: Add new insurance types by inheriting `InsuranceBase` and updating repositories.

## Reflection
I have previously worked on microservice integrations and API design.
The most interesting part of this assignment was to try out fast endpoints, that was fun.
The main challenge was to setup the boilerplate code.

With more time, I would probably think of these things:
* Implementing authentication
* Expand integration tests with snapshots
* Comprehensive code documentation
* Adding logging and monitoring
* refine the Swagger documentation.
* Add more test projects and tests