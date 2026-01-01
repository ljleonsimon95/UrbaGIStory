---
project_name: 'UrbaGIStory'
user_name: 'AllTech'
date: '2025-12-25'
sections_completed: ['technology_stack', 'language_rules', 'framework_rules', 'testing_rules', 'code_quality', 'workflow_rules', 'critical_rules']
status: 'complete'
rule_count: 85
optimized_for_llm: true
---

# Project Context for AI Agents

_This file contains critical rules and patterns that AI agents must follow when implementing code in this project. Focus on unobvious details that agents might otherwise miss._

---

## Technology Stack & Versions

**Core Framework:**
- .NET 10 (C# 12)
- Blazor WebAssembly (Frontend)
- ASP.NET Core Web API (Backend)

**Database & Spatial:**
- PostgreSQL (latest stable)
- PostGIS extension (required)
- Npgsql.EntityFrameworkCore.PostgreSQL v9.0.x
- Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite v9.0.x
- NetTopologySuite v2.5.x
- NetTopologySuite.IO.GeoJSON v4.0.x

**Frontend UI:**
- MudBlazor (latest stable)
- OpenLayers (via JS Interop, not Blazor wrapper)

**Backend Libraries:**
- FluentValidation (latest stable)
- ASP.NET Core Identity
- Swashbuckle.AspNetCore (Swagger/OpenAPI)

**Critical Version Constraints:**
- Npgsql 9.0.x is required for .NET 10 compatibility
- NetTopologySuite 2.5.x is required for PostGIS spatial operations
- All packages must be compatible with .NET 10

## Critical Implementation Rules

### Language-Specific Rules (C#)

**Async/Await Patterns:**
- ALL repository and service methods MUST be async (suffix with `Async`)
- Use `Task<T>` for return types, never `void` for async methods
- Always use `await` instead of `.Result` or `.Wait()` (prevents deadlocks)
- Use `ConfigureAwait(false)` in library code, but NOT in Blazor components (Blazor requires context)

**Nullable Reference Types:**
- Enable nullable reference types in project files
- Use `?` for nullable types explicitly: `Entity? GetById(int id)`
- Use `!` null-forgiving operator only when absolutely certain (prefer null checks)

**LINQ & EF Core:**
- NEVER use `.ToList()` before filtering - always filter in database queries
- Use `.AsNoTracking()` for read-only queries to improve performance
- Use `.Include()` for eager loading, avoid N+1 queries
- For spatial queries, use NetTopologySuite types directly in LINQ

**Exception Handling:**
- Use custom exceptions from `Exceptions/` folder (EntityNotFoundException, ValidationException, etc.)
- NEVER catch generic `Exception` - catch specific exceptions
- Always log exceptions before re-throwing or handling
- Let GlobalExceptionHandlerMiddleware convert exceptions to ProblemDetails

**String Handling:**
- Use `string.IsNullOrWhiteSpace()` instead of `== null || == ""`
- Use string interpolation `$""` for formatted strings
- Use `StringBuilder` for multiple string concatenations in loops

### Framework-Specific Rules

#### Blazor WebAssembly

**Component Lifecycle:**
- Use `OnInitializedAsync()` for data loading (called once)
- Use `OnParametersSetAsync()` when parameters change
- Call `StateHasChanged()` explicitly only when needed (Blazor auto-detects most changes)
- Dispose IDisposable resources in `Dispose()` or `DisposeAsync()`

**JS Interop:**
- ALWAYS use `MapInterop` namespace from `map-interop.js` - never create global functions
- Use `IJSRuntime.InvokeAsync<T>()` for calling JavaScript functions
- Use `DotNetObjectReference` for JavaScript-to-Blazor callbacks
- Map loads GeoJSON directly from API URLs - NEVER pass large geometry data through JS Interop
- Clean up DotNetObjectReference in Dispose to prevent memory leaks

**State Management:**
- Use component parameters and local state - NO global state management library needed
- Use `@bind` for two-way binding, `@bind:event` for specific events
- For complex state, use injected services (IEntityService, etc.)

**MudBlazor:**
- Use MudBlazor components consistently (MudButton, MudTextField, etc.)
- Follow MudBlazor theming - do NOT override with custom CSS unless necessary
- Use MudBlazor's built-in validation components

#### ASP.NET Core Web API

**Controller Patterns:**
- Controllers MUST be thin - delegate to services, no business logic
- Use `[ApiController]` attribute on all controllers
- Use `[Route("api/[controller]")]` for route templates
- Use `[Authorize]` attribute for protected endpoints
- Return `IActionResult` or `ActionResult<T>` - never raw types

**Dependency Injection:**
- Register all services in `Program.cs` or `ServiceCollectionExtensions.cs`
- Use interfaces for all services and repositories
- Use scoped lifetime for DbContext and services
- Use singleton only for stateless services (e.g., configuration)

**EF Core Configuration:**
- MUST call `modelBuilder.HasPostgresExtension("postgis")` in `OnModelCreating`
- Use Fluent API configurations in `Data/Configurations/` folder
- Configure GIST indexes for geometry columns: `.HasIndex(e => e.Geometry).HasMethod("GIST")`
- Configure GIN indexes for JSONB columns: `.HasIndex(e => e.DynamicProperties).HasMethod("GIN")`
- Apply configurations with `modelBuilder.ApplyConfigurationsFromAssembly()`

**PostGIS Spatial Queries:**
- Use NetTopologySuite `Geometry` type for spatial properties
- Use `.Distance()` for distance calculations (returns meters)
- Use `.Intersects()`, `.Contains()`, `.Within()` for spatial predicates
- ALWAYS use spatial indexes (GIST) - verify in migration
- For complex spatial queries, use raw SQL with `FromSqlRaw()` if needed

**JSONB Dynamic Properties:**
- Store dynamic properties in JSONB column `DynamicProperties`
- Use GIN index on JSONB column for efficient queries
- Query JSONB with `.Where(e => EF.Functions.JsonContains(e.DynamicProperties, ...))`
- Validate JSONB structure in application layer (category system)

### Testing Rules

**Test Organization:**
- Separate test projects in `tests/` folder
- Test project naming: `{ProjectName}.Tests` (e.g., `UrbaGIStory.Server.Tests`)
- Mirror source structure in test projects (Services/, Repositories/, Controllers/)

**Test Naming:**
- Test method naming: `{MethodName}_{Scenario}_{ExpectedResult}`
- Example: `GetEntityByIdAsync_WhenEntityExists_ReturnsEntity`
- Use descriptive test names that explain what is being tested

**Mocking:**
- Mock repository interfaces, not concrete implementations
- Use Moq or NSubstitute for mocking
- Mock services in controller tests
- Use in-memory database for integration tests (EF Core InMemory provider)

**Test Structure:**
- Arrange-Act-Assert (AAA) pattern
- One assertion per test (when possible)
- Test edge cases: null, empty, invalid inputs
- Test error scenarios: not found, validation failures

**Integration Tests:**
- Use TestServer for API integration tests
- Use separate test database or in-memory database
- Clean up test data after each test

### Code Quality & Style Rules

**Naming Conventions (CRITICAL - prevents conflicts):**

**Database:**
- Tables: PascalCase, singular (Entity, Category, Document)
- Columns: PascalCase (Id, Name, CreatedAt, DynamicProperties)
- Foreign Keys: `{EntityName}Id` (EntityId, CategoryId)
- Indexes: `IX_{TableName}_{ColumnName}`

**API:**
- Endpoints: lowercase, plural, kebab-case for multi-word (`/api/entities`, `/api/entity-types`)
- Route parameters: `{id}` (lowercase)
- Query parameters: camelCase (`?page=1&pageSize=10&categoryId=5`)

**C# Code:**
- Classes/Interfaces: PascalCase (EntityService, IEntityRepository)
- Interfaces: Prefix with `I` (IEntityService)
- Methods: PascalCase, verb-based (GetEntityByIdAsync)
- Properties: PascalCase (Entity.Name, Entity.CreatedAt)
- Variables: camelCase (var entityId = 1;)
- DTOs: Suffix with `Request` or `Response` (CreateEntityRequest, EntityResponse)

**JavaScript (JS Interop):**
- Functions: camelCase (initMap, loadGeoJsonFromUrl)
- Variables: camelCase
- Namespace: `MapInterop` (window.MapInterop)

**File Organization:**
- Place files in correct folders per architecture document
- Repository interfaces: `Repositories/I{Entity}Repository.cs`
- Repository implementations: `Repositories/{Entity}Repository.cs`
- Service interfaces: `Services/I{Entity}Service.cs`
- Service implementations: `Services/{Entity}Service.cs`
- Validators: `Validators/{Action}{Entity}Validator.cs`
- DTOs: `DTOs/Requests/` and `DTOs/Responses/`

**Code Structure:**
- One class per file
- File name matches class name exactly
- Use regions sparingly (only for very large files)
- Group related methods together

**Documentation:**
- Use XML comments for public APIs (controllers, services, repositories)
- Use `/// <summary>` for class and method descriptions
- Document complex business logic with inline comments
- Keep comments up-to-date with code changes

### Development Workflow Rules

**Repository Pattern:**
- ALWAYS use repository interfaces - never access DbContext directly from services
- Repositories handle ALL data access - services never touch DbContext
- Repository methods return domain entities, not DTOs
- Services map entities to DTOs using extension methods in `Mappings/` folder

**Service Layer:**
- Services contain business logic, not controllers
- Services use repository interfaces via dependency injection
- Services validate using FluentValidation validators
- Services throw custom exceptions (EntityNotFoundException, ValidationException)

**Error Handling Flow:**
1. Service throws custom exception (EntityNotFoundException, ValidationException, etc.)
2. GlobalExceptionHandlerMiddleware catches exception
3. Middleware converts to ProblemDetails (RFC 7807)
4. Returns appropriate HTTP status code (404, 400, 500, etc.)

**Validation:**
- Use FluentValidation validators in `Validators/` folder
- Validator naming: `{Action}{Entity}Validator.cs` (CreateEntityValidator, UpdateEntityValidator)
- Register validators in DI container
- Call validation in service layer, not controller

**Mapping:**
- Use manual mapping (extension methods) - NO AutoMapper
- Mapping methods in `Mappings/` folder (EntityMappings.cs)
- Methods: `ToDto()`, `ToEntity()`, `ToResponse()`
- Keep mappings simple and explicit

### Critical Don't-Miss Rules

**QGIS Integration (CRITICAL):**
- QGIS tables are SEPARATE - never modify QGIS tables directly
- Use `qgis_geometries` table via Foreign Key link only
- Foreign Key uses `ON DELETE SET NULL` to handle QGIS deletions gracefully
- Validate geometry exists before creating links
- Handle QGISIntegrationException when QGIS unavailable

**Spatial Data:**
- ALWAYS use NetTopologySuite `Geometry` type, never raw coordinates
- Configure GIST indexes on ALL geometry columns
- Use spatial queries (Intersects, Contains, Within) - never load all and filter in memory
- GeoJSON format: Standard GeoJSON, not custom format

**JSONB Dynamic Properties:**
- Properties stored in JSONB `DynamicProperties` column
- GIN index REQUIRED on JSONB column for performance
- Validate JSONB structure using category system
- Query JSONB with EF.Functions.JsonContains, not string manipulation

**JS Interop Anti-Patterns:**
- ❌ NEVER pass large geometry data through JS Interop (serialization overhead)
- ❌ NEVER create global JavaScript functions (use MapInterop namespace)
- ❌ NEVER forget to dispose DotNetObjectReference (memory leak)
- ✅ DO load GeoJSON directly from API URLs in JavaScript
- ✅ DO use DotNetObjectReference for callbacks
- ✅ DO clean up in Dispose method

**Performance Gotchas:**
- ❌ NEVER use `.ToList()` before filtering (loads all data into memory)
- ❌ NEVER use N+1 queries (use `.Include()` for related data)
- ❌ NEVER forget spatial indexes (GIST) or JSONB indexes (GIN)
- ✅ DO use `.AsNoTracking()` for read-only queries
- ✅ DO use pagination for list endpoints
- ✅ DO optimize PostGIS queries with proper indexes

**Security Rules:**
- ALWAYS use `[Authorize]` on protected endpoints
- NEVER expose sensitive data in API responses
- ALWAYS validate user permissions before data access
- NEVER trust client input - validate all requests
- Use parameterized queries (EF Core handles this automatically)

**Concurrency:**
- Use optimistic concurrency with version field (RowVersion)
- Check version before updates to detect conflicts
- Return 409 Conflict when version mismatch detected
- Soft delete: Set `IsDeleted = true`, never hard delete

**API Response Format:**
- Success: Direct DTO/entity, no wrapper (`{id: 1, name: "..."}`)
- Error: ProblemDetails format (RFC 7807)
- Lists: Array of items, pagination in headers (`X-Total-Count`, `X-Page`, `X-Page-Size`)
- Dates: ISO 8601 format in UTC (`2025-12-25T10:00:00Z`)
- JSON: camelCase for all JSON fields

**Blazor Component Rules:**
- Use `@if (isLoading)` for loading states, not global state
- Call `StateHasChanged()` explicitly only when needed
- Dispose IDisposable resources properly
- Use component parameters for parent-child communication
- Use injected services for API calls, not direct HttpClient

---

## Usage Guidelines

**For AI Agents:**

- Read this file before implementing any code
- Follow ALL rules exactly as documented
- When in doubt, prefer the more restrictive option
- Reference the architecture document (`_bmad-output/architecture.md`) for detailed patterns and examples
- Update this file if new patterns emerge during implementation

**For Humans:**

- Keep this file lean and focused on agent needs
- Update when technology stack changes
- Review quarterly for outdated rules
- Remove rules that become obvious over time
- Maintain focus on unobvious details that agents might miss

**Remember:** When in doubt, refer to the architecture document (`_bmad-output/architecture.md`) for detailed patterns and examples. This context file focuses on unobvious rules that agents might miss.

---

**Last Updated:** 2025-12-25

