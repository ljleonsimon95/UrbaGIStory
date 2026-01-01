---
stepsCompleted: [1, 2, 3, 4, 5, 6, 7, 8]
workflowType: 'architecture'
lastStep: 8
status: 'complete'
completedAt: '2025-12-25'
inputDocuments:
  - prd.md
  - project-planning-artifacts/ux-design-specification.md
  - project-planning-artifacts/research/technical-stack-mapas-research-2025-12-25.md
workflowType: 'architecture'
project_name: 'UrbaGIStory'
user_name: 'AllTech'
date: '2025-12-25'
---

# Architecture Decision Document

_This document builds collaboratively through step-by-step discovery. Sections are appended as we work through each architectural decision together._

## Project Context Analysis

### Requirements Overview

**Functional Requirements:**
The system has 77 functional requirements organized into 9 core capability areas. The most architecturally significant requirements are:

- **QGIS Integration (FR45-FR49)**: Critical integration with external desktop GIS tool. Requires separate data architecture where QGIS and UrbaGIStory operate in parallel without interference. Simple link between entities and geometries stored in QGIS tables.

- **Dynamic Category System (FR20-FR28, FR70)**: Categories function as work templates that dictate what information specialists collect. System automatically displays properties based on categories assigned to entity type. This requires dynamic form generation and filtering capabilities.

- **Entity Management (FR6-FR14, FR61-FR62)**: Entities can be linked to geometries or exist without them. One geometry can link to multiple entities. Requires flexible data model supporting optional spatial relationships.

- **Document Management with Temporal Metadata (FR15-FR19, FR63-FR66)**: Documents attached to entities with temporal metadata. All documents have datetime field (fixed or estimated) for searching. Requires document storage, metadata management, and temporal query capabilities.

- **Dynamic Filtering & Analysis (FR29-FR35, FR69, FR76)**: Robust filtering based on categories and properties. Map updates to show filtered results. Requires efficient spatial queries combined with dynamic property filtering.

- **Multi-user Collaboration (FR43-FR44, FR57-FR60, FR77)**: Multiple specialists work on same entity simultaneously. System maintains data integrity. Uses soft delete strategy. Requires concurrency handling and data consistency mechanisms.

**Non-Functional Requirements:**
18 NFRs drive critical architectural decisions:

- **Performance (NFR1-NFR6)**: Entity selection < 1 second (highest priority), map load < 3 seconds, filter application < 2 seconds. Local network deployment allows good response times but requires optimized spatial queries.

- **Security (NFR7-NFR11)**: Document protection, unauthorized access prevention, data integrity protection, secure authentication. Government sector application handling heritage data.

- **Integration (NFR12-NFR14)**: QGIS integration reliability is critical. System must handle QGIS connection failures gracefully. Entities without geometries must still be accessible.

- **Reliability (NFR15-NFR18)**: 24/7 availability, backup and recovery, system monitoring, data persistence. Critical for daily operations.

**Scale & Complexity:**
- Primary domain: Full-stack Web Application (Specialized GIS)
- Complexity level: **High**
  - External tool integration (QGIS)
  - Complex spatial data (PostGIS)
  - Dynamic categorization system
  - Multi-user collaboration
  - Hybrid data model (SQL + JSONB)
- Estimated architectural components: 8-12 major components

### Technical Constraints & Dependencies

**Stack Already Defined:**
- Frontend: Blazor WebAssembly (.NET 10)
- UI Framework: MudBlazor
- Maps Library: OpenLayers (selected based on research)
- Backend: .NET 10 Web API
- Database: PostgreSQL + PostGIS
- Spatial ORM: NetTopologySuite + Npgsql
- Integration: JS Interop custom (no mature Blazor wrappers for OpenLayers)

**QGIS Integration Constraint:**
- Geometries created exclusively in QGIS
- QGIS and UrbaGIStory operate in parallel
- Separate tables in same database
- Simple link between entities and geometries
- Critical requirement: must not interfere with QGIS operations

**Deployment Constraints:**
- Local deployment (localhost)
- Internal network only
- No cloud infrastructure
- Desktop-first (no mobile initially)

**UX Constraints:**
- Two distinct flows: Edit/Add vs Consult (both require map view)
- Dynamic property display based on categories
- Map always visible in both modes
- Desktop-optimized interface

### Cross-Cutting Concerns Identified

1. **QGIS Integration Architecture**
   - Separate data tables for QGIS vs UrbaGIStory
   - Link mechanism between entities and geometries
   - Parallel operation without interference
   - Error handling when QGIS unavailable

2. **Dynamic Category System**
   - Category-to-property mapping
   - Dynamic form generation
   - Property validation based on category
   - Dynamic filtering based on categories

3. **Spatial Data Performance**
   - PostGIS query optimization
   - Geometry loading and rendering
   - Map performance with large datasets
   - Spatial indexing strategy

4. **Multi-user Concurrency**
   - Simultaneous entity editing
   - Data integrity mechanisms
   - Soft delete implementation
   - Conflict resolution strategy

5. **JS Interop Architecture**
   - Blazor-OpenLayers communication pattern
   - Event handling (map clicks → Blazor)
   - Data transfer optimization (GeoJSON direct to map)
   - Memory management and cleanup

6. **Hybrid Data Model**
   - SQL for structured data (entities, relationships)
   - JSONB for dynamic properties (category variables)
   - Indexing strategy for JSONB queries
   - Query performance optimization

### Architecture Decision Records (ADRs)

Through collaborative architectural analysis, the following key decisions have been made with explicit trade-offs:

#### ADR 1: QGIS Integration Architecture

**Decision:** Separate tables with Foreign Key linking

**Approach:**
- QGIS tables: `qgis_geometries` (managed exclusively by QGIS)
- UrbaGIStory tables: `entities`, `entity_geometry_links`
- Link mechanism: `entity_geometry_links.geometry_id → qgis_geometries.id` (FK)

**Trade-offs Considered:**
- ✅ **Pros**: Parallel operation without interference, QGIS never touches UrbaGIStory data, simple link mechanism, maintainable
- ⚠️ **Cons**: Requires manual link synchronization, risk if QGIS deletes geometries without notification

**Alternative Considered:** Materialized view shared approach - rejected due to stronger coupling and complexity

**Rationale:** Minimizes risk of interference, enables true parallel operation, simple link mechanism. Synchronization risk mitigated through application-level validation.

**Implementation Notes:**
- Foreign key with ON DELETE SET NULL to handle QGIS geometry deletions gracefully
- Application validates geometry existence before creating links
- Monitoring for orphaned links

---

#### ADR 2: Hybrid Data Model (SQL + JSONB)

**Decision:** SQL structure + JSONB column for dynamic properties

**Approach:**
- SQL tables for entities, relationships, metadata (structured data)
- JSONB column `dynamic_properties` for category variables
- GIN indexes on JSONB for efficient queries

**Trade-offs Considered:**
- ✅ **Pros**: Total flexibility, no schema changes needed, efficient JSONB queries with GIN indexes, scales without major refactoring
- ⚠️ **Cons**: Validation in application layer, more complex queries, less type-safety

**Alternative Considered:** EAV (Entity-Attribute-Value) table - rejected due to expensive joins and poor performance with many properties

**Rationale:** PostGIS already handles JSONB well, GIN indexes are efficient, hybrid model allows scaling without major refactoring. Validation handled in application layer through category system.

**Implementation Notes:**
- GIN index on `dynamic_properties` JSONB column
- Category system validates property types and constraints
- Repository pattern abstracts query complexity

---

#### ADR 3: JS Interop Architecture for OpenLayers

**Decision:** Thin wrapper JS Interop pattern with direct API data loading

**Approach:**
- JavaScript module `map-interop.js` with specific functions
- Blazor orchestrates but doesn't transfer large data
- Map loads GeoJSON directly from API endpoints
- Events: JavaScript → Blazor via DotNetObjectReference

**Trade-offs Considered:**
- ✅ **Pros**: Minimal overhead, data goes directly to map (not through Blazor), simple pattern, optimal performance
- ⚠️ **Cons**: Requires maintaining JavaScript code, more complex debugging

**Alternative Considered:** Full Blazor component wrapper - rejected due to serialization overhead and performance degradation

**Rationale:** Map must load GeoJSON directly from API for performance. Blazor orchestrates but doesn't transfer large geometries. Debugging handled through logging and DevTools.

**Implementation Notes:**
- Map loads GeoJSON via `fetch()` directly from API
- Blazor only sends commands (zoom, center, add layer)
- Event callbacks use DotNetObjectReference for efficiency
- Memory cleanup with IAsyncDisposable pattern
- **Party Mode Insight**: Document JS Interop pattern from the start and create integration tests to validate Blazor-OpenLayers communication

---

#### ADR 4: Dynamic Category System - Property Generation

**Decision:** Runtime property generation from category definitions

**Approach:**
- Categories stored in database with property definitions
- On entity selection: system queries assigned categories → generates dynamic form at runtime
- Properties rendered with MudBlazor components based on type

**Trade-offs Considered:**
- ✅ **Pros**: Total flexibility, no recompilation needed, immediate changes, configurable by office manager
- ⚠️ **Cons**: Runtime validation, potential type errors, more complex testing

**Alternative Considered:** Build-time code generation - rejected due to requiring recompilation and less flexibility

**Rationale:** Categories are configurable by office manager. Runtime allows changes without deployment. Validation handled through category system and business rules.

**Implementation Notes:**
- Category definitions include property types, constraints, validation rules
- Form generation service creates MudBlazor components dynamically
- Validation rules enforced at both client and server
- Category changes take effect immediately (no restart needed)
- **Party Mode Insight**: Properties must be visually grouped by category when entity has multiple categories assigned. Use MudExpansionPanels with collapsible headers for clarity.

---

#### ADR 5: Multi-user Concurrency Strategy

**Decision:** Optimistic concurrency with version-based conflict detection

**Approach:**
- Each entity has `version` timestamp or row version
- On save: verify version hasn't changed
- If changed: show conflict, allow manual merge

**Trade-offs Considered:**
- ✅ **Pros**: No locks, good UX, allows simultaneous work, non-blocking
- ⚠️ **Cons**: Manual conflict resolution, potential data loss if not handled properly

**Alternative Considered:** Pessimistic locking - rejected due to blocking work and poor UX

**Rationale:** Specialists work on different aspects (architectural vs heritage), real conflicts will be rare. Optimistic allows fluid collaboration. Soft delete (FR77) helps maintain integrity.

**Implementation Notes:**
- Entity table includes `row_version` or `updated_at` timestamp
- Repository checks version before update
- Conflict resolution UI shows both versions for manual merge
- Audit trail tracks all changes

---

#### ADR 6: Dynamic Filtering Strategy with PostGIS

**Decision:** Hybrid query combining PostGIS spatial filters with JSONB property filters

**Approach:**
- Single query combining PostGIS spatial operations with JSONB GIN operators
- Leverages both GIST (spatial) and GIN (JSONB) indexes
- Complex query abstracted in repository layer

**Trade-offs Considered:**
- ✅ **Pros**: Single query, efficient, leverages both PostGIS and JSONB indexes, scales well
- ⚠️ **Cons**: Complex query, requires careful optimization

**Alternative Considered:** Two-phase filtering (spatial then in-memory) - rejected due to inefficiency and poor scalability

**Rationale:** PostGIS and PostgreSQL handle hybrid queries well. With proper indexes (GIST for spatial, GIN for JSONB), performance is excellent. Complexity abstracted in repository.

**Implementation Notes:**
- GIST index on geometry columns
- GIN index on JSONB `dynamic_properties` column
- Query builder service constructs hybrid queries
- Performance monitoring for query optimization
- **Party Mode Insight**: Need pagination or lazy loading for large datasets (1,000-5,000 entities expected in MVP). Validate filtering performance with real data volumes.

---

### Summary of Architectural Decisions

| ADR | Decision | Impact | Risk Level |
|-----|----------|--------|------------|
| QGIS Integration | Separate tables with FK | Low risk, parallel operation | Low |
| Data Model | SQL + JSONB hybrid | Flexibility without sacrificing performance | Medium |
| JS Interop | Thin wrapper, direct data | Optimal performance | Low |
| Categories | Runtime property generation | Total flexibility | Medium |
| Concurrency | Optimistic with versioning | Fluid collaboration | Low |
| Filtering | Hybrid PostGIS + JSONB query | Performance and scalability | Low |

### Party Mode Collaborative Insights

**Technical Spike Recommendation:**
- **Priority**: High - Validate JS Interop pattern before full implementation
- **Scope**: 2-3 day spike to validate:
  1. OpenLayers loads GeoJSON directly from API
  2. Blazor receives click events from map
  3. Panel displays properties dynamically
  4. Loading states and visual feedback during map operations

**UX Considerations:**
- **Office Manager Category Assignment View**: Most complex UX challenge. Requires hierarchical view showing: Entity Type → Assigned Categories → Available Properties. Consider Notion-style drag-and-drop for category assignment.
- **Property Grouping**: When entity has multiple categories, properties must be visually grouped by category using MudExpansionPanels with collapsible headers.
- **Loading States**: Skeleton loaders in property panel while map loads GeoJSON. JS Interop should expose progress events that Blazor listens to.

**Performance Validation:**
- **Expected Volume**: 1,000-5,000 entities in MVP
- **Filtering Performance**: Validate hybrid PostGIS + JSONB queries maintain < 2 seconds (NFR3) with real data volumes
- **Map Loading**: Ensure initial geometry load stays within 3 seconds (NFR2) with expected entity count

**Testing Strategy:**
- Integration tests for Blazor-OpenLayers JS Interop communication
- Performance tests for filtering with large datasets
- Validation tests for dynamic property generation with multiple categories

## Starter Template Evaluation

### Primary Technology Domain

**Full-stack Web Application** using .NET 10 + Blazor WebAssembly based on project requirements analysis.

The stack is already defined:
- Frontend: Blazor WebAssembly (.NET 10)
- Backend: .NET 10 Web API
- Database: PostgreSQL + PostGIS
- UI Framework: MudBlazor
- Maps: OpenLayers (JS Interop)

### Starter Options Considered

**Option 1: Official .NET Blazor WebAssembly Template**
- Command: `dotnet new blazorwasm`
- Provides: Basic Blazor WASM project structure, minimal dependencies
- **Evaluation**: Too minimal - lacks backend API, database setup, and project structure for full-stack application

**Option 2: .NET Blazor WebAssembly with ASP.NET Core Hosted**
- Command: `dotnet new blazorwasm -ho` (hosted option)
- Provides: Blazor WASM + Web API in same solution, shared project for models
- **Evaluation**: Better structure but still requires significant setup for PostGIS, MudBlazor, authentication, etc.

**Option 3: Custom Project Structure (Recommended)**
- No starter template - define structure from scratch based on architectural decisions
- **Evaluation**: Best fit - allows full control over architecture, aligns with ADRs, incorporates all required components from the start

### Selected Approach: Custom Project Structure

**Rationale for Selection:**

Given the specific requirements:
- PostgreSQL/PostGIS integration (not standard in .NET templates)
- MudBlazor UI framework (needs to be added)
- OpenLayers JS Interop (custom integration)
- Dynamic category system (domain-specific architecture)
- QGIS integration pattern (unique requirement)

A custom project structure is more appropriate than trying to adapt a generic starter. This allows us to:
1. Establish the exact architecture from ADRs from day one
2. Include all required dependencies and configurations upfront
3. Set up proper separation of concerns (Frontend, Backend, Shared)
4. Configure PostGIS and spatial data handling correctly from the start
5. Structure code to support dynamic category system and JS Interop patterns

**Project Structure to Initialize:**

```
UrbaGIStory/
├── src/
│   ├── UrbaGIStory.Client/          # Blazor WebAssembly
│   │   ├── Components/              # MudBlazor components
│   │   ├── Services/                # API clients, JS Interop
│   │   ├── wwwroot/
│   │   │   └── js/
│   │   │       └── map-interop.js   # OpenLayers integration
│   │   └── UrbaGIStory.Client.csproj
│   │
│   ├── UrbaGIStory.Server/          # .NET Web API
│   │   ├── Controllers/
│   │   ├── Services/                # Business logic
│   │   ├── Repositories/            # Data access
│   │   ├── Models/                  # Domain models
│   │   └── UrbaGIStory.Server.csproj
│   │
│   └── UrbaGIStory.Shared/          # Shared models/DTOs
│       └── UrbaGIStory.Shared.csproj
│
├── tests/
│   ├── UrbaGIStory.Server.Tests/
│   └── UrbaGIStory.Client.Tests/
│
└── UrbaGIStory.sln
```

**Initialization Commands:**

```bash
# Create solution
dotnet new sln -n UrbaGIStory

# Create Blazor WebAssembly project
dotnet new blazorwasm -n UrbaGIStory.Client -o src/UrbaGIStory.Client

# Create Web API project
dotnet new webapi -n UrbaGIStory.Server -o src/UrbaGIStory.Server

# Create shared library
dotnet new classlib -n UrbaGIStory.Shared -o src/UrbaGIStory.Shared

# Add projects to solution
dotnet sln add src/UrbaGIStory.Client/UrbaGIStory.Client.csproj
dotnet sln add src/UrbaGIStory.Server/UrbaGIStory.Server.csproj
dotnet sln add src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj

# Add reference from Client to Shared
dotnet add src/UrbaGIStory.Client/UrbaGIStory.Client.csproj reference src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj

# Add reference from Server to Shared
dotnet add src/UrbaGIStory.Server/UrbaGIStory.Server.csproj reference src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj
```

**Architectural Decisions Provided by This Structure:**

**Language & Runtime:**
- C# 12 (.NET 10)
- Blazor WebAssembly for frontend
- ASP.NET Core Web API for backend
- Shared library for common models and DTOs

**Project Organization:**
- Clear separation: Client (Blazor WASM), Server (Web API), Shared (common code)
- Solution structure supports full-stack development
- Test projects structure ready for unit and integration tests

**Development Experience:**
- Standard .NET tooling (dotnet CLI, Visual Studio, VS Code)
- Hot reload for Blazor development
- Integrated debugging across client and server
- Standard .NET project structure familiar to .NET developers

**Next Steps After Initialization:**
1. Add MudBlazor package to Client project
2. Configure PostgreSQL/PostGIS in Server project
3. Add NetTopologySuite and Npgsql packages
4. Set up JS Interop structure for OpenLayers
5. Configure authentication and authorization
6. Set up Entity Framework Core with PostGIS support

**Note:** Project initialization using these commands should be the first implementation story.

## Core Architectural Decisions

### Decision Priority Analysis

**Critical Decisions (Block Implementation):**
- Backend architecture pattern and folder structure
- Data access pattern (Repository)
- Authentication and authorization strategy
- Entity Framework Core + PostGIS configuration
- API design and structure

**Important Decisions (Shape Architecture):**
- Validation strategy (FluentValidation)
- Error handling approach
- Service layer organization
- API documentation (Swagger)

**Deferred Decisions (Post-MVP):**
- API versioning (not needed for MVP)
- Rate limiting (can be added later if needed)
- Advanced caching strategies

### Backend Architecture & Structure

#### Architecture Pattern Decision

**Decision:** Layered Architecture (single project with folders)

**Rationale:**
- Project is medium-sized (8-12 components)
- Local deployment simplifies infrastructure needs
- Folder organization provides sufficient separation
- Faster to develop and maintain than multi-project Clean Architecture
- Sufficient for project scope and team size

**Structure:**
```
src/UrbaGIStory.Server/
├── Controllers/              # API endpoints
│   ├── EntitiesController.cs
│   ├── CategoriesController.cs
│   ├── DocumentsController.cs
│   ├── FiltersController.cs
│   └── QGISController.cs
│
├── Services/                 # Business logic
│   ├── IEntityService.cs
│   ├── EntityService.cs
│   ├── ICategoryService.cs
│   ├── CategoryService.cs
│   ├── IDocumentService.cs
│   ├── DocumentService.cs
│   ├── IFilterService.cs
│   ├── FilterService.cs          # Hybrid PostGIS + JSONB filtering
│   ├── IQGISIntegrationService.cs
│   └── QGISIntegrationService.cs # QGIS link validation
│
├── Repositories/             # Data access (Repository pattern)
│   ├── IEntityRepository.cs
│   ├── EntityRepository.cs
│   ├── ICategoryRepository.cs
│   ├── CategoryRepository.cs
│   ├── IDocumentRepository.cs
│   ├── DocumentRepository.cs
│   ├── IQGISGeometryRepository.cs
│   └── QGISGeometryRepository.cs
│
├── Models/                   # Domain entities
│   ├── Entity.cs
│   ├── Category.cs
│   ├── Document.cs
│   ├── QGISGeometryLink.cs
│   └── ApplicationUser.cs
│
├── Data/                     # EF Core configuration
│   ├── AppDbContext.cs
│   ├── Configurations/       # Fluent API configurations
│   │   ├── EntityConfiguration.cs
│   │   ├── CategoryConfiguration.cs
│   │   └── QGISGeometryLinkConfiguration.cs
│   └── Migrations/           # EF Core migrations
│
├── DTOs/                     # Data Transfer Objects
│   ├── Requests/
│   │   ├── CreateEntityRequest.cs
│   │   ├── UpdateEntityRequest.cs
│   │   ├── FilterRequest.cs
│   │   └── AssignCategoryRequest.cs
│   └── Responses/
│       ├── EntityResponse.cs
│       ├── FilterResultResponse.cs
│       └── CategoryResponse.cs
│
├── Mappings/                 # Extension methods for mapping
│   └── EntityMappings.cs    # ToDto(), ToEntity() methods
│
├── Validators/               # FluentValidation validators
│   ├── CreateEntityValidator.cs
│   ├── UpdateEntityValidator.cs
│   └── FilterRequestValidator.cs
│
├── Exceptions/               # Custom exceptions
│   ├── EntityNotFoundException.cs
│   ├── ValidationException.cs
│   ├── QGISIntegrationException.cs
│   └── CategoryNotFoundException.cs
│
├── Middleware/               # Custom middleware
│   └── GlobalExceptionHandlerMiddleware.cs
│
├── Identity/                 # ASP.NET Core Identity
│   ├── ApplicationUser.cs
│   ├── ApplicationRole.cs
│   └── IdentityService.cs
│
├── Extensions/               # Extension methods
│   ├── ServiceCollectionExtensions.cs
│   └── ApplicationBuilderExtensions.cs
│
└── Program.cs                # Startup configuration
```

### Data Architecture

#### Repository Pattern Decision

**Decision:** Full Repository Pattern with interfaces

**Rationale:**
- Complex spatial and JSONB queries (ADR 6) benefit from abstraction
- Facilitates testing of data access logic
- Abstracts PostGIS complexity from business layer
- Aligns with ADR 2 (hybrid SQL + JSONB model)

**Implementation:**
- All repositories implement interfaces (IEntityRepository, etc.)
- Repositories handle EF Core operations
- Services use repository interfaces (dependency injection)
- Complex queries (spatial, JSONB filtering) abstracted in repositories

#### Entity Framework Core + PostGIS Configuration

**Decision:** EF Core with Npgsql and NetTopologySuite

**NuGet Packages:**
- `Npgsql.EntityFrameworkCore.PostgreSQL` (v9.0.x)
- `Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite` (v9.0.x)
- `NetTopologySuite` (v2.5.x)
- `NetTopologySuite.IO.GeoJSON` (v4.0.x)

**Configuration:**
```csharp
// Program.cs
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
    )
);

// AppDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.HasPostgresExtension("postgis");
    
    // Apply configurations from Configurations/ folder
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    
    // GIST indexes for spatial data (configured in EntityConfiguration)
    // GIN indexes for JSONB (configured in EntityConfiguration)
}
```

**Indexing Strategy:**
- GIST indexes on geometry columns (spatial queries)
- GIN indexes on JSONB `dynamic_properties` column (property filtering)
- Composite indexes for common query patterns

### Authentication & Security

#### Authentication Strategy

**Decision:** ASP.NET Core Identity + JWT

**Rationale:**
- Supports differentiated roles (FR36-FR42): office manager vs specialists
- Dynamic permissions (FR39-FR40) easier with Identity
- Works well with Blazor WASM
- Scalable if more roles needed in future

**Implementation:**
- ASP.NET Core Identity for user and role management
- JWT tokens for stateless authentication
- Role-based authorization for API endpoints
- Custom authorization policies for dynamic permissions

**Structure:**
```
src/UrbaGIStory.Server/
├── Identity/
│   ├── ApplicationUser.cs      # Extends IdentityUser
│   ├── ApplicationRole.cs      # Extends IdentityRole
│   └── IdentityService.cs       # Custom identity operations
├── Controllers/
│   └── AuthController.cs       # Login, register endpoints
```

**Security Configuration:**
- JWT token expiration: 24 hours (configurable)
- Password requirements: Standard Identity defaults
- HTTPS in production (HTTP acceptable for localhost deployment)

#### CORS Configuration

**Decision:** CORS enabled for Blazor WASM client

**Configuration:**
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

### API & Communication Patterns

#### API Design

**Decision:** RESTful API with standard conventions

**Endpoint Structure:**
- `/api/entities` - GET (list), POST (create)
- `/api/entities/{id}` - GET (get), PUT (update), DELETE (soft delete)
- `/api/entities/{id}/documents` - GET, POST (document management)
- `/api/categories` - GET, POST, PUT (category management)
- `/api/categories/{id}/assign` - POST (assign category to entity type)
- `/api/filters/apply` - POST (apply complex filters)
- `/api/qgis/geometries` - GET (read QGIS geometries)
- `/api/qgis/geometries/{id}/link` - POST (link geometry to entity)
- `/api/auth/login` - POST
- `/api/auth/register` - POST (admin only)

**API Versioning:**

**Decision:** No versioning initially

**Rationale:**
- MVP is complete product from day one (per PRD)
- No external public APIs
- Can add versioning later if needed
- Simplifies initial development

#### API Documentation

**Decision:** Swagger/OpenAPI with Swashbuckle

**Rationale:**
- Interactive API documentation
- Facilitates frontend-backend integration
- Standard in .NET ecosystem
- Useful for development and testing

**Package:** `Swashbuckle.AspNetCore` (v6.x)

**Configuration:**
- Swagger UI enabled in development
- OpenAPI JSON available for code generation
- XML comments included in documentation

### Validation & Mapping

#### Validation Strategy

**Decision:** FluentValidation

**Rationale:**
- Dynamic categories require complex validation (ADR 4)
- JSONB properties need custom validation rules
- Better separation of concerns
- More flexible than Data Annotations

**Structure:**
```
src/UrbaGIStory.Server/
├── Validators/
│   ├── CreateEntityValidator.cs
│   ├── UpdateEntityValidator.cs
│   ├── FilterRequestValidator.cs
│   └── AssignCategoryValidator.cs
```

#### Mapping Strategy

**Decision:** Manual mapping with extension methods

**Rationale:**
- Medium-sized project doesn't justify AutoMapper
- Explicit mapping is clearer
- Fewer dependencies
- Dynamic category mapping may be complex

**Implementation:**
```csharp
// Mappings/EntityMappings.cs
public static class EntityMappings
{
    public static EntityResponse ToDto(this Entity entity) { ... }
    public static Entity ToEntity(this CreateEntityRequest request) { ... }
}
```

### Error Handling

#### Error Handling Strategy

**Decision:** Custom exceptions + Global exception middleware

**Rationale:**
- Standard .NET approach
- ProblemDetails format (ASP.NET Core standard)
- Centralized error handling
- Sufficient for project scope

**Structure:**
```
src/UrbaGIStory.Server/
├── Exceptions/
│   ├── EntityNotFoundException.cs
│   ├── ValidationException.cs
│   ├── QGISIntegrationException.cs
│   └── CategoryNotFoundException.cs
├── Middleware/
│   └── GlobalExceptionHandlerMiddleware.cs
```

**Error Response Format:**
- Uses ProblemDetails (RFC 7807)
- Includes error code, message, and details
- Logs errors for diagnostics

### Service Layer Organization

#### Service Pattern

**Decision:** Service Layer with interfaces

**Rationale:**
- Complex business logic (dynamic categories, filtering)
- Facilitates testing
- Aligns with Repository pattern
- Proper separation of concerns

**Structure:**
- All services implement interfaces (IEntityService, etc.)
- Services use repository interfaces
- Business logic in services, not controllers
- Controllers are thin, delegate to services

### Decision Impact Analysis

**Implementation Sequence:**

1. **Project Structure Setup**
   - Create solution and projects
   - Set up folder structure
   - Configure base dependencies

2. **Data Layer**
   - Configure EF Core + PostGIS
   - Create AppDbContext
   - Set up entity configurations
   - Create initial migrations

3. **Repository Layer**
   - Implement repository interfaces
   - Implement spatial query methods
   - Implement JSONB query methods

4. **Service Layer**
   - Implement service interfaces
   - Implement business logic
   - Category system logic
   - Filter service (hybrid queries)

5. **API Layer**
   - Create controllers
   - Configure Swagger
   - Set up CORS
   - Configure authentication

6. **Integration**
   - QGIS integration service
   - Document management
   - Testing and validation

**Cross-Component Dependencies:**

- **Repositories → EF Core:** All repositories depend on AppDbContext
- **Services → Repositories:** Services use repository interfaces
- **Controllers → Services:** Controllers use service interfaces
- **Services → Validators:** Services use FluentValidation
- **All → Identity:** Authentication/authorization throughout
- **FilterService → Both Repositories:** Combines Entity and Category repositories for hybrid queries

## Implementation Patterns & Consistency Rules

### Pattern Categories Defined

**Critical Conflict Points Identified:**
15+ areas where AI agents could make different choices that would cause implementation conflicts.

### Naming Patterns

#### Database Naming Conventions

**Table Naming:**
- **Pattern:** PascalCase, singular noun
- **Examples:** `Entity`, `Category`, `Document`, `QGISGeometryLink`
- **Rationale:** Aligns with C# conventions, EF Core uses PascalCase by default
- **Anti-pattern:** `entities`, `users`, `user_data` (snake_case or plural)

**Column Naming:**
- **Pattern:** PascalCase for standard columns, descriptive names
- **Examples:** `Id`, `Name`, `CreatedAt`, `DynamicProperties`, `GeometryId`
- **Rationale:** Consistent with C# property naming
- **Anti-pattern:** `user_id`, `created_at`, `dynamic_properties` (snake_case)

**Foreign Key Naming:**
- **Pattern:** `{EntityName}Id` (e.g., `EntityId`, `CategoryId`)
- **Examples:** `EntityId`, `CategoryId`, `GeometryId`, `CreatedById`
- **Rationale:** Clear, follows EF Core conventions
- **Anti-pattern:** `fk_entity`, `entity_fk`, `entity_id` (with prefix/suffix)

**Index Naming:**
- **Pattern:** `IX_{TableName}_{ColumnName}` for standard indexes
- **Examples:** `IX_Entity_CategoryId`, `IX_Document_EntityId`
- **Spatial Indexes:** GIST indexes use default PostGIS naming
- **JSONB Indexes:** GIN indexes use default PostgreSQL naming
- **Rationale:** EF Core convention, clear and searchable

**Navigation Property Naming:**
- **Pattern:** Singular for single relationship, plural for collection
- **Examples:** `Entity.Category`, `Entity.Documents`, `Category.Entities`
- **Rationale:** EF Core conventions, intuitive

#### API Naming Conventions

**Endpoint Naming:**
- **Pattern:** RESTful, plural nouns, lowercase with hyphens for multi-word
- **Examples:** 
  - `/api/entities`
  - `/api/categories`
  - `/api/entity-types`
  - `/api/qgis-geometries`
- **Rationale:** RESTful standard, clear and consistent
- **Anti-pattern:** `/api/entity`, `/api/Entity`, `/api/entityTypes` (singular, PascalCase, camelCase)

**Route Parameters:**
- **Pattern:** `{id}` or `{name}` - lowercase, descriptive
- **Examples:** `/api/entities/{id}`, `/api/categories/{id}`, `/api/entities/{entityId}/documents`
- **Rationale:** ASP.NET Core route parameter convention
- **Anti-pattern:** `:id`, `{Id}`, `{entity_id}` (different syntax, PascalCase, snake_case)

**Query Parameters:**
- **Pattern:** camelCase, descriptive names
- **Examples:** `?page=1&pageSize=10&categoryId=5&includeDeleted=false`
- **Rationale:** Common API convention, JavaScript-friendly
- **Anti-pattern:** `?page_size=10`, `?CategoryId=5` (snake_case, PascalCase)

**HTTP Methods:**
- **Pattern:** Standard REST verbs
- **GET:** Retrieve resources
- **POST:** Create resources
- **PUT:** Update entire resource
- **PATCH:** Partial updates (if needed)
- **DELETE:** Soft delete (per FR77)
- **Rationale:** RESTful standard

#### Code Naming Conventions

**C# Classes and Interfaces:**
- **Pattern:** PascalCase
- **Examples:** `EntityService`, `IEntityRepository`, `CreateEntityRequest`
- **Interfaces:** Prefix with `I` (e.g., `IEntityService`)
- **DTOs:** Suffix with `Request` or `Response` (e.g., `CreateEntityRequest`, `EntityResponse`)
- **Rationale:** C# standard conventions

**C# Methods:**
- **Pattern:** PascalCase, verb-based
- **Examples:** `GetEntityById`, `CreateEntity`, `UpdateEntity`, `DeleteEntity`
- **Async methods:** Suffix with `Async` (e.g., `GetEntityByIdAsync`)
- **Rationale:** C# standard, async naming convention

**C# Properties and Variables:**
- **Pattern:** PascalCase for properties, camelCase for local variables
- **Examples:** 
  - Properties: `Entity.Name`, `Entity.CreatedAt`
  - Variables: `var entityId = 1;`, `var entityName = "Building";`
- **Rationale:** C# standard conventions

**Blazor Components:**
- **Pattern:** PascalCase, descriptive names
- **Examples:** `EntityCard.razor`, `CategorySelector.razor`, `MapComponent.razor`
- **File naming:** Match component name exactly
- **Rationale:** Blazor convention, matches C# class naming

**JavaScript/TypeScript (for JS Interop):**
- **Pattern:** camelCase for functions and variables
- **Examples:** `initMap()`, `loadGeoJsonFromUrl()`, `mapInterop`
- **Constants:** UPPER_SNAKE_CASE (if needed)
- **Rationale:** JavaScript standard conventions

### Structure Patterns

#### Project Organization

**Test Location:**
- **Pattern:** Separate test projects in `tests/` folder
- **Structure:**
  ```
  tests/
  ├── UrbaGIStory.Server.Tests/
  │   ├── Services/
  │   ├── Repositories/
  │   └── Controllers/
  └── UrbaGIStory.Client.Tests/
      └── Components/
  ```
- **Rationale:** Clear separation, standard .NET test project structure
- **Anti-pattern:** Tests co-located with source code

**Component Organization:**
- **Pattern:** By feature/domain, then by type
- **Backend:** Controllers → Services → Repositories (by layer)
- **Frontend:** By feature (e.g., `Entities/`, `Categories/`, `Map/`)
- **Rationale:** Easier to find related code, scales better

**Shared Utilities:**
- **Backend:** `Extensions/` folder for extension methods
- **Frontend:** `Services/` for shared services, `Utils/` for utilities
- **Rationale:** Clear location, easy to discover

**Configuration Files:**
- **Pattern:** `appsettings.json`, `appsettings.Development.json`
- **Location:** Root of Server project
- **Rationale:** ASP.NET Core standard

#### File Structure Patterns

**Entity Configuration Files:**
- **Pattern:** `{EntityName}Configuration.cs` in `Data/Configurations/`
- **Examples:** `EntityConfiguration.cs`, `CategoryConfiguration.cs`
- **Rationale:** Clear, organized, matches EF Core conventions

**Validator Files:**
- **Pattern:** `{Action}{EntityName}Validator.cs` in `Validators/`
- **Examples:** `CreateEntityValidator.cs`, `UpdateEntityValidator.cs`
- **Rationale:** Clear naming, easy to find

**DTO Files:**
- **Pattern:** Organized by `Requests/` and `Responses/` subfolders
- **Examples:** `Requests/CreateEntityRequest.cs`, `Responses/EntityResponse.cs`
- **Rationale:** Clear separation of input/output

**Exception Files:**
- **Pattern:** `{EntityName}{Action}Exception.cs` in `Exceptions/`
- **Examples:** `EntityNotFoundException.cs`, `ValidationException.cs`
- **Rationale:** Descriptive, easy to understand

### Format Patterns

#### API Response Formats

**Success Response:**
- **Pattern:** Direct entity/DTO, no wrapper
- **Examples:**
  ```json
  // GET /api/entities/1
  {
    "id": 1,
    "name": "Building A",
    "entityType": "Building",
    "dynamicProperties": {...},
    "createdAt": "2025-12-25T10:00:00Z"
  }
  ```
- **Rationale:** Simple, standard REST, no unnecessary wrapping
- **Anti-pattern:** `{data: {...}, success: true}` wrapper

**Error Response:**
- **Pattern:** ProblemDetails format (RFC 7807)
- **Examples:**
  ```json
  {
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "detail": "Entity with id 1 was not found",
    "instance": "/api/entities/1"
  }
  ```
- **Rationale:** ASP.NET Core standard, RFC compliant
- **Anti-pattern:** Custom error format, inconsistent structure

**List Response:**
- **Pattern:** Array of items, pagination in headers
- **Examples:**
  ```json
  // GET /api/entities
  [
    {"id": 1, "name": "Building A", ...},
    {"id": 2, "name": "Building B", ...}
  ]
  ```
  Headers: `X-Total-Count: 100`, `X-Page: 1`, `X-Page-Size: 10`
- **Rationale:** Simple, standard REST
- **Anti-pattern:** `{items: [...], total: 100}` wrapper

#### Data Exchange Formats

**JSON Field Naming:**
- **Pattern:** camelCase for JSON (API responses/requests)
- **Examples:** `entityId`, `createdAt`, `dynamicProperties`
- **Rationale:** JavaScript convention, standard for APIs
- **Note:** C# properties use PascalCase, serialization converts to camelCase

**Date/Time Format:**
- **Pattern:** ISO 8601 strings in UTC
- **Examples:** `"2025-12-25T10:00:00Z"`, `"2025-12-25T10:00:00.123Z"`
- **Rationale:** Standard, unambiguous, timezone-aware
- **Anti-pattern:** Unix timestamps, local time strings

**Boolean Values:**
- **Pattern:** `true`/`false` (JSON boolean)
- **Examples:** `"isActive": true`, `"includeDeleted": false`
- **Rationale:** Standard JSON
- **Anti-pattern:** `1`/`0`, `"true"`/`"false"` (strings)

**Null Handling:**
- **Pattern:** `null` for missing values, empty arrays `[]` for collections
- **Examples:** `"geometryId": null`, `"documents": []`
- **Rationale:** Standard JSON, clear intent
- **Anti-pattern:** Omitting fields, `undefined`

**GeoJSON Format:**
- **Pattern:** Standard GeoJSON format for geometry data
- **Examples:**
  ```json
  {
    "type": "Feature",
    "geometry": {
      "type": "Point",
      "coordinates": [-3.7038, 40.4168]
    },
    "properties": {...}
  }
  ```
- **Rationale:** Standard format, OpenLayers compatible
- **Anti-pattern:** Custom geometry format

### Communication Patterns

#### JS Interop Patterns

**Function Naming:**
- **Pattern:** camelCase, descriptive action verbs
- **Examples:** `initMap()`, `loadGeoJsonFromUrl()`, `zoomToFeature()`
- **Rationale:** JavaScript convention, clear intent

**Event Callback Naming:**
- **Pattern:** `on{EventName}` for callbacks
- **Examples:** `onFeatureClicked`, `onMapLoaded`, `onLayerAdded`
- **Rationale:** Standard event naming convention

**JS Interop Module:**
- **Pattern:** Single `map-interop.js` module with namespace
- **Structure:**
  ```javascript
  window.MapInterop = {
    map: null,
    init: function() {...},
    loadGeoJsonFromUrl: function() {...}
  };
  ```
- **Rationale:** Organized, prevents global namespace pollution

#### State Management Patterns

**Blazor Component State:**
- **Pattern:** Component parameters and local state
- **Examples:** `[Parameter] public Entity Entity { get; set; }`
- **State updates:** Use `StateHasChanged()` explicitly when needed
- **Rationale:** Blazor standard, explicit state management

**API Client State:**
- **Pattern:** HttpClient service, no global state management library needed
- **Examples:** `IEntityService` injected, calls API on demand
- **Rationale:** Simple, sufficient for MVP scope
- **Anti-pattern:** Redux/Flux for simple state

### Process Patterns

#### Error Handling Patterns

**Global Error Handling:**
- **Pattern:** `GlobalExceptionHandlerMiddleware` catches all exceptions
- **Location:** `Middleware/GlobalExceptionHandlerMiddleware.cs`
- **Behavior:** Converts exceptions to ProblemDetails, logs errors
- **Rationale:** Centralized, consistent error responses

**Validation Errors:**
- **Pattern:** FluentValidation throws `ValidationException`
- **Response:** 400 Bad Request with validation details in ProblemDetails
- **Example:**
  ```json
  {
    "type": "validation-error",
    "title": "Validation Failed",
    "status": 400,
    "errors": {
      "name": ["Name is required"],
      "entityType": ["Invalid entity type"]
    }
  }
  ```
- **Rationale:** Clear validation feedback

**Not Found Errors:**
- **Pattern:** Custom exception `EntityNotFoundException`
- **Response:** 404 Not Found with ProblemDetails
- **Rationale:** Consistent, informative

**QGIS Integration Errors:**
- **Pattern:** Custom exception `QGISIntegrationException`
- **Response:** 503 Service Unavailable (if QGIS unavailable) or 400 Bad Request (if invalid link)
- **Rationale:** Distinguishes QGIS-specific errors

#### Loading State Patterns

**API Loading States:**
- **Pattern:** No global loading state, component-level loading
- **Blazor:** Use `@if (isLoading)` in components
- **Examples:** `bool isLoading = false;` in component
- **Rationale:** Simple, sufficient for MVP

**Map Loading:**
- **Pattern:** Loading indicator in map container, progress events from JS Interop
- **Examples:** Skeleton loader or spinner while GeoJSON loads
- **Rationale:** User feedback during async operations

**Form Submission:**
- **Pattern:** Disable submit button, show loading indicator
- **Examples:** `[Disabled]="isSubmitting"` on button
- **Rationale:** Prevents double submission, clear feedback

### Enforcement Guidelines

**All AI Agents MUST:**

1. **Follow Naming Conventions:**
   - Database: PascalCase tables/columns
   - API: lowercase plural endpoints, camelCase query params
   - C#: PascalCase classes/methods, camelCase variables
   - JavaScript: camelCase functions/variables

2. **Maintain Structure:**
   - Place files in correct folders per structure defined
   - Use correct file naming patterns
   - Organize tests in separate test projects

3. **Use Standard Formats:**
   - API responses: Direct DTOs, ProblemDetails for errors
   - JSON: camelCase fields, ISO 8601 dates
   - GeoJSON: Standard format for geometries

4. **Follow Communication Patterns:**
   - JS Interop: Use `MapInterop` namespace
   - State: Component-level, explicit `StateHasChanged()`
   - Errors: Use custom exceptions, middleware handles conversion

5. **Implement Processes Consistently:**
   - Error handling: Custom exceptions → Middleware → ProblemDetails
   - Loading: Component-level boolean flags
   - Validation: FluentValidation in validators folder

**Pattern Enforcement:**

- **Code Review:** Check naming, structure, and format compliance
- **Linting:** Use .NET analyzers and style rules
- **Documentation:** Update patterns document if new patterns needed
- **Examples:** Reference this document when implementing new features

### Pattern Examples

**Good Examples:**

```csharp
// ✅ Correct: Repository interface and implementation
public interface IEntityRepository
{
    Task<Entity?> GetByIdAsync(int id);
    Task<IEnumerable<Entity>> GetByCategoryAsync(int categoryId);
}

public class EntityRepository : IEntityRepository
{
    private readonly AppDbContext _context;
    // Implementation
}

// ✅ Correct: Service with interface
public interface IEntityService
{
    Task<EntityResponse> GetEntityByIdAsync(int id);
}

public class EntityService : IEntityService
{
    private readonly IEntityRepository _repository;
    // Implementation
}

// ✅ Correct: Controller using service
[ApiController]
[Route("api/entities")]
public class EntitiesController : ControllerBase
{
    private readonly IEntityService _service;
    // Implementation
}
```

```csharp
// ✅ Correct: DTO naming
public class CreateEntityRequest
{
    public string Name { get; set; }
    public string EntityType { get; set; }
}

public class EntityResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

```javascript
// ✅ Correct: JS Interop pattern
window.MapInterop = {
    map: null,
    init: function(containerId, config) {
        // Initialize map
    },
    loadGeoJsonFromUrl: async function(url) {
        // Load GeoJSON
    }
};
```

**Anti-Patterns:**

```csharp
// ❌ Wrong: No interface, direct DbContext in service
public class EntityService
{
    private readonly AppDbContext _context; // Should use repository
}

// ❌ Wrong: Business logic in controller
[HttpPost]
public async Task<IActionResult> Create(CreateEntityRequest request)
{
    var entity = new Entity { Name = request.Name };
    _context.Entities.Add(entity); // Should use service
    await _context.SaveChangesAsync();
    return Ok(entity);
}

// ❌ Wrong: Wrong naming
public class entity_service // Should be EntityService
{
    public async Task get_entity(int id) // Should be GetEntityAsync
}
```

```javascript
// ❌ Wrong: Global functions, no namespace
function initMap() { } // Should be MapInterop.init
function loadGeoJson() { } // Should be MapInterop.loadGeoJsonFromUrl
```

## Project Structure & Boundaries

### Complete Project Directory Structure

```
UrbaGIStory/
├── .gitignore
├── .editorconfig
├── README.md
├── UrbaGIStory.sln
│
├── src/
│   ├── UrbaGIStory.Client/                    # Blazor WebAssembly Frontend
│   │   ├── UrbaGIStory.Client.csproj
│   │   ├── Program.cs
│   │   ├── App.razor
│   │   ├── App.css
│   │   ├── _Imports.razor
│   │   │
│   │   ├── Components/                        # Blazor Components
│   │   │   ├── Layout/
│   │   │   │   ├── MainLayout.razor
│   │   │   │   └── NavMenu.razor
│   │   │   ├── Pages/
│   │   │   │   ├── Index.razor                # Map view (FR1-FR5)
│   │   │   │   ├── EntityDetail.razor         # Entity view/edit (FR8-FR12, FR61-FR62)
│   │   │   │   ├── EntityList.razor           # Entity list view
│   │   │   │   ├── CategoryManagement.razor   # Category CRUD (FR21-FR23, FR70) - Office Manager only
│   │   │   │   ├── FilterPanel.razor          # Filtering UI (FR29-FR35)
│   │   │   │   ├── DocumentViewer.razor       # Document management (FR15-FR19, FR63-FR66)
│   │   │   │   ├── UserManagement.razor        # User management (FR36-FR38, FR71) - Admin only
│   │   │   │   ├── PermissionManagement.razor # Permission management (FR39-FR40) - Office Manager
│   │   │   │   ├── Dashboard.razor             # Analytics dashboard (FR67-FR68) - Office Manager
│   │   │   │   └── Login.razor                # Authentication
│   │   │   ├── Entities/                      # Entity-related components
│   │   │   │   ├── EntityCard.razor           # Entity display card
│   │   │   │   ├── EntityForm.razor           # Entity create/edit form (FR6-FR7, FR9-FR10)
│   │   │   │   ├── EntityPropertyPanel.razor  # Dynamic properties panel (FR10, FR25-FR27)
│   │   │   │   ├── EntityTypeSelector.razor    # Entity type selection (FR7)
│   │   │   │   └── EntityInfoDisplay.razor    # Entity information display (FR8, FR11, FR58)
│   │   │   ├── Categories/                    # Category-related components
│   │   │   │   ├── CategorySelector.razor      # Category selection
│   │   │   │   ├── CategoryForm.razor         # Category create/edit (FR21-FR22)
│   │   │   │   │   ├── CategoryAssignmentPanel.razor # Assign categories to entity types (FR23)
│   │   │   │   └── PropertyDefinitionEditor.razor    # Property definition editor (FR24)
│   │   │   ├── Map/                           # Map components
│   │   │   │   ├── MapComponent.razor         # Main map component (FR1-FR5, FR35, FR46)
│   │   │   │   ├── MapControls.razor          # Map navigation controls (FR4)
│   │   │   │   ├── EntityLayer.razor          # Entity layer on map (FR2, FR5)
│   │   │   │   └── GeometrySelector.razor     # Geometry selection (FR3, FR6, FR49)
│   │   │   ├── Filters/                       # Filtering components
│   │   │   │   ├── FilterPanel.razor         # Main filter panel (FR29-FR32)
│   │   │   │   ├── CategoryFilter.razor      # Category filter (FR29)
│   │   │   │   ├── PropertyFilter.razor      # Property filter (FR30)
│   │   │   │   ├── EntityTypeFilter.razor     # Entity type filter (FR31)
│   │   │   │   └── FilterResultDisplay.razor  # Filter results display (FR35)
│   │   │   ├── Documents/                     # Document components
│   │   │   │   ├── DocumentList.razor        # Document list (FR16, FR18-FR19)
│   │   │   │   ├── DocumentUpload.razor      # Document upload (FR15)
│   │   │   │   ├── DocumentViewer.razor      # Document preview (FR64)
│   │   │   │   └── DocumentMetadataEditor.razor # Temporal metadata editor (FR17, FR65-FR66)
│   │   │   ├── Reports/                       # Reporting components
│   │   │   │   ├── ReportGenerator.razor     # Report generation (FR34, FR76)
│   │   │   │   ├── ReportViewer.razor        # Report display (FR69)
│   │   │   │   └── AnalyticsDashboard.razor  # Analytics dashboard (FR67-FR68)
│   │   │   └── Shared/                        # Shared components
│   │   │       ├── LoadingIndicator.razor
│   │   │       ├── ErrorDisplay.razor
│   │   │       └── ConfirmationDialog.razor
│   │   │
│   │   ├── Services/                          # Client-side services
│   │   │   ├── IEntityService.cs
│   │   │   ├── EntityService.cs                # Entity API client (FR6-FR14, FR61-FR62)
│   │   │   ├── ICategoryService.cs
│   │   │   ├── CategoryService.cs             # Category API client (FR20-FR28, FR70)
│   │   │   ├── IDocumentService.cs
│   │   │   ├── DocumentService.cs             # Document API client (FR15-FR19, FR63-FR66)
│   │   │   ├── IFilterService.cs
│   │   │   ├── FilterService.cs               # Filter API client (FR29-FR35, FR76)
│   │   │   ├── IAuthService.cs
│   │   │   ├── AuthService.cs                 # Authentication service
│   │   │   ├── IMapInteropService.cs
│   │   │   └── MapInteropService.cs           # JS Interop wrapper for OpenLayers
│   │   │
│   │   ├── Models/                            # Client-side models
│   │   │   ├── EntityDto.cs
│   │   │   ├── CategoryDto.cs
│   │   │   ├── DocumentDto.cs
│   │   │   ├── FilterRequestDto.cs
│   │   │   └── FilterResultDto.cs
│   │   │
│   │   ├── wwwroot/                           # Static files
│   │   │   ├── css/
│   │   │   │   ├── app.css
│   │   │   │   └── bootstrap/
│   │   │   ├── js/
│   │   │   │   └── map-interop.js             # OpenLayers JS Interop (FR2, FR46, ADR 3)
│   │   │   ├── lib/
│   │   │   │   └── openlayers/                # OpenLayers library files
│   │   │   └── favicon.ico
│   │   │
│   │   └── Properties/
│   │       └── launchSettings.json
│   │
│   ├── UrbaGIStory.Server/                    # ASP.NET Core Web API Backend
│   │   ├── UrbaGIStory.Server.csproj
│   │   ├── Program.cs                         # Application entry point
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   │
│   │   ├── Controllers/                       # API Controllers
│   │   │   ├── EntitiesController.cs          # Entity endpoints (FR6-FR14, FR61-FR62)
│   │   │   ├── CategoriesController.cs        # Category endpoints (FR20-FR28, FR70)
│   │   │   ├── DocumentsController.cs         # Document endpoints (FR15-FR19, FR63-FR66)
│   │   │   ├── FiltersController.cs           # Filter endpoints (FR29-FR35, FR76)
│   │   │   ├── QGISController.cs              # QGIS integration endpoints (FR45-FR49)
│   │   │   ├── AuthController.cs              # Authentication endpoints
│   │   │   ├── UsersController.cs             # User management endpoints (FR36-FR38, FR71)
│   │   │   ├── PermissionsController.cs       # Permission endpoints (FR39-FR40)
│   │   │   └── ReportsController.cs           # Report endpoints (FR34, FR67-FR68, FR69, FR76)
│   │   │
│   │   ├── Services/                          # Business Logic Services
│   │   │   ├── IEntityService.cs
│   │   │   ├── EntityService.cs               # Entity business logic (FR6-FR14, FR61-FR62, FR60)
│   │   │   ├── ICategoryService.cs
│   │   │   ├── CategoryService.cs             # Category business logic (FR20-FR28, FR70, FR10, FR25)
│   │   │   ├── IDocumentService.cs
│   │   │   ├── DocumentService.cs             # Document business logic (FR15-FR19, FR63-FR66)
│   │   │   ├── IFilterService.cs
│   │   │   ├── FilterService.cs               # Filter business logic (FR29-FR35, ADR 6)
│   │   │   ├── IQGISIntegrationService.cs
│   │   │   ├── QGISIntegrationService.cs      # QGIS integration logic (FR45-FR49, ADR 1)
│   │   │   ├── IReportService.cs
│   │   │   ├── ReportService.cs               # Report generation (FR34, FR67-FR68, FR69, FR76)
│   │   │   ├── IPermissionService.cs
│   │   │   ├── PermissionService.cs           # Permission management (FR39-FR40)
│   │   │   └── IUserService.cs
│   │   │   └── UserService.cs                 # User management (FR36-FR38, FR71)
│   │   │
│   │   ├── Repositories/                      # Data Access Layer
│   │   │   ├── IEntityRepository.cs
│   │   │   ├── EntityRepository.cs            # Entity data access (FR6-FR14, FR61-FR62, ADR 2)
│   │   │   ├── ICategoryRepository.cs
│   │   │   ├── CategoryRepository.cs          # Category data access (FR20-FR28, FR70)
│   │   │   ├── IDocumentRepository.cs
│   │   │   ├── DocumentRepository.cs         # Document data access (FR15-FR19, FR63-FR66)
│   │   │   ├── IQGISGeometryRepository.cs
│   │   │   └── QGISGeometryRepository.cs     # QGIS geometry access (FR45-FR49, ADR 1)
│   │   │
│   │   ├── Models/                           # Domain Entities
│   │   │   ├── Entity.cs                      # Entity domain model (FR6-FR14, ADR 2)
│   │   │   ├── Category.cs                    # Category domain model (FR20-FR24)
│   │   │   ├── Document.cs                    # Document domain model (FR15-FR19, FR65-FR66)
│   │   │   ├── QGISGeometryLink.cs            # QGIS link model (FR47, ADR 1)
│   │   │   ├── EntityType.cs                  # Entity type enum/model
│   │   │   ├── CategoryProperty.cs             # Category property definition (FR24)
│   │   │   └── ApplicationUser.cs             # User model (extends IdentityUser)
│   │   │
│   │   ├── Data/                              # EF Core Configuration
│   │   │   ├── AppDbContext.cs                # Main DbContext (ADR 2)
│   │   │   ├── Configurations/                # Fluent API Configurations
│   │   │   │   ├── EntityConfiguration.cs     # Entity EF config (geometries, JSONB, indexes)
│   │   │   │   ├── CategoryConfiguration.cs  # Category EF config
│   │   │   │   ├── DocumentConfiguration.cs  # Document EF config
│   │   │   │   ├── QGISGeometryLinkConfiguration.cs # QGIS link config (ADR 1)
│   │   │   │   └── ApplicationUserConfiguration.cs # User EF config
│   │   │   └── Migrations/                    # EF Core Migrations
│   │   │       └── (Generated migrations)
│   │   │
│   │   ├── DTOs/                              # Data Transfer Objects
│   │   │   ├── Requests/
│   │   │   │   ├── CreateEntityRequest.cs
│   │   │   │   ├── UpdateEntityRequest.cs
│   │   │   │   ├── FilterRequest.cs           # Filter request (FR29-FR32)
│   │   │   │   ├── AssignCategoryRequest.cs   # Category assignment (FR23)
│   │   │   │   ├── CreateCategoryRequest.cs   # Category creation (FR21)
│   │   │   │   ├── UpdateCategoryRequest.cs   # Category update (FR22)
│   │   │   │   ├── UploadDocumentRequest.cs   # Document upload (FR15)
│   │   │   │   └── LinkGeometryRequest.cs    # QGIS link (FR47)
│   │   │   └── Responses/
│   │   │       ├── EntityResponse.cs
│   │   │       ├── CategoryResponse.cs
│   │   │       ├── DocumentResponse.cs
│   │   │       ├── FilterResultResponse.cs    # Filter results (FR35)
│   │   │       ├── ReportResponse.cs           # Report response (FR34, FR76)
│   │   │       └── AnalyticsResponse.cs        # Analytics response (FR67-FR68)
│   │   │
│   │   ├── Mappings/                          # DTO Mapping Extensions
│   │   │   ├── EntityMappings.cs              # Entity ↔ DTO mappings
│   │   │   ├── CategoryMappings.cs            # Category ↔ DTO mappings
│   │   │   └── DocumentMappings.cs            # Document ↔ DTO mappings
│   │   │
│   │   ├── Validators/                        # FluentValidation Validators
│   │   │   ├── CreateEntityValidator.cs
│   │   │   ├── UpdateEntityValidator.cs
│   │   │   ├── FilterRequestValidator.cs
│   │   │   ├── CreateCategoryValidator.cs
│   │   │   ├── AssignCategoryValidator.cs
│   │   │   └── UploadDocumentValidator.cs
│   │   │
│   │   ├── Exceptions/                        # Custom Exceptions
│   │   │   ├── EntityNotFoundException.cs
│   │   │   ├── CategoryNotFoundException.cs
│   │   │   ├── ValidationException.cs
│   │   │   ├── QGISIntegrationException.cs   # QGIS errors (FR45-FR49)
│   │   │   └── PermissionDeniedException.cs   # Permission errors (FR39-FR40)
│   │   │
│   │   ├── Middleware/                        # Custom Middleware
│   │   │   └── GlobalExceptionHandlerMiddleware.cs # Error handling (ADR 5)
│   │   │
│   │   ├── Identity/                          # ASP.NET Core Identity
│   │   │   ├── ApplicationUser.cs             # User model
│   │   │   ├── ApplicationRole.cs             # Role model (FR36-FR37, FR41-FR42)
│   │   │   └── IdentityService.cs              # Identity operations
│   │   │
│   │   ├── Extensions/                        # Extension Methods
│   │   │   ├── ServiceCollectionExtensions.cs # DI configuration
│   │   │   └── ApplicationBuilderExtensions.cs # Middleware configuration
│   │   │
│   │   └── Properties/
│   │       └── launchSettings.json
│   │
│   └── UrbaGIStory.Shared/                    # Shared Models/DTOs
│       ├── UrbaGIStory.Shared.csproj
│       ├── Models/
│       │   ├── EntityType.cs                  # Entity type enum
│       │   ├── CategoryPropertyType.cs         # Property type enum (FR24)
│       │   └── UserRole.cs                    # User role enum (FR36-FR37)
│       └── Constants/
│           └── ApiEndpoints.cs                # API endpoint constants
│
├── tests/                                      # Test Projects
│   ├── UrbaGIStory.Server.Tests/
│   │   ├── UrbaGIStory.Server.Tests.csproj
│   │   ├── Services/
│   │   │   ├── EntityServiceTests.cs
│   │   │   ├── CategoryServiceTests.cs
│   │   │   ├── FilterServiceTests.cs          # Filter logic tests (FR29-FR35)
│   │   │   └── QGISIntegrationServiceTests.cs # QGIS integration tests (FR45-FR49)
│   │   ├── Repositories/
│   │   │   ├── EntityRepositoryTests.cs
│   │   │   └── CategoryRepositoryTests.cs
│   │   ├── Controllers/
│   │   │   ├── EntitiesControllerTests.cs
│   │   │   └── CategoriesControllerTests.cs
│   │   └── Integration/
│   │       ├── EntityIntegrationTests.cs
│   │       └── QGISIntegrationTests.cs        # QGIS integration tests (ADR 1)
│   │
│   └── UrbaGIStory.Client.Tests/
│       ├── UrbaGIStory.Client.Tests.csproj
│       └── Components/
│           ├── MapComponentTests.cs          # Map component tests (FR1-FR5)
│           └── EntityFormTests.cs             # Entity form tests (FR6-FR10)
│
└── docs/                                       # Documentation
    ├── architecture.md                         # This document
    ├── setup-guide.md                          # Setup instructions (FR75)
    ├── api-documentation.md                   # API documentation
    └── deployment.md                           # Deployment guide
```

### Architectural Boundaries

#### API Boundaries

**External API Endpoints:**

- **Entity Management API:**
  - `GET /api/entities` - List entities (FR8, FR12)
  - `GET /api/entities/{id}` - Get entity details (FR8, FR11, FR58)
  - `POST /api/entities` - Create entity (FR6-FR7, FR9)
  - `PUT /api/entities/{id}` - Update entity (FR61)
  - `DELETE /api/entities/{id}` - Soft delete entity (FR62, FR77)
  - `GET /api/entities/{id}/documents` - Get entity documents (FR18-FR19)
  - `POST /api/entities/{id}/documents` - Attach document (FR15)

- **Category Management API:**
  - `GET /api/categories` - List categories (FR20, FR26)
  - `POST /api/categories` - Create category (FR21) - Office Manager only
  - `PUT /api/categories/{id}` - Update category (FR22) - Office Manager only
  - `POST /api/categories/{id}/assign` - Assign to entity type (FR23) - Office Manager only
  - `POST /api/categories/{id}/unassign` - Unassign from entity type (FR70) - Office Manager only

- **Filtering API:**
  - `POST /api/filters/apply` - Apply complex filters (FR29-FR35, ADR 6)
  - `GET /api/filters/entities` - Get filtered entities (FR35)

- **QGIS Integration API:**
  - `GET /api/qgis/geometries` - Read QGIS geometries (FR45-FR46, ADR 1)
  - `POST /api/qgis/geometries/{id}/link` - Link to entity (FR47, FR49)
  - `GET /api/qgis/geometries/{id}/entities` - Get linked entities (FR14)

- **Document Management API:**
  - `GET /api/documents/{id}` - Get document (FR18, FR64)
  - `POST /api/documents` - Upload document (FR15-FR17)
  - `DELETE /api/documents/{id}` - Delete document (FR63)
  - `GET /api/documents/search` - Search by datetime (FR65-FR66)

- **Authentication & Authorization API:**
  - `POST /api/auth/login` - User login
  - `POST /api/auth/register` - Register user (FR36) - Admin only
  - `GET /api/auth/me` - Get current user

- **User Management API:**
  - `GET /api/users` - List users (FR36) - Admin only
  - `POST /api/users` - Create user (FR36) - Admin only
  - `PUT /api/users/{id}` - Update user (FR38) - Admin only
  - `DELETE /api/users/{id}` - Deactivate user (FR71) - Admin only

- **Permission Management API:**
  - `GET /api/permissions/entities/{id}` - Get entity permissions (FR39-FR40)
  - `POST /api/permissions/entities/{id}` - Assign permissions (FR39-FR40) - Office Manager

- **Reporting API:**
  - `GET /api/reports/analytics` - Get analytics (FR67-FR68) - Office Manager
  - `POST /api/reports/generate` - Generate report (FR34, FR76, FR69)

**Internal Service Boundaries:**

- **Service Layer → Repository Layer:** Services use repository interfaces only
- **Controller Layer → Service Layer:** Controllers use service interfaces only
- **Service Layer → Validator Layer:** Services use FluentValidation validators
- **All Layers → Identity:** Authentication/authorization throughout

#### Component Boundaries

**Frontend Component Communication:**

- **Map Component → Entity Components:** Map selection triggers entity detail view (FR3)
- **Entity Form → Category Service:** Dynamic property generation based on categories (FR10, FR25)
- **Filter Panel → Map Component:** Filter updates trigger map refresh (FR35)
- **Document Components → Entity Components:** Documents linked to entities (FR15-FR19)

**State Management Boundaries:**

- **Component-Level State:** Each component manages its own loading/error states
- **API Client Services:** Stateless services, fetch on demand
- **No Global State Management:** Sufficient for MVP scope

#### Data Boundaries

**Database Schema Boundaries:**

- **UrbaGIStory Tables:** `Entity`, `Category`, `Document`, `QGISGeometryLink`, `ApplicationUser`, etc.
- **QGIS Tables:** Separate tables managed by QGIS (ADR 1)
- **Link Table:** `QGISGeometryLink` connects entities to QGIS geometries (FR47)

**Data Access Patterns:**

- **Repository Pattern:** All data access through repository interfaces
- **EF Core DbContext:** Single `AppDbContext` for all UrbaGIStory tables
- **PostGIS Queries:** Spatial queries abstracted in repositories (ADR 6)
- **JSONB Queries:** Dynamic property queries abstracted in repositories (ADR 2, ADR 6)

**Caching Boundaries:**

- **No Caching Initially:** Can be added later if needed
- **Category Cache:** Categories loaded at startup (FR20) - in-memory cache

### Requirements to Structure Mapping

#### Feature/Epic Mapping

**Geographic Visualization & Navigation (FR1-FR5):**
- **Components:** `src/UrbaGIStory.Client/Components/Map/MapComponent.razor`
- **Services:** `src/UrbaGIStory.Client/Services/MapInteropService.cs`
- **JavaScript:** `src/UrbaGIStory.Client/wwwroot/js/map-interop.js`
- **API:** `src/UrbaGIStory.Server/Controllers/QGISController.cs`
- **Repositories:** `src/UrbaGIStory.Server/Repositories/QGISGeometryRepository.cs`

**Entity Management (FR6-FR14, FR61-FR62):**
- **Components:** `src/UrbaGIStory.Client/Components/Entities/EntityForm.razor`, `EntityInfoDisplay.razor`
- **Services:** `src/UrbaGIStory.Client/Services/EntityService.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/EntitiesController.cs`
- **Services:** `src/UrbaGIStory.Server/Services/EntityService.cs`
- **Repositories:** `src/UrbaGIStory.Server/Repositories/EntityRepository.cs`
- **Models:** `src/UrbaGIStory.Server/Models/Entity.cs`
- **Data:** `src/UrbaGIStory.Server/Data/Configurations/EntityConfiguration.cs`

**Category & Template System (FR20-FR28, FR70):**
- **Components:** `src/UrbaGIStory.Client/Components/Categories/CategoryForm.razor`, `CategoryAssignmentPanel.razor`, `PropertyDefinitionEditor.razor`
- **Services:** `src/UrbaGIStory.Client/Services/CategoryService.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/CategoriesController.cs`
- **Services:** `src/UrbaGIStory.Server/Services/CategoryService.cs` (dynamic property generation - ADR 4)
- **Repositories:** `src/UrbaGIStory.Server/Repositories/CategoryRepository.cs`
- **Models:** `src/UrbaGIStory.Server/Models/Category.cs`, `CategoryProperty.cs`

**Document Management (FR15-FR19, FR63-FR66):**
- **Components:** `src/UrbaGIStory.Client/Components/Documents/DocumentList.razor`, `DocumentUpload.razor`, `DocumentMetadataEditor.razor`
- **Services:** `src/UrbaGIStory.Client/Services/DocumentService.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/DocumentsController.cs`
- **Services:** `src/UrbaGIStory.Server/Services/DocumentService.cs`
- **Repositories:** `src/UrbaGIStory.Server/Repositories/DocumentRepository.cs`
- **Models:** `src/UrbaGIStory.Server/Models/Document.cs`

**Filtering & Analysis (FR29-FR35, FR76):**
- **Components:** `src/UrbaGIStory.Client/Components/Filters/FilterPanel.razor`, `FilterResultDisplay.razor`
- **Services:** `src/UrbaGIStory.Client/Services/FilterService.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/FiltersController.cs`
- **Services:** `src/UrbaGIStory.Server/Services/FilterService.cs` (hybrid PostGIS + JSONB - ADR 6)
- **Repositories:** Uses `EntityRepository` and `CategoryRepository` for filtering

**QGIS Integration (FR45-FR49):**
- **Services:** `src/UrbaGIStory.Server/Services/QGISIntegrationService.cs` (ADR 1)
- **Repositories:** `src/UrbaGIStory.Server/Repositories/QGISGeometryRepository.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/QGISController.cs`
- **Models:** `src/UrbaGIStory.Server/Models/QGISGeometryLink.cs`
- **Data:** `src/UrbaGIStory.Server/Data/Configurations/QGISGeometryLinkConfiguration.cs`

**User & Permission Management (FR36-FR44, FR71):**
- **Components:** `src/UrbaGIStory.Client/Components/Pages/UserManagement.razor`, `PermissionManagement.razor`
- **Services:** `src/UrbaGIStory.Server/Services/UserService.cs`, `PermissionService.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/UsersController.cs`, `PermissionsController.cs`
- **Identity:** `src/UrbaGIStory.Server/Identity/ApplicationUser.cs`, `ApplicationRole.cs`

**Reporting & Analytics (FR67-FR68, FR34, FR69, FR76):**
- **Components:** `src/UrbaGIStory.Client/Components/Reports/AnalyticsDashboard.razor`, `ReportGenerator.razor`
- **Services:** `src/UrbaGIStory.Server/Services/ReportService.cs`
- **API:** `src/UrbaGIStory.Server/Controllers/ReportsController.cs`

#### Cross-Cutting Concerns

**Authentication & Authorization:**
- **Identity:** `src/UrbaGIStory.Server/Identity/`
- **Controllers:** All controllers use `[Authorize]` attributes
- **Services:** Permission checks in services (FR39-FR40, FR41-FR42)
- **Frontend:** `src/UrbaGIStory.Client/Services/AuthService.cs`

**Error Handling:**
- **Middleware:** `src/UrbaGIStory.Server/Middleware/GlobalExceptionHandlerMiddleware.cs`
- **Exceptions:** `src/UrbaGIStory.Server/Exceptions/`
- **Frontend:** `src/UrbaGIStory.Client/Components/Shared/ErrorDisplay.razor`

**Validation:**
- **Validators:** `src/UrbaGIStory.Server/Validators/` (FluentValidation)
- **Services:** Validation called in services before business logic

**Soft Delete Strategy (FR77):**
- **Models:** All entities have `IsDeleted` or `DeletedAt` property
- **Repositories:** Filter out deleted items in queries
- **Services:** Soft delete logic in services

**Concurrency Handling (FR43-FR44, FR60, ADR 5):**
- **Models:** `Entity` has `RowVersion` or `UpdatedAt` for optimistic concurrency
- **Services:** Version checking in `EntityService.UpdateAsync()`
- **Frontend:** Conflict resolution UI in `EntityForm.razor`

### Integration Points

#### Internal Communication

**Frontend → Backend:**
- **HTTP API Calls:** Blazor WASM uses `HttpClient` to call Web API endpoints
- **Authentication:** JWT tokens in Authorization header
- **Error Handling:** ProblemDetails responses converted to user-friendly errors

**Backend Service Communication:**
- **Dependency Injection:** All services registered in `Program.cs`
- **Service → Repository:** Services inject repository interfaces
- **Service → Service:** Services can inject other services (e.g., `FilterService` uses `EntityRepository` and `CategoryRepository`)

**Component Communication (Frontend):**
- **Parent → Child:** Component parameters
- **Child → Parent:** EventCallbacks
- **Sibling Components:** Shared service or parent component state

#### External Integrations

**QGIS Integration (FR45-FR49, ADR 1):**
- **Data Layer:** Read-only access to QGIS PostGIS tables
- **Link Mechanism:** `QGISGeometryLink` table with foreign key to QGIS geometry table
- **Service:** `QGISIntegrationService` validates and manages links
- **No Direct QGIS API:** Integration is data-only through PostgreSQL

**PostgreSQL/PostGIS:**
- **Connection:** Connection string in `appsettings.json`
- **EF Core:** `AppDbContext` configured with PostGIS extension
- **Spatial Queries:** NetTopologySuite for C# geometries, PostGIS for database queries

#### Data Flow

**Entity Creation Flow (FR6-FR7, FR9):**
1. User selects geometry on map (FR3) → `MapComponent.razor`
2. User fills entity form → `EntityForm.razor`
3. Form calls `EntityService.CreateAsync()` → `EntityService.cs` (Client)
4. HTTP POST to `/api/entities` → `EntitiesController.cs`
5. Controller calls `EntityService.CreateAsync()` → `EntityService.cs` (Server)
6. Service validates → `CreateEntityValidator.cs`
7. Service calls `EntityRepository.AddAsync()` → `EntityRepository.cs`
8. Repository saves to database via `AppDbContext`
9. Response returned to client
10. Map updates to show new entity (FR5)

**Dynamic Property Display Flow (FR10, FR25):**
1. User selects entity → `EntityForm.razor`
2. Component calls `CategoryService.GetCategoriesForEntityTypeAsync()` → `CategoryService.cs` (Client)
3. HTTP GET to `/api/categories?entityType={type}` → `CategoriesController.cs`
4. Controller calls `CategoryService.GetByEntityTypeAsync()` → `CategoryService.cs` (Server)
5. Service queries categories assigned to entity type (FR23)
6. Service generates property definitions from categories (FR24, ADR 4)
7. Response includes property definitions
8. `EntityPropertyPanel.razor` dynamically renders MudBlazor components based on properties
9. User fills properties (FR27)
10. Properties saved as JSONB in `Entity.DynamicProperties` (ADR 2)

**Filtering Flow (FR29-FR35, ADR 6):**
1. User sets filter criteria → `FilterPanel.razor`
2. Component calls `FilterService.ApplyFiltersAsync()` → `FilterService.cs` (Client)
3. HTTP POST to `/api/filters/apply` → `FiltersController.cs`
4. Controller calls `FilterService.ApplyFiltersAsync()` → `FilterService.cs` (Server)
5. Service builds hybrid query (PostGIS spatial + JSONB property filters)
6. Service calls `EntityRepository.GetFilteredAsync()` → `EntityRepository.cs`
7. Repository executes hybrid query using GIST (spatial) and GIN (JSONB) indexes
8. Results returned as GeoJSON
9. Map updates to show filtered entities (FR35)

### File Organization Patterns

#### Configuration Files

**Root Configuration:**
- `UrbaGIStory.sln` - Solution file
- `.gitignore` - Git ignore rules
- `.editorconfig` - Editor configuration
- `README.md` - Project documentation

**Server Configuration:**
- `src/UrbaGIStory.Server/appsettings.json` - Production settings
- `src/UrbaGIStory.Server/appsettings.Development.json` - Development settings
- `src/UrbaGIStory.Server/Properties/launchSettings.json` - Launch profiles

**Client Configuration:**
- `src/UrbaGIStory.Client/Properties/launchSettings.json` - Client launch settings

#### Source Organization

**Backend Organization:**
- **By Layer:** Controllers → Services → Repositories → Models → Data
- **By Feature:** Within each layer, files organized by domain (Entity, Category, Document, etc.)
- **Shared:** DTOs, Mappings, Validators, Exceptions, Middleware in root of Server project

**Frontend Organization:**
- **By Feature:** Components organized by feature (Entities, Categories, Map, Filters, etc.)
- **By Type:** Pages, Components, Services, Models in separate folders
- **Shared:** Shared components in `Components/Shared/`

#### Test Organization

**Test Structure:**
- **Mirrors Source:** Test projects mirror source project structure
- **By Layer:** Tests organized by layer (Services, Repositories, Controllers)
- **Integration Tests:** Separate folder for integration tests
- **Test Utilities:** Shared test utilities in test projects

#### Asset Organization

**Static Assets:**
- **JavaScript:** `wwwroot/js/` - Custom JS files (map-interop.js)
- **CSS:** `wwwroot/css/` - Custom styles
- **Libraries:** `wwwroot/lib/` - Third-party libraries (OpenLayers)
- **Images:** `wwwroot/images/` - Static images (if needed)

**Document Storage:**
- **Server-Side:** Documents stored in file system or database (decision needed)
- **Path:** Configured in `appsettings.json`

### Development Workflow Integration

**Development Server Structure:**
- **Client:** Blazor WASM runs on `https://localhost:5001` (or configured port)
- **Server:** Web API runs on `https://localhost:7001` (or configured port)
- **CORS:** Configured to allow client → server communication
- **Hot Reload:** Enabled for both client and server during development

**Build Process Structure:**
- **Client Build:** Blazor WASM compiled to WebAssembly, served statically
- **Server Build:** .NET Web API compiled to DLL
- **Shared:** Shared project compiled into both client and server

### Deployment Architecture

**Two Separate Applications on Server:**

**Application 1: Frontend (Blazor WebAssembly)**
- **Type:** Static web application (HTML, CSS, JS, WASM files)
- **Served by:** Web server (IIS, Nginx, or simple static file server)
- **User Access:** Users access this application via URL (e.g., `http://localhost` or `http://server-ip`)
- **Port:** Typically port 80 (HTTP) or 443 (HTTPS)
- **What it does:**
  - Serves the Blazor WASM application
  - Users log in and interact with the UI
  - Makes HTTP requests to the Backend API

**Application 2: Backend (ASP.NET Core Web API)**
- **Type:** .NET Web API service
- **Hosted as:** Windows Service, IIS application, or standalone service
- **User Access:** NOT directly accessed by users (only by Frontend)
- **Port:** Different port (e.g., `http://localhost:5000` or `http://server-ip:5000`)
- **What it does:**
  - Provides REST API endpoints
  - Handles business logic
  - Accesses PostgreSQL/PostGIS database
  - Manages authentication/authorization

**Communication Flow:**
```
User Browser
    ↓ (HTTP request)
Frontend (Blazor WASM) - http://localhost (port 80)
    ↓ (HTTP API call)
Backend (Web API) - http://localhost:5000
    ↓ (Database query)
PostgreSQL/PostGIS Database
```

**Deployment Structure:**
- **Frontend Deployment:**
  - Blazor WASM compiled to static files (`wwwroot/` folder)
  - Files deployed to web server directory
  - Served as static website
  - Users access via main URL (e.g., `http://server-ip`)

- **Backend Deployment:**
  - .NET Web API compiled to DLL
  - Hosted as Windows Service or IIS application
  - Runs on different port (not directly accessible to users)
  - Only Frontend makes requests to it

- **Configuration:**
  - Frontend configured with Backend API URL in `appsettings.json`
  - CORS configured on Backend to allow Frontend origin
  - Both applications can run on same server or different servers

- **Database:**
  - PostgreSQL/PostGIS on same server or separate server
  - Backend connects to database via connection string

## Architecture Validation Results

### Coherence Validation ✅

**Decision Compatibility:**

All architectural decisions are compatible and work together:

- ✅ **Stack Compatibility:** .NET 10 + Blazor WASM + PostgreSQL/PostGIS + OpenLayers - All technologies are compatible and work together
- ✅ **Version Compatibility:** All package versions specified are compatible (.NET 10, Npgsql 9.0.x, NetTopologySuite 2.5.x)
- ✅ **Pattern Alignment:** Repository pattern aligns with EF Core, Service layer aligns with Repository pattern, Controller layer aligns with Service layer
- ✅ **No Contradictions:** All decisions support each other (e.g., JSONB model supports dynamic categories, Repository pattern supports complex queries)

**Pattern Consistency:**

Implementation patterns consistently support architectural decisions:

- ✅ **Naming Conventions:** Consistent across all layers (PascalCase for C#, camelCase for JSON/JavaScript)
- ✅ **Structure Patterns:** Folder organization aligns with layered architecture decision
- ✅ **Communication Patterns:** JS Interop pattern aligns with OpenLayers integration (ADR 3)
- ✅ **Process Patterns:** Error handling and validation patterns align with service layer architecture

**Structure Alignment:**

Project structure fully supports all architectural decisions:

- ✅ **Layer Separation:** Clear separation between Controllers → Services → Repositories → Data
- ✅ **Boundary Respect:** API boundaries, component boundaries, and data boundaries are clearly defined
- ✅ **Pattern Enablement:** Structure enables Repository pattern, Service layer, and JS Interop patterns
- ✅ **Integration Points:** All integration points (Frontend ↔ Backend, Backend ↔ Database, QGIS integration) are properly structured

### Requirements Coverage Validation ✅

**Functional Requirements Coverage:**

All 77 functional requirements are architecturally supported:

- ✅ **Geographic Visualization (FR1-FR5):** MapComponent.razor, MapInteropService, OpenLayers integration
- ✅ **Entity Management (FR6-FR14, FR61-FR62):** EntityService, EntityRepository, Entity model with optional geometry link
- ✅ **Document Management (FR15-FR19, FR63-FR66):** DocumentService, DocumentRepository, temporal metadata support
- ✅ **Category System (FR20-FR28, FR70):** CategoryService with dynamic property generation (ADR 4), CategoryRepository
- ✅ **Filtering & Analysis (FR29-FR35, FR76):** FilterService with hybrid PostGIS + JSONB queries (ADR 6)
- ✅ **User & Permissions (FR36-FR44, FR71):** ASP.NET Core Identity + JWT, PermissionService, role-based authorization
- ✅ **QGIS Integration (FR45-FR49):** QGISIntegrationService, separate tables architecture (ADR 1)
- ✅ **System Administration (FR50-FR56, FR72-FR75):** Configuration endpoints, backup/restore capabilities
- ✅ **Data Linking (FR57-FR60, FR77):** Entity model supports multiple specialists, soft delete strategy, optimistic concurrency (ADR 5)
- ✅ **Reporting (FR67-FR68, FR69):** ReportService, AnalyticsDashboard component

**Non-Functional Requirements Coverage:**

All 18 NFRs are architecturally addressed:

- ✅ **Performance (NFR1-NFR6):**
  - Entity selection < 1s: Optimized repository queries, indexed database
  - Map load < 3s: Direct GeoJSON loading from API (ADR 3), lazy loading
  - Filter < 2s: Hybrid query with GIST/GIN indexes (ADR 6)
  - Document operations: File upload optimization, streaming
  - Concurrent users: Optimistic concurrency (ADR 5), no blocking locks
  - Graceful degradation: Loading indicators, async operations

- ✅ **Security (NFR7-NFR11):**
  - Document protection: Permission-based access (PermissionService)
  - Unauthorized access: ASP.NET Core Identity + JWT, [Authorize] attributes
  - Data integrity: Optimistic concurrency, soft delete, audit trails
  - Authentication: Identity with secure password hashing
  - Database security: Connection string security, restricted access

- ✅ **Integration (NFR12-NFR14):**
  - QGIS reliability: Separate tables architecture (ADR 1), graceful error handling
  - PostGIS integrity: EF Core with PostGIS, proper indexing
  - Error handling: QGISIntegrationException, logging

- ✅ **Reliability (NFR15-NFR18):**
  - 24/7 availability: Service hosting, error recovery
  - Backup/recovery: Database backup endpoints (FR72-FR73)
  - Monitoring: Logging infrastructure, performance monitoring
  - Data persistence: EF Core transactions, reliable database operations

### Implementation Readiness Validation ✅

**Decision Completeness:**

All critical decisions are documented with sufficient detail:

- ✅ **Technology Versions:** All packages have versions specified (Npgsql 9.0.x, NetTopologySuite 2.5.x, etc.)
- ✅ **Architecture Decisions:** 6 ADRs documented with trade-offs and rationale
- ✅ **Backend Decisions:** All 11 backend decisions documented (architecture pattern, repository, authentication, validation, etc.)
- ✅ **Deployment Architecture:** Two-application deployment clearly documented

**Structure Completeness:**

Project structure is complete and specific:

- ✅ **Complete Directory Tree:** All files and directories defined with specific names
- ✅ **File Organization:** Every component, service, repository, and model has a defined location
- ✅ **Integration Points:** All integration points (Frontend ↔ Backend, QGIS, Database) clearly specified
- ✅ **Component Boundaries:** Clear boundaries between all components

**Pattern Completeness:**

Implementation patterns are comprehensive:

- ✅ **Naming Patterns:** Conventions defined for database, API, C# code, JavaScript, and Blazor components
- ✅ **Structure Patterns:** Project organization, file structure, test organization all defined
- ✅ **Format Patterns:** API responses, JSON formats, date formats, GeoJSON format all specified
- ✅ **Communication Patterns:** JS Interop patterns, state management, error handling all documented
- ✅ **Process Patterns:** Error handling, loading states, validation all specified
- ✅ **Examples Provided:** Good examples and anti-patterns provided for each pattern category

### Gap Analysis Results

**Critical Gaps:** None identified

All critical architectural decisions are complete and documented.

**Important Gaps (Non-Blocking):**

1. **Document Storage Strategy:**
   - **Gap:** Decision needed: File system vs database storage for documents
   - **Impact:** Affects DocumentService and DocumentRepository implementation
   - **Recommendation:** Decide during first document management story implementation
   - **Location:** DocumentService.cs, DocumentRepository.cs

2. **Backup/Restore Implementation Details:**
   - **Gap:** Specific backup/restore procedures not detailed
   - **Impact:** FR72-FR73 implementation needs more detail
   - **Recommendation:** Can be detailed during system administration story
   - **Location:** System administration endpoints

3. **Logging Strategy Details:**
   - **Gap:** Specific logging framework and log levels not specified
   - **Impact:** FR74 (system logs) needs logging configuration
   - **Recommendation:** Use standard .NET logging (ILogger), can be configured in Program.cs
   - **Location:** Program.cs, all services

**Nice-to-Have Gaps:**

1. **API Rate Limiting:**
   - Not needed for MVP (local deployment, limited users)
   - Can be added later if needed

2. **Caching Strategy:**
   - Categories loaded at startup (FR20) - in-memory cache sufficient
   - Can add Redis or other caching later if needed

3. **Monitoring and Observability:**
   - Basic logging sufficient for MVP
   - Can add Application Insights or similar later

### Validation Issues Addressed

**No Critical Issues Found**

The architecture is coherent, complete, and ready for implementation. All requirements are supported, and all decisions work together.

**Minor Clarifications Made:**

1. **Deployment Architecture Clarified:**
   - Updated to clearly document two separate applications (Frontend and Backend)
   - Users access Frontend, Frontend calls Backend API
   - Both applications run on server but on different ports

### Architecture Completeness Checklist

**✅ Requirements Analysis**

- [x] Project context thoroughly analyzed
- [x] Scale and complexity assessed (High complexity, 8-12 components)
- [x] Technical constraints identified (QGIS integration, local deployment, desktop-first)
- [x] Cross-cutting concerns mapped (6 major concerns identified)

**✅ Architectural Decisions**

- [x] Critical decisions documented with versions (6 ADRs + 11 backend decisions)
- [x] Technology stack fully specified (.NET 10, Blazor WASM, PostgreSQL/PostGIS, OpenLayers, MudBlazor)
- [x] Integration patterns defined (QGIS integration, JS Interop, Frontend-Backend communication)
- [x] Performance considerations addressed (Indexing strategy, query optimization, loading patterns)

**✅ Implementation Patterns**

- [x] Naming conventions established (Database, API, C#, JavaScript, Blazor)
- [x] Structure patterns defined (Project organization, file structure, test organization)
- [x] Communication patterns specified (JS Interop, API communication, state management)
- [x] Process patterns documented (Error handling, loading states, validation)

**✅ Project Structure**

- [x] Complete directory structure defined (All files and folders specified)
- [x] Component boundaries established (API, component, service, data boundaries)
- [x] Integration points mapped (Frontend-Backend, QGIS, Database)
- [x] Requirements to structure mapping complete (All FRs mapped to specific files/components)

**✅ Deployment Architecture**

- [x] Two-application deployment clearly documented
- [x] Frontend and Backend separation explained
- [x] Communication flow between applications defined
- [x] Port configuration and access patterns specified

### Architecture Readiness Assessment

**Overall Status:** ✅ **READY FOR IMPLEMENTATION**

**Confidence Level:** **High** - Architecture is complete, coherent, and all requirements are supported.

**Key Strengths:**

1. **Comprehensive ADRs:** 6 Architecture Decision Records with explicit trade-offs and rationale
2. **Complete Structure:** Detailed project structure with all files and directories specified
3. **Clear Patterns:** Comprehensive implementation patterns prevent AI agent conflicts
4. **Requirements Coverage:** All 77 FRs and 18 NFRs architecturally supported
5. **Technology Alignment:** All technology choices align with project requirements
6. **Integration Clarity:** QGIS integration, Frontend-Backend communication, and Database access all clearly defined
7. **Deployment Clarity:** Two-application deployment architecture clearly documented

**Areas for Future Enhancement:**

1. **Document Storage:** Decide file system vs database during implementation
2. **Advanced Monitoring:** Add detailed observability tools post-MVP
3. **Caching Layer:** Add Redis or similar if performance needs increase
4. **API Versioning:** Add versioning if external APIs are needed in future
5. **Rate Limiting:** Add if user base grows significantly

### Implementation Handoff

**AI Agent Guidelines:**

When implementing stories, AI agents MUST:

1. **Follow Architectural Decisions Exactly:**
   - Use Repository pattern for all data access
   - Use Service layer for all business logic
   - Follow ADR decisions (QGIS integration, JSONB model, optimistic concurrency, etc.)
   - Respect deployment architecture (two separate applications)

2. **Use Implementation Patterns Consistently:**
   - Follow naming conventions (PascalCase for C#, camelCase for JSON)
   - Place files in correct directories per structure defined
   - Use standard formats (ProblemDetails for errors, ISO 8601 for dates)
   - Follow JS Interop patterns for OpenLayers integration

3. **Respect Project Structure and Boundaries:**
   - Place components in correct feature folders
   - Use defined service and repository interfaces
   - Follow API endpoint naming conventions
   - Maintain separation of concerns (Controller → Service → Repository)

4. **Refer to This Document:**
   - Check ADRs before making data model decisions
   - Reference patterns when implementing new features
   - Follow structure when creating new files
   - Use examples as templates for similar implementations

**First Implementation Priority:**

1. **Project Initialization:**
   ```bash
   # Create solution and projects (from Starter Template section)
   dotnet new sln -n UrbaGIStory
   dotnet new blazorwasm -n UrbaGIStory.Client -o src/UrbaGIStory.Client
   dotnet new webapi -n UrbaGIStory.Server -o src/UrbaGIStory.Server
   dotnet new classlib -n UrbaGIStory.Shared -o src/UrbaGIStory.Shared
   # ... (complete initialization commands from document)
   ```

2. **Technical Spike (Recommended):**
   - Validate JS Interop pattern with OpenLayers
   - Test PostGIS integration with NetTopologySuite
   - Verify dynamic property generation pattern

3. **Core Infrastructure:**
   - Set up AppDbContext with PostGIS
   - Configure authentication (Identity + JWT)
   - Set up basic repository and service patterns
   - Configure CORS for Frontend-Backend communication

**Architecture Document Status:** ✅ **COMPLETE**

This architecture document provides comprehensive guidance for consistent AI agent implementation. All critical decisions are documented, all requirements are supported, and all patterns are defined.

## Architecture Completion Summary

### Workflow Completion

**Architecture Decision Workflow:** COMPLETED ✅
**Total Steps Completed:** 8
**Date Completed:** 2025-12-25
**Document Location:** _bmad-output/architecture.md

### Final Architecture Deliverables

**📋 Complete Architecture Document**

- All architectural decisions documented with specific versions
- Implementation patterns ensuring AI agent consistency
- Complete project structure with all files and directories
- Requirements to architecture mapping
- Validation confirming coherence and completeness

**🏗️ Implementation Ready Foundation**

- **17 architectural decisions** made (6 ADRs + 11 backend decisions)
- **5 pattern categories** defined (Naming, Structure, Format, Communication, Process)
- **8-12 architectural components** specified
- **95 requirements** fully supported (77 FRs + 18 NFRs)

**📚 AI Agent Implementation Guide**

- Technology stack with verified versions (.NET 10, Blazor WASM, PostgreSQL/PostGIS, OpenLayers, MudBlazor)
- Consistency rules that prevent implementation conflicts
- Project structure with clear boundaries
- Integration patterns and communication standards

### Implementation Handoff

**For AI Agents:**
This architecture document is your complete guide for implementing UrbaGIStory. Follow all decisions, patterns, and structures exactly as documented.

**First Implementation Priority:**

1. **Project Initialization:**
   ```bash
   # Create solution and projects
   dotnet new sln -n UrbaGIStory
   dotnet new blazorwasm -n UrbaGIStory.Client -o src/UrbaGIStory.Client
   dotnet new webapi -n UrbaGIStory.Server -o src/UrbaGIStory.Server
   dotnet new classlib -n UrbaGIStory.Shared -o src/UrbaGIStory.Shared
   dotnet sln add src/UrbaGIStory.Client/UrbaGIStory.Client.csproj
   dotnet sln add src/UrbaGIStory.Server/UrbaGIStory.Server.csproj
   dotnet sln add src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj
   dotnet add src/UrbaGIStory.Client/UrbaGIStory.Client.csproj reference src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj
   dotnet add src/UrbaGIStory.Server/UrbaGIStory.Server.csproj reference src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj
   ```

2. **Technical Spike (Recommended):**
   - Validate JS Interop pattern with OpenLayers
   - Test PostGIS integration with NetTopologySuite
   - Verify dynamic property generation pattern

3. **Core Infrastructure:**
   - Set up AppDbContext with PostGIS
   - Configure authentication (Identity + JWT)
   - Set up basic repository and service patterns
   - Configure CORS for Frontend-Backend communication

**Development Sequence:**

1. Initialize project using documented starter template
2. Set up development environment per architecture
3. Implement core architectural foundations
4. Build features following established patterns
5. Maintain consistency with documented rules

### Quality Assurance Checklist

**✅ Architecture Coherence**

- [x] All decisions work together without conflicts
- [x] Technology choices are compatible
- [x] Patterns support the architectural decisions
- [x] Structure aligns with all choices

**✅ Requirements Coverage**

- [x] All functional requirements are supported
- [x] All non-functional requirements are addressed
- [x] Cross-cutting concerns are handled
- [x] Integration points are defined

**✅ Implementation Readiness**

- [x] Decisions are specific and actionable
- [x] Patterns prevent agent conflicts
- [x] Structure is complete and unambiguous
- [x] Examples are provided for clarity

**✅ Deployment Architecture**

- [x] Two-application deployment clearly documented
- [x] Frontend and Backend separation explained
- [x] Client requirements clarified (no installation needed)
- [x] Communication flow between applications defined

### Project Success Factors

**🎯 Clear Decision Framework**
Every technology choice was made collaboratively with clear rationale, ensuring all stakeholders understand the architectural direction.

**🔧 Consistency Guarantee**
Implementation patterns and rules ensure that multiple AI agents will produce compatible, consistent code that works together seamlessly.

**📋 Complete Coverage**
All project requirements are architecturally supported, with clear mapping from business needs to technical implementation.

**🏗️ Solid Foundation**
The chosen architecture and patterns provide a production-ready foundation following current best practices for .NET 10, Blazor WebAssembly, and PostgreSQL/PostGIS.

---

**Architecture Status:** READY FOR IMPLEMENTATION ✅

**Next Phase:** Begin implementation using the architectural decisions and patterns documented herein.

**Document Maintenance:** Update this architecture when major technical decisions are made during implementation.

**Example Deployment:**
```
Server (localhost or server-ip)
├── Frontend Application
│   ├── Served on: http://localhost (port 80) - USER ACCESS POINT
│   ├── Files: index.html, _framework/, wwwroot/
│   └── Users access this URL to use the application
│
├── Backend Application
│   ├── Running on: http://localhost:5000 (port 5000) - INTERNAL ONLY
│   ├── Type: .NET Web API service
│   └── Frontend makes API calls to this
│
└── PostgreSQL/PostGIS Database
    ├── Running on: localhost:5432
    └── Backend connects to this
```

**User Experience:**
1. User opens browser → goes to `http://server-ip` (Frontend)
2. Frontend loads → user sees login page
3. User logs in → Frontend calls `http://server-ip:5000/api/auth/login` (Backend)
4. User interacts with UI → Frontend makes API calls to Backend
5. Backend processes requests → queries database → returns responses
6. Frontend updates UI based on responses

**Client Requirements:**
- **NO INSTALLATION REQUIRED** on client computers
- Users only need:
  - Modern web browser (Chrome, Firefox, Edge, Safari - latest 2 versions)
  - Network access to server (localhost/internal network)
  - WebAssembly support (all modern browsers support this)
- **Everything runs in the browser:**
  - Blazor WebAssembly application runs entirely in the user's browser
  - No client-side installation or configuration needed
  - No plugins or extensions required
  - Application is accessed via URL, works like any web application

