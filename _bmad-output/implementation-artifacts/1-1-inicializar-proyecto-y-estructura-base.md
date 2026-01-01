# Story 1.1: Inicializar proyecto y estructura base

Status: done

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a technical administrator,
I want to initialize the project with the correct structure and dependencies,
So that I have a solid foundation for building the application.

**FRs covered:** FR51, FR52

## Acceptance Criteria

**Given** I have .NET 10 SDK installed
**When** I execute the project initialization commands from the Architecture document
**Then** A solution is created with three projects: Client (Blazor WASM), Server (Web API), and Shared
**And** All required NuGet packages are installed (MudBlazor, Npgsql.EntityFrameworkCore.PostgreSQL v9.0.x, NetTopologySuite v2.5.x, etc.)
**And** The folder structure matches the Architecture document (Controllers/, Services/, Repositories/, Models/, Data/, DTOs/, etc.)
**And** Basic Program.cs files are configured for both Client and Server projects
**And** The Shared project contains common models and DTOs

**Given** The project structure is initialized
**When** I build the solution
**Then** The solution builds without errors
**And** All projects reference each other correctly

## Tasks / Subtasks

- [x] Task 1: Create solution and project structure (AC: 1)
  - [x] Create solution file: `dotnet new sln -n UrbaGIStory`
  - [x] Create Blazor WebAssembly project: `dotnet new blazorwasm -n UrbaGIStory.Client -o src/UrbaGIStory.Client`
  - [x] Create Web API project: `dotnet new webapi -n UrbaGIStory.Server -o src/UrbaGIStory.Server`
  - [x] Create shared library: `dotnet new classlib -n UrbaGIStory.Shared -o src/UrbaGIStory.Shared`
  - [x] Add all projects to solution
  - [x] Add project references (Client → Shared, Server → Shared)
  - [x] Verify solution structure matches Architecture document

- [x] Task 2: Install required NuGet packages (AC: 1)
  - [x] Client project packages:
    - [x] MudBlazor (latest stable - v8.15.0)
    - [x] Any other Blazor WASM dependencies
  - [x] Server project packages:
    - [x] Npgsql.EntityFrameworkCore.PostgreSQL v10.0.0 (updated from v9.0.x for .NET 10 compatibility)
    - [x] Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite v10.0.0 (updated from v9.0.x)
    - [x] NetTopologySuite v2.6.0 (updated from v2.5.x for Npgsql 10.0.0 compatibility)
    - [x] NetTopologySuite.IO.GeoJSON v4.0.0
    - [x] FluentValidation.AspNetCore v11.3.1 (latest stable)
    - [x] Swashbuckle.AspNetCore v10.1.0 (latest stable)
    - [x] Microsoft.AspNetCore.Identity.EntityFrameworkCore v10.0.1
  - [x] Shared project packages:
    - [x] No packages needed yet (will be added in future stories if needed)
  - [x] Verify all packages are compatible with .NET 10

- [x] Task 3: Create folder structure for Server project (AC: 1)
  - [x] Create Controllers/ folder
  - [x] Create Services/ folder
  - [x] Create Repositories/ folder
  - [x] Create Models/ folder
  - [x] Create Data/ folder with Configurations/ subfolder
  - [x] Create DTOs/ folder with Requests/ and Responses/ subfolders
  - [x] Create Mappings/ folder
  - [x] Create Validators/ folder
  - [x] Create Exceptions/ folder
  - [x] Create Middleware/ folder
  - [x] Create Identity/ folder
  - [x] Create Extensions/ folder
  - [x] Verify structure matches Architecture document exactly

- [x] Task 4: Create folder structure for Client project (AC: 1)
  - [x] Create Components/ folder (for MudBlazor components)
  - [x] Create Services/ folder (for API clients, JS Interop)
  - [x] Create wwwroot/js/ folder structure
  - [x] Create wwwroot/js/map-interop.js placeholder file
  - [x] Verify structure matches Architecture document

- [x] Task 5: Configure basic Program.cs files (AC: 1)
  - [x] Configure Client Program.cs:
    - [x] Add MudBlazor services
    - [x] Configure HttpClient for API communication
    - [x] Set up basic Blazor WASM configuration
  - [x] Configure Server Program.cs:
    - [x] Set up basic Web API configuration
    - [x] Configure CORS for Blazor WASM client (placeholder - will be configured in Story 1.3)
    - [x] Set up basic services container
  - [x] Verify both Program.cs files compile

- [x] Task 6: Verify solution builds (AC: 2)
  - [x] Run `dotnet build` on solution
  - [x] Fix any compilation errors
  - [x] Verify all project references are correct
  - [x] Verify no missing dependencies
  - [x] Confirm solution builds without errors

## Dev Notes

### Epic Context

This is the first story in **Epic 1: Project Foundation & System Setup**. This epic establishes the technical foundation of the project and enables initial system configuration. The epic includes project initialization, database setup, QGIS integration configuration, system administration capabilities, and setup documentation.

**Epic Objectives:**
- Establish technical foundation
- Enable system configuration
- Set up database and PostGIS
- Configure authentication infrastructure
- Set up error handling and API documentation

**Related Stories in Epic 1:**
- Story 1.2: Configurar base de datos y PostGIS (depends on this story)
- Story 1.3: Configurar autenticación y autorización base (depends on this story)
- Story 1.4: Implementar manejo de errores global (depends on this story)
- Story 1.5: Configurar documentación API (Swagger) (depends on this story)

### Technical Requirements

**Technology Stack (from Architecture):**
- .NET 10 (C# 12)
- Blazor WebAssembly (Frontend)
- ASP.NET Core Web API (Backend)
- PostgreSQL + PostGIS (Database - configured in Story 1.2)
- MudBlazor (UI Framework)
- OpenLayers (via JS Interop - configured in Epic 3)

**Critical Version Constraints (from Project Context):**
- Npgsql.EntityFrameworkCore.PostgreSQL v9.0.x is REQUIRED for .NET 10 compatibility
- NetTopologySuite v2.5.x is REQUIRED for PostGIS spatial operations
- All packages MUST be compatible with .NET 10
- Do NOT use older versions - they are incompatible

**Project Structure Requirements:**

The solution must follow this exact structure:

```text
UrbaGIStory/
├── src/
│   ├── UrbaGIStory.Client/          # Blazor WebAssembly
│   │   ├── Components/              # MudBlazor components
│   │   ├── Services/                # API clients, JS Interop
│   │   ├── wwwroot/
│   │   │   └── js/
│   │   │       └── map-interop.js   # OpenLayers integration (placeholder for now)
│   │   └── UrbaGIStory.Client.csproj
│   │
│   ├── UrbaGIStory.Server/          # .NET Web API
│   │   ├── Controllers/             # API endpoints
│   │   ├── Services/                # Business logic
│   │   ├── Repositories/            # Data access
│   │   ├── Models/                  # Domain entities
│   │   ├── Data/                    # EF Core configuration
│   │   │   └── Configurations/     # Fluent API configurations
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   │   ├── Requests/
│   │   │   └── Responses/
│   │   ├── Mappings/                # Extension methods for mapping
│   │   ├── Validators/              # FluentValidation validators
│   │   ├── Exceptions/              # Custom exceptions
│   │   ├── Middleware/              # Custom middleware
│   │   ├── Identity/                # ASP.NET Core Identity
│   │   ├── Extensions/              # Extension methods
│   │   └── UrbaGIStory.Server.csproj
│   │
│   └── UrbaGIStory.Shared/          # Shared models/DTOs
│       └── UrbaGIStory.Shared.csproj
│
├── tests/                           # (Optional for this story - can be created later)
│   ├── UrbaGIStory.Server.Tests/
│   └── UrbaGIStory.Client.Tests/
│
└── UrbaGIStory.sln
```

**Initialization Commands (from Architecture):**

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

**NuGet Package Installation Commands:**

```bash
# Client project
cd src/UrbaGIStory.Client
dotnet add package MudBlazor

# Server project
cd ../UrbaGIStory.Server
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite --version 9.0.0
dotnet add package NetTopologySuite --version 2.5.0
dotnet add package NetTopologySuite.IO.GeoJSON --version 4.0.0
dotnet add package FluentValidation.AspNetCore
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Shared project (if needed)
cd ../UrbaGIStory.Shared
# Add packages only if shared models need them
```

### Architecture Compliance

**Layered Architecture Pattern:**
- Server project uses folder-based layered architecture (not multi-project Clean Architecture)
- Clear separation: Controllers → Services → Repositories → Data
- All layers in single project with folder organization

**Repository Pattern:**
- Repository interfaces will be created in Repositories/ folder (in future stories)
- Services will use repository interfaces via dependency injection
- This story only creates the folder structure - implementation comes later

**Deployment Architecture:**
- Frontend (Blazor WASM) and Backend (Web API) are TWO SEPARATE applications
- They run on the server as separate processes
- Users access the Frontend URL, which communicates with Backend via HTTP
- No client-side installation required - everything via web browser

### Library & Framework Requirements

**MudBlazor (Client):**
- Install latest stable version
- Will be configured in Program.cs in this story
- Component usage will be implemented in future stories

**PostgreSQL/PostGIS Packages (Server):**
- **CRITICAL**: Must use v9.0.x for Npgsql packages (required for .NET 10)
- **CRITICAL**: Must use v2.5.x for NetTopologySuite (required for PostGIS operations)
- Do NOT use older versions - they are incompatible with .NET 10
- Database connection will be configured in Story 1.2

**FluentValidation (Server):**
- Install latest stable version
- Validators will be created in Validators/ folder in future stories

**Swashbuckle (Server):**
- Install latest stable version
- Swagger configuration will be done in Story 1.5

### File Structure Requirements

**Server Project Structure:**
- All folders must be created exactly as specified in Architecture document
- Empty folders are acceptable - files will be added in future stories
- Do NOT create placeholder files unless specified in tasks

**Client Project Structure:**
- Components/ folder for MudBlazor components
- Services/ folder for API clients and JS Interop services
- wwwroot/js/map-interop.js should be created as a placeholder file (empty is fine)

**Shared Project:**
- Initially empty - will contain DTOs and models in future stories
- No specific structure required yet

### Testing Requirements

**For This Story:**
- No unit tests required yet
- Manual verification: solution builds without errors
- Manual verification: folder structure matches Architecture document
- Manual verification: all NuGet packages install successfully

**Future Testing:**
- Test projects can be created in tests/ folder in future stories
- Testing frameworks will be configured later

### Project Structure Notes

**Alignment with Architecture:**
- Structure MUST match Architecture document exactly
- Folder names must match exactly (case-sensitive)
- Project names must match exactly: UrbaGIStory.Client, UrbaGIStory.Server, UrbaGIStory.Shared

**No Conflicts or Variances:**
- This is the initial setup - no existing structure to conflict with
- Follow Architecture document exactly

### Critical Implementation Rules (from Project Context)

**C# Language Rules:**
- Enable nullable reference types in all .csproj files: `<Nullable>enable</Nullable>`
- Use async/await patterns (will be implemented in future stories)
- Use PascalCase for C# code

**Blazor WASM Rules:**
- Use component lifecycle methods correctly (OnInitializedAsync, etc.)
- JS Interop will use MapInterop namespace from map-interop.js (created in Epic 3)
- Use MudBlazor components (configured in this story, used in future stories)

**EF Core Rules:**
- Use async methods for all data access (will be implemented in Story 1.2)
- Use .AsNoTracking() for read-only queries
- Use .Include() for eager loading to avoid N+1 queries

**Exception Handling:**
- Custom exceptions will be created in Exceptions/ folder (future stories)
- Global exception handler will be created in Story 1.4

### References

**Source Documents:**
- [Source: _bmad-output/project-planning-artifacts/epics.md#epic-1-story-1.1] - Story requirements and acceptance criteria
- [Source: _bmad-output/architecture.md#starter-template-evaluation] - Project initialization commands and structure
- [Source: _bmad-output/architecture.md#backend-architecture-structure] - Server project folder structure
- [Source: _bmad-output/architecture.md#data-architecture] - Repository pattern and EF Core configuration details
- [Source: _bmad-output/project-context.md#technology-stack-versions] - Critical version constraints and package requirements
- [Source: _bmad-output/project-context.md#critical-implementation-rules] - C# and framework-specific rules

**Architecture Decisions:**
- Custom Project Structure (not using starter template) - [Source: _bmad-output/architecture.md#starter-template-evaluation]
- Layered Architecture (folder-based, not multi-project) - [Source: _bmad-output/architecture.md#backend-architecture-structure]
- Repository Pattern for data access - [Source: _bmad-output/architecture.md#data-architecture]
- Two-application deployment (Frontend + Backend separate) - [Source: _bmad-output/architecture.md#deployment-architecture-clarification]

## Dev Agent Record

### Agent Model Used

Auto (Cursor AI Agent)

### Debug Log References

- Package version conflicts resolved:
  - Updated Npgsql.EntityFrameworkCore.PostgreSQL from v9.0.0 to v10.0.0 for .NET 10 compatibility
  - Updated Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite from v9.0.0 to v10.0.0
  - Updated NetTopologySuite from v2.5.0 to v2.6.0 (required by Npgsql 10.0.0)

### Completion Notes List

- All acceptance criteria met
- Solution compiles successfully without errors
- All required NuGet packages installed with correct versions for .NET 10
- Folder structure matches Architecture document exactly
- Program.cs files configured with MudBlazor (Client) and CORS placeholder (Server)
- Project references correctly configured (Client → Shared, Server → Shared)
- Note: Package versions were updated from story specifications to ensure .NET 10 compatibility:
  - Npgsql packages: v9.0.x → v10.0.0
  - NetTopologySuite: v2.5.x → v2.6.0

### File List

**Created Files:**
- UrbaGIStory.sln
- src/UrbaGIStory.Client/UrbaGIStory.Client.csproj
- src/UrbaGIStory.Client/Program.cs (modified)
- src/UrbaGIStory.Server/UrbaGIStory.Server.csproj
- src/UrbaGIStory.Server/Program.cs (modified)
- src/UrbaGIStory.Shared/UrbaGIStory.Shared.csproj
- src/UrbaGIStory.Client/wwwroot/js/map-interop.js (placeholder)

**Created Directories:**
- src/UrbaGIStory.Server/Controllers/
- src/UrbaGIStory.Server/Services/
- src/UrbaGIStory.Server/Repositories/
- src/UrbaGIStory.Server/Models/
- src/UrbaGIStory.Server/Data/Configurations/
- src/UrbaGIStory.Server/DTOs/Requests/
- src/UrbaGIStory.Server/DTOs/Responses/
- src/UrbaGIStory.Server/Mappings/
- src/UrbaGIStory.Server/Validators/
- src/UrbaGIStory.Server/Exceptions/
- src/UrbaGIStory.Server/Middleware/
- src/UrbaGIStory.Server/Identity/
- src/UrbaGIStory.Server/Extensions/
- src/UrbaGIStory.Client/Components/
- src/UrbaGIStory.Client/Services/
- src/UrbaGIStory.Client/wwwroot/js/

