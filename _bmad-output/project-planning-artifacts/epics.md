---
stepsCompleted: [1, 2, 3, 4]
inputDocuments:
  - prd.md
  - architecture.md
  - project-planning-artifacts/ux-design-specification.md
functionalRequirementsCount: 77
nonFunctionalRequirementsCount: 18
additionalRequirementsCount: 20
epicsCount: 9
storiesCount: 58
validationStatus: 'approved'
workflowStatus: 'complete'
readyForDevelopment: true
---

# UrbaGIStory - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for UrbaGIStory, decomposing the requirements from the PRD, UX Design, and Architecture requirements into implementable stories.

## Requirements Inventory

### Functional Requirements

- FR1: Users can view an interactive map showing their work zone
- FR2: Users can visualize geometries created in QGIS on the interactive map
- FR3: Users can select urban entities on the map to view their information
- FR4: Users can navigate geographically through their work zone using map controls (zoom, pan, etc.)
- FR5: Users can see entities displayed on the map based on their geographic location
- FR6: Users can create entities based on existing geometries created in QGIS
- FR7: Users can assign entity type to created entities (building, plaza, street, boulevard, facade line, block, etc.)
- FR8: Users can view information from existing entities
- FR9: Users can add information to entities (variables, metrics, metadata)
- FR10: The system automatically displays available properties based on categories assigned to the entity type
- FR11: Users can see information from multiple specialists/departments linked to the same entity
- FR12: Users can query information from existing entities
- FR13: An entity can be linked to a geometry or exist without a geometry
- FR14: A geometry can be linked to multiple entities (e.g., container with multiple buildings inside)
- FR15: Users can attach documents to urban entities
- FR16: Users can manage documents related to each entity
- FR17: Users can add temporal metadata to documents (dates, types, authors)
- FR18: Users can view documents linked to each entity
- FR19: Users can see documents from different specialists linked to the same entity
- FR20: The system loads predefined categories at application startup
- FR21: Office manager can create new categories that function as work templates
- FR22: Office manager can edit existing categories
- FR23: Office manager can assign categories to entity types
- FR24: Each category contains properties that define what information to collect
- FR25: The system automatically displays available properties based on categories assigned to entity type
- FR26: Specialists can use existing categories assigned to entity types
- FR27: Specialists can populate properties defined within categories
- FR28: The system dictates unified methodology through categories
- FR29: Users can filter entities by categories
- FR30: Users can filter entities by properties within categories
- FR31: Users can filter entities by entity type
- FR32: Users can filter entities by any combination of criteria (categories, properties, entity type, etc.)
- FR33: Users can correlate variables from different departments for analysis
- FR34: Users can generate reports combining different perspectives (architectural, heritage, urbanistic)
- FR35: The map updates to show filtered results based on selected criteria
- FR36: Technical administrator can create new users in the system
- FR37: Technical administrator can assign roles to users (office manager, specialist, technical administrator)
- FR38: Technical administrator can manage user authentication and passwords
- FR39: Office manager can moderate permissions between groups of users and entities
- FR40: Office manager can assign dynamic permissions (read/write) to user groups for specific entities
- FR41: Users with advanced privileges (office manager) can create and edit categories
- FR42: Users with normal privileges (specialists) can use existing categories and populate properties
- FR43: Multiple specialists can work on the same entity simultaneously
- FR44: The system handles concurrent access to entities (synchronous application)
- FR45: The system can read geometries from PostgreSQL/PostGIS tables created by QGIS
- FR46: The system displays geometries created in QGIS correctly on the interactive map
- FR47: The system maintains a link between entities and geometries stored in QGIS tables
- FR48: QGIS and UrbaGIStory can operate in parallel without interfering with each other's data
- FR49: Users can select entities that are linked to QGIS geometries without errors
- FR50: Technical administrator can configure QGIS → PostgreSQL/PostGIS connection
- FR51: Technical administrator can create database tables for the system
- FR52: Technical administrator can perform initial system configuration
- FR53: Technical administrator can load predefined categories at application startup
- FR54: Technical administrator can monitor system performance
- FR55: Technical administrator can resolve technical issues and provide support
- FR56: Technical administrator can manage system updates and maintenance
- FR57: The system links information from multiple specialists to the same entity
- FR58: Users can see all information related to an entity in a unified view
- FR59: Information from different departments appears linked in one place for each entity
- FR60: The system maintains data integrity when multiple users work on the same entity
- FR61: Users can edit information previously added to entities
- FR62: Users can delete information from entities
- FR63: Users can delete documents attached to entities
- FR64: Users can preview documents before downloading
- FR65: All documents have a datetime field (fixed or estimated) that can be used for searching
- FR66: Users can search documents by datetime (fixed or estimated) regardless of document type
- FR67: Office manager can view summary/dashboard of specialist activity
- FR68: Office manager can view statistics of information completeness
- FR69: Users can see authorship/traceability of information in reports
- FR70: Office manager can unassign categories from entity types (soft delete strategy - categories remain in system but are not active)
- FR71: Technical administrator can deactivate users without deleting them (soft delete strategy)
- FR72: Technical administrator can backup the database
- FR73: Technical administrator can restore data from backup
- FR74: Technical administrator can view system logs for diagnostics
- FR75: The system provides guided process or documentation for initial setup
- FR76: Users can export or save generated reports

### NonFunctional Requirements

- NFR1: Entity Selection Response Time - Entity selection on the map must complete within 1 second (highest priority)
- NFR2: Map Initial Load Time - Interactive map must load and display initial geometries within 3 seconds
- NFR3: Filter Application Response Time - Filtering operations must update the map and results within 2 seconds
- NFR4: Document Operations Performance - Document upload within 5 seconds for files up to 10MB, preview within 2 seconds, download initiation within 1 second
- NFR5: Concurrent User Support - System must support up to 10 concurrent users without performance degradation
- NFR6: Graceful Performance Degradation - System must provide loading indicators for operations taking longer than 2 seconds, no operation should block UI completely
- NFR7: Document Protection - Document access must be controlled by the permission system (FR39-FR40)
- NFR8: Unauthorized Access Prevention - Role-based access control (FR36-FR42) must be enforced at all access points
- NFR9: Data Integrity Protection - System must maintain data integrity when multiple users work on the same entity (FR60), soft delete strategy (FR77) must prevent referential integrity errors, all data modifications must be traceable (FR69)
- NFR10: User Authentication Security - User authentication must be secure (FR38)
- NFR11: Database Security - Database backups (FR72) must be stored securely
- NFR12: QGIS Integration Reliability - QGIS integration (FR45-FR49) must be reliable and stable, system must maintain data consistency when QGIS and UrbaGIStory operate in parallel (FR48)
- NFR13: PostGIS Data Integrity - Link between entities and geometries (FR47) must remain consistent
- NFR14: Integration Error Handling - System must log integration errors for diagnostics (FR74)
- NFR15: System Availability - System must be available 24/7 for daily operations
- NFR16: Backup and Recovery - Database backups (FR72) must be performed regularly, system must support data restoration from backup (FR73)
- NFR17: System Monitoring - Technical administrator must be able to monitor system performance (FR54), system logs (FR74) must be available for diagnostics
- NFR18: Data Persistence - All data must be persisted reliably to PostgreSQL/PostGIS database

### Additional Requirements

**From Architecture Document:**

- Project initialization using custom starter template (not generic template) - commands documented in Architecture for creating solution, Blazor WASM project, Web API project, and Shared library
- EF Core + PostGIS configuration with specific NuGet packages (Npgsql.EntityFrameworkCore.PostgreSQL v9.0.x, NetTopologySuite v2.5.x)
- PostGIS extension must be enabled in database (`modelBuilder.HasPostgresExtension("postgis")`)
- GIST indexes required on all geometry columns for spatial query performance
- GIN indexes required on JSONB `dynamic_properties` column for efficient property filtering
- Repository Pattern with interfaces - all repositories must implement interfaces
- Service Layer with interfaces - all services must implement interfaces
- Authentication: ASP.NET Core Identity + JWT
- Validation: FluentValidation validators in `Validators/` folder
- Error Handling: GlobalExceptionHandlerMiddleware converts exceptions to ProblemDetails (RFC 7807)
- API Design: RESTful API with standard conventions, no versioning initially
- API Documentation: Swagger/OpenAPI with Swashbuckle
- CORS: Configured for Blazor WASM client
- JS Interop: Thin wrapper pattern with `MapInterop` namespace, map loads GeoJSON directly from API URLs
- QGIS Integration: Separate tables architecture (ADR 1), Foreign Key with ON DELETE SET NULL
- Hybrid Data Model: SQL structure + JSONB column for dynamic properties (ADR 2)
- Dynamic Category System: Runtime property generation from category definitions (ADR 4)
- Optimistic Concurrency: Version-based conflict detection (ADR 5)
- Dynamic Filtering: Hybrid query combining PostGIS spatial filters with JSONB property filters (ADR 6)
- Two-application deployment: Frontend (Blazor WASM) and Backend (Web API) are separate applications
- Client Requirements: No installation required on client computers, only modern web browser needed

**From UX Design Document:**

- Desktop-first approach (no mobile initially)
- Two distinct user flows: Edit/Add vs Consult (both require map view)
- Dynamic property display based on categories
- Map always visible in both modes
- Clear hierarchical design for office manager's view
- Group properties by category in the UI
- Loading indicators for async operations
- Error handling with user-friendly messages

### FR Coverage Map

FR1: Epic 3 - View interactive map showing work zone
FR2: Epic 3 - Visualize geometries created in QGIS on map
FR3: Epic 3 - Select urban entities on map to view information
FR4: Epic 3 - Navigate geographically through work zone using map controls
FR5: Epic 3 - See entities displayed on map based on geographic location
FR6: Epic 5 - Create entities based on existing geometries from QGIS
FR7: Epic 5 - Assign entity type to created entities
FR8: Epic 5 - View information from existing entities
FR9: Epic 5 - Add information to entities (variables, metrics, metadata)
FR10: Epic 5 - System automatically displays available properties based on categories
FR11: Epic 5 - See information from multiple specialists/departments linked to same entity
FR12: Epic 5 - Query information from existing entities
FR13: Epic 5 - Entity can be linked to geometry or exist without geometry
FR14: Epic 5 - Geometry can be linked to multiple entities
FR15: Epic 7 - Attach documents to urban entities
FR16: Epic 7 - Manage documents related to each entity
FR17: Epic 7 - Add temporal metadata to documents
FR18: Epic 7 - View documents linked to each entity
FR19: Epic 7 - See documents from different specialists linked to same entity
FR20: Epic 6 - System loads predefined categories at application startup
FR21: Epic 6 - Office manager can create new categories as work templates
FR22: Epic 6 - Office manager can edit existing categories
FR23: Epic 6 - Office manager can assign categories to entity types
FR24: Epic 6 - Each category contains properties that define what information to collect
FR25: Epic 6 - System automatically displays available properties based on categories
FR26: Epic 6 - Specialists can use existing categories assigned to entity types
FR27: Epic 6 - Specialists can populate properties defined within categories
FR28: Epic 6 - System dictates unified methodology through categories
FR29: Epic 8 - Filter entities by categories
FR30: Epic 8 - Filter entities by properties within categories
FR31: Epic 8 - Filter entities by entity type
FR32: Epic 8 - Filter entities by any combination of criteria
FR33: Epic 8 - Correlate variables from different departments for analysis
FR34: Epic 8 - Generate reports combining different perspectives
FR35: Epic 8 - Map updates to show filtered results based on selected criteria
FR36: Epic 2 - Technical administrator can create new users
FR37: Epic 2 - Technical administrator can assign roles to users
FR38: Epic 2 - Technical administrator can manage user authentication and passwords
FR39: Epic 2 - Office manager can moderate permissions between groups of users and entities
FR40: Epic 2 - Office manager can assign dynamic permissions to user groups
FR41: Epic 2 - Users with advanced privileges can create and edit categories
FR42: Epic 2 - Users with normal privileges can use existing categories and populate properties
FR43: Epic 2 - Multiple specialists can work on same entity simultaneously
FR44: Epic 2 - System handles concurrent access to entities
FR45: Epic 4 - System can read geometries from PostgreSQL/PostGIS tables created by QGIS
FR46: Epic 4 - System displays geometries created in QGIS correctly on interactive map
FR47: Epic 4 - System maintains link between entities and geometries stored in QGIS tables
FR48: Epic 4 - QGIS and UrbaGIStory can operate in parallel without interfering
FR49: Epic 4 - Users can select entities linked to QGIS geometries without errors
FR50: Epic 1 - Technical administrator can configure QGIS → PostgreSQL/PostGIS connection
FR51: Epic 1 - Technical administrator can create database tables for the system
FR52: Epic 1 - Technical administrator can perform initial system configuration
FR53: Epic 1 - Technical administrator can load predefined categories at application startup
FR54: Epic 1 - Technical administrator can monitor system performance
FR55: Epic 1 - Technical administrator can resolve technical issues and provide support
FR56: Epic 1 - Technical administrator can manage system updates and maintenance
FR57: Epic 5 - System links information from multiple specialists to same entity
FR58: Epic 5 - Users can see all information related to entity in unified view
FR59: Epic 5 - Information from different departments appears linked in one place
FR60: Epic 5 - System maintains data integrity when multiple users work on same entity
FR61: Epic 5 - Users can edit information previously added to entities
FR62: Epic 5 - Users can delete information from entities
FR63: Epic 7 - Users can delete documents attached to entities
FR64: Epic 7 - Users can preview documents before downloading
FR65: Epic 7 - All documents have datetime field for searching
FR66: Epic 7 - Users can search documents by datetime regardless of document type
FR67: Epic 9 - Office manager can view summary/dashboard of specialist activity
FR68: Epic 9 - Office manager can view statistics of information completeness
FR69: Epic 8 - Users can see authorship/traceability of information in reports
FR70: Epic 6 - Office manager can unassign categories from entity types (soft delete)
FR71: Epic 2 - Technical administrator can deactivate users without deleting them (soft delete)
FR72: Epic 1 - Technical administrator can backup the database
FR73: Epic 1 - Technical administrator can restore data from backup
FR74: Epic 1 - Technical administrator can view system logs for diagnostics
FR75: Epic 1 - System provides guided process or documentation for initial setup
FR76: Epic 8 - Users can export or save generated reports
FR77: Epic 5 - System uses soft delete strategy to maintain data consistency

## Epic List

### Epic 1: Project Foundation & System Setup

Establish the technical foundation of the project and enable initial system configuration. This epic includes project initialization, database setup, QGIS integration configuration, system administration capabilities, and setup documentation.

**FRs covered:** FR50-FR56, FR72-FR75

**User Outcome:** Technical administrators can set up and configure the system, perform backups, monitor performance, and manage system maintenance.

**Implementation Notes:**

- Project initialization using custom starter template (Architecture commands)
- EF Core + PostGIS configuration with specific NuGet packages
- PostGIS extension enablement
- GIST/GIN indexes configuration
- Repository and Service layer setup
- Authentication infrastructure
- Error handling middleware
- API documentation setup

#### Story 1.1: Inicializar proyecto y estructura base

As a technical administrator,
I want to initialize the project with the correct structure and dependencies,
So that I have a solid foundation for building the application.

**FRs covered:** FR51, FR52

**Acceptance Criteria:**

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

#### Story 1.2: Configurar base de datos y PostGIS

As a technical administrator,
I want to configure the database connection and PostGIS extension,
So that the system can store and query spatial data.

**FRs covered:** FR50, FR51

**Acceptance Criteria:**

**Given** PostgreSQL with PostGIS extension is installed
**When** I configure the database connection in appsettings.json
**Then** The connection string is properly configured
**And** EF Core DbContext is set up with Npgsql provider
**And** PostGIS extension is enabled via `modelBuilder.HasPostgresExtension("postgis")`
**And** The database can be created using `dotnet ef database update`
**And** PostGIS functions are available for spatial queries

**Given** The database is configured
**When** I run the application
**Then** The database connection is established successfully
**And** PostGIS extension is verified to be active

#### Story 1.3: Configurar autenticación y autorización base

As a technical administrator,
I want to set up the authentication and authorization infrastructure,
So that users can securely access the system.

**FRs covered:** FR38, FR52

**Acceptance Criteria:**

**Given** ASP.NET Core Identity is configured
**When** I set up the authentication system
**Then** ASP.NET Core Identity is integrated with PostgreSQL
**And** JWT token generation is configured
**And** User and Role entities are created in the database
**And** Three default roles exist: TechnicalAdministrator, OfficeManager, Specialist
**And** Password hashing is configured securely
**And** JWT settings are configurable via appsettings.json

**Given** Authentication is configured
**When** I create a test user
**Then** The user can be stored in the database
**And** Passwords are hashed (not stored in plain text)
**And** Roles can be assigned to users

#### Story 1.4: Implementar manejo de errores global

As a technical administrator,
I want to have consistent error handling across the application,
So that errors are properly logged and returned to clients in a standard format.

**FRs covered:** FR55, FR74

**Acceptance Criteria:**

**Given** A GlobalExceptionHandlerMiddleware is implemented
**When** An unhandled exception occurs in the API
**Then** The exception is caught by the middleware
**And** A ProblemDetails (RFC 7807) response is returned
**And** The error is logged with appropriate detail level
**And** Sensitive information is not exposed to clients
**And** The response includes a trace ID for diagnostics

**Given** Different types of errors occur
**When** Exceptions are thrown (validation, not found, unauthorized, etc.)
**Then** Each error type returns appropriate HTTP status code
**And** Error messages are user-friendly
**And** Stack traces are only logged, not returned to clients

#### Story 1.5: Configurar documentación API (Swagger)

As a technical administrator,
I want to have API documentation available,
So that developers can understand and test the API endpoints.

**FRs covered:** FR52

**Acceptance Criteria:**

**Given** Swashbuckle is installed
**When** I configure Swagger/OpenAPI
**Then** Swagger UI is accessible at `/swagger`
**And** All API endpoints are documented
**And** Request/response models are displayed
**And** Authentication can be tested via Swagger UI (JWT Bearer token)
**And** API version information is displayed
**And** Example requests and responses are shown

**Given** Swagger is configured
**When** I add a new API endpoint
**Then** It automatically appears in Swagger documentation
**And** XML comments are displayed as descriptions

#### Story 1.6: Implementar endpoints de administración del sistema

As a technical administrator,
I want to have endpoints for system administration tasks,
So that I can configure and manage the system.

**FRs covered:** FR50, FR52, FR53, FR54, FR55, FR56

**Acceptance Criteria:**

**Given** System administration endpoints are implemented
**When** I access the QGIS configuration endpoint
**Then** I can view and update QGIS → PostgreSQL/PostGIS connection settings
**And** Connection settings are validated before saving
**And** Changes are persisted to configuration

**Given** System configuration endpoints exist
**When** I access the system configuration endpoint
**Then** I can view current system settings
**And** I can update system configuration
**And** Configuration changes are validated

**Given** Category management endpoints exist
**When** I access the predefined categories endpoint
**Then** I can trigger loading of predefined categories at startup
**And** Categories are loaded from seed data or configuration file
**And** Loading status is returned

**Given** System monitoring endpoints exist
**When** I access the system status endpoint
**Then** I can view system health information
**And** Database connection status is displayed
**And** PostGIS extension status is verified
**And** Basic performance metrics are available

**Given** All endpoints are implemented
**When** I access them
**Then** All endpoints require TechnicalAdministrator role
**And** Unauthorized access returns 403 Forbidden
**And** All endpoints return proper error responses

#### Story 1.7: Implementar funcionalidad de backup/restore

As a technical administrator,
I want to backup and restore the database,
So that I can protect data and recover from failures.

**FRs covered:** FR72, FR73

**Acceptance Criteria:**

**Given** Backup functionality is implemented
**When** I trigger a database backup
**Then** A backup file is created with timestamp in the filename
**And** The backup includes all database data and schema
**And** PostGIS spatial data is included in the backup
**And** The backup file is stored in a configured secure location
**And** Backup completion status is returned
**And** Backup errors are logged

**Given** Restore functionality is implemented
**When** I trigger a database restore from a backup file
**Then** The system validates the backup file before restoring
**And** Current database is backed up before restore (safety measure)
**And** Database is restored from the selected backup file
**And** PostGIS data is correctly restored
**And** Restore completion status is returned
**And** Restore errors are logged

**Given** Backup/restore endpoints exist
**When** I access them
**Then** All endpoints require TechnicalAdministrator role
**And** Backup files are listed with metadata (date, size)
**And** Restore can be performed from the list of available backups

#### Story 1.8: Implementar logging y monitoreo

As a technical administrator,
I want to have comprehensive logging and monitoring,
So that I can diagnose issues and monitor system performance.

**FRs covered:** FR54, FR74

**Acceptance Criteria:**

**Given** Logging is configured
**When** Application events occur
**Then** Logs are written with appropriate levels (Information, Warning, Error, Critical)
**And** Logs include timestamp, log level, message, and context
**And** Exceptions include stack traces
**And** Logs are stored in configured location (file, database, or both)
**And** Log rotation is configured to prevent disk space issues

**Given** System monitoring is implemented
**When** I access the logs endpoint
**Then** I can view recent log entries
**And** I can filter logs by level, date range, or search term
**And** Log entries are paginated
**And** Logs can be exported for analysis

**Given** Performance monitoring is implemented
**When** API requests are made
**Then** Request duration is logged
**And** Slow queries (> 2 seconds) are flagged
**And** Database query performance is tracked
**And** Performance metrics are available via monitoring endpoint

**Given** All logging is configured
**When** I access monitoring endpoints
**Then** All endpoints require TechnicalAdministrator role
**And** Logs are accessible for diagnostics
**And** Sensitive information (passwords, tokens) is not logged

### Epic 2: User Authentication & Authorization

Users can authenticate securely and the system manages roles and permissions effectively. This epic enables user management, role-based access control, dynamic permissions, and concurrent user support.

**FRs covered:** FR36-FR44, FR71

**User Outcome:** Users can log in securely, administrators can manage users and roles, office managers can configure permissions, and multiple users can work simultaneously without conflicts.

**Implementation Notes:**

- ASP.NET Core Identity + JWT
- Role-based access control (Office Manager, Specialist, Technical Administrator)
- Dynamic permission system
- Optimistic concurrency for multi-user support
- Soft delete for user deactivation

#### Story 2.1: Implementar registro e inicio de sesión de usuarios

As a user,
I want to log in to the system with my credentials,
So that I can access the application securely.

**FRs covered:** FR38

**Acceptance Criteria:**

**Given** A user account exists in the system
**When** I provide valid username and password
**Then** I receive a JWT token
**And** The token includes my user ID and roles
**And** The token has appropriate expiration time
**And** I am redirected to the main application

**Given** I provide invalid credentials
**When** I attempt to log in
**Then** I receive an error message indicating invalid credentials
**And** No token is issued
**And** Failed login attempt is logged

**Given** I am logged in
**When** My token expires
**Then** I am redirected to login page
**And** I receive a message indicating session expired

#### Story 2.2: Implementar gestión de usuarios por administrador técnico

As a technical administrator,
I want to create and manage user accounts,
So that I can control who has access to the system.

**FRs covered:** FR36, FR37, FR38

**Acceptance Criteria:**

**Given** I am a technical administrator
**When** I create a new user
**Then** I can provide username, email, and password
**And** The user is created in the database
**And** Password is hashed securely
**And** User is assigned a default role (Specialist)
**And** User creation is logged

**Given** A user exists in the system
**When** I update user information
**Then** I can change username, email, or password
**And** Changes are validated before saving
**And** Password changes require confirmation
**And** Updates are logged

**Given** I want to view all users
**When** I access the user management interface
**Then** I see a list of all users
**And** Each user shows username, email, role, and status (active/inactive)
**And** I can filter users by role or status
**And** I can search users by username or email

#### Story 2.3: Implementar asignación de roles a usuarios

As a technical administrator,
I want to assign roles to users,
So that users have appropriate permissions in the system.

**FRs covered:** FR37, FR41, FR42

**Acceptance Criteria:**

**Given** Three roles exist: TechnicalAdministrator, OfficeManager, Specialist
**When** I assign a role to a user
**Then** The role is assigned successfully
**And** User permissions are updated immediately
**And** Role assignment is logged
**And** User must re-authenticate for permission changes to take effect

**Given** A user has a role assigned
**When** I change the user's role
**Then** The previous role is removed
**And** The new role is assigned
**And** Role change is logged
**And** User permissions are updated

**Given** I want to view role assignments
**When** I access the user management interface
**Then** I can see which role each user has
**And** I can filter users by role
**And** I can see users with multiple roles (if supported)

#### Story 2.4: Implementar gestión de permisos dinámicos por jefe de oficina

As an office manager,
I want to configure permissions for user groups and entities,
So that I can control access to specific information.

**FRs covered:** FR39, FR40

**Acceptance Criteria:**

**Given** I am an office manager
**When** I configure permissions for a user group
**Then** I can assign read/write permissions to specific entities
**And** Permissions are stored in the database
**And** Permission changes take effect immediately
**And** Permission configuration is logged

**Given** Permissions are configured
**When** A user attempts to access an entity
**Then** The system checks user's group permissions
**And** Access is granted or denied based on permissions
**And** Unauthorized access attempts are logged

**Given** I want to view current permissions
**When** I access the permission management interface
**Then** I can see all user groups and their permissions
**And** I can see which entities each group can access
**And** I can filter by user group or entity
**And** I can modify permissions for any group

#### Story 2.5: Implementar desactivación de usuarios (soft delete)

As a technical administrator,
I want to deactivate users without deleting them,
So that I can maintain data integrity and audit trails.

**FRs covered:** FR71

**Acceptance Criteria:**

**Given** A user exists in the system
**When** I deactivate the user
**Then** The user's `IsActive` flag is set to false
**And** The user record remains in the database
**And** The user cannot log in
**And** User deactivation is logged with timestamp and administrator

**Given** A deactivated user exists
**When** I reactivate the user
**Then** The user's `IsActive` flag is set to true
**And** The user can log in again
**And** User reactivation is logged

**Given** I want to view user status
**When** I access the user management interface
**Then** I can see which users are active or inactive
**And** I can filter users by status
**And** Deactivated users are clearly marked

**Given** A deactivated user has created data
**When** I view entities or documents created by that user
**Then** The data remains accessible
**And** The user's name is still associated with the data (for audit trail)
**And** No referential integrity errors occur

#### Story 2.6: Implementar control de acceso basado en roles (RBAC)

As a user,
I want the system to enforce role-based permissions,
So that I can only access features appropriate to my role.

**FRs covered:** FR41, FR42, FR43, FR44

**Acceptance Criteria:**

**Given** I am a Specialist
**When** I attempt to access office manager features
**Then** Access is denied
**And** I receive an appropriate error message
**And** Unauthorized access attempt is logged

**Given** I am an Office Manager
**When** I access the system
**Then** I can create and edit categories
**And** I can configure permissions
**And** I can view dashboards and reports
**And** I cannot access technical administrator features

**Given** I am a Technical Administrator
**When** I access the system
**Then** I can access all features
**And** I can manage users and roles
**And** I can configure system settings

**Given** Role-based access is enforced
**When** API endpoints are called
**Then** Each endpoint checks user roles
**And** Unauthorized requests return 403 Forbidden
**And** Role checks are performed server-side (not just client-side)

#### Story 2.7: Implementar soporte para acceso concurrente de múltiples usuarios

As a user,
I want to work on entities simultaneously with other users,
So that collaboration is fluid without conflicts.

**FRs covered:** FR43, FR44, FR60

**Acceptance Criteria:**

**Given** Multiple users are logged in
**When** Two users access the same entity simultaneously
**Then** Both users can view the entity
**And** Both users can see each other's changes in real-time (or on refresh)
**And** No data corruption occurs

**Given** Optimistic concurrency is implemented
**When** Two users attempt to edit the same entity
**Then** The first user's changes are saved successfully
**And** The second user receives a conflict notification
**And** The second user can view the updated data and retry
**And** Version numbers are used to detect conflicts

**Given** Concurrent access is supported
**When** Multiple users work on different entities
**Then** No performance degradation occurs
**And** All operations complete successfully
**And** System supports up to 10 concurrent users (NFR5)

### Epic 3: Geographic Visualization

Users can visualize and navigate through an interactive map of their work zone. This epic enables the core map functionality that is central to the application.

**FRs covered:** FR1-FR5

**User Outcome:** Users can view their work zone on an interactive map, navigate geographically, see geometries, and select entities to view information.

**Implementation Notes:**

- OpenLayers integration via JS Interop
- Thin wrapper pattern (MapInterop namespace)
- Direct GeoJSON loading from API
- Map controls (zoom, pan, selection)
- Performance: Map load < 3 seconds (NFR2)

#### Story 3.1: Integrar OpenLayers mediante JS Interop

As a developer,
I want to integrate OpenLayers with Blazor using JS Interop,
So that I can display interactive maps in the application.

**FRs covered:** FR1

**Acceptance Criteria:**

**Given** OpenLayers library is included
**When** I set up JS Interop integration
**Then** A thin wrapper is created in `MapInterop` namespace
**And** OpenLayers is initialized via JavaScript
**And** Map container is created in Blazor component
**And** JS Interop methods are available for map operations
**And** Map can be rendered in the browser

**Given** JS Interop is configured
**When** I call map initialization from Blazor
**Then** The map is created successfully
**And** No JavaScript errors occur
**And** Map is responsive to container size changes

#### Story 3.2: Implementar visualización del mapa interactivo con zona de trabajo

As a user,
I want to view an interactive map showing my work zone,
So that I can see the geographic area I'm working with.

**FRs covered:** FR1, FR4

**Acceptance Criteria:**

**Given** I am logged in
**When** I access the main application
**Then** An interactive map is displayed
**And** The map shows my work zone (configured geographic bounds)
**And** Map loads within 3 seconds (NFR2)
**And** Loading indicator is shown while map loads

**Given** The map is displayed
**When** I interact with the map
**Then** I can zoom in and out using mouse wheel or controls
**And** I can pan by dragging the map
**And** Map controls (zoom buttons, scale bar) are visible
**And** Map maintains smooth performance during navigation

**Given** I navigate the map
**When** I change the viewport
**Then** The map updates smoothly
**And** No performance degradation occurs
**And** Map state is maintained during navigation

#### Story 3.3: Implementar carga y visualización de geometrías desde API

As a user,
I want to see geometries on the map,
So that I can visualize spatial data from the system.

**FRs covered:** FR2, FR5

**Acceptance Criteria:**

**Given** Geometries exist in the database
**When** The map loads
**Then** An API endpoint returns geometries as GeoJSON
**And** GeoJSON is loaded directly into OpenLayers via API URL
**And** Geometries are displayed on the map as vector layers
**And** Different geometry types (points, lines, polygons) are rendered correctly
**And** Map load completes within 3 seconds (NFR2)

**Given** Geometries are displayed
**When** I navigate the map
**Then** Geometries remain visible and correctly positioned
**And** Geometries are styled appropriately (colors, line width, etc.)
**And** Performance remains smooth with many geometries

**Given** New geometries are added to the database
**When** I refresh the map
**Then** New geometries appear on the map
**And** Existing geometries remain visible
**And** No duplicate geometries are shown

#### Story 3.4: Implementar selección de entidades en el mapa

As a user,
I want to select entities on the map,
So that I can view their information.

**FRs covered:** FR3, FR5

**Acceptance Criteria:**

**Given** Entities are displayed on the map
**When** I click on an entity
**Then** The entity is selected (highlighted visually)
**And** Entity selection completes within 1 second (NFR1 - highest priority)
**And** Entity information panel opens or updates
**And** Selected entity is clearly indicated on the map

**Given** An entity is selected
**When** I click on a different entity
**Then** Previous selection is cleared
**And** New entity is selected
**And** Information panel updates with new entity data
**And** Selection happens within 1 second

**Given** I want to deselect an entity
**When** I click on empty map area or close button
**Then** Selection is cleared
**And** Entity information panel is closed or cleared
**And** Map returns to normal state

**Given** Multiple entities are close together
**When** I click in an area with overlapping entities
**Then** A selection list is shown
**And** I can choose which entity to select
**And** Selected entity is highlighted on the map

### Epic 4: QGIS Integration

The system can read and display geometries created in QGIS without interfering with QGIS operations. This epic establishes the critical integration with the external GIS tool.

**FRs covered:** FR45-FR49

**User Outcome:** The system can read geometries from QGIS tables, display them correctly, maintain links between entities and geometries, and operate in parallel with QGIS.

**Implementation Notes:**

- Separate tables architecture (ADR 1)
- Foreign Key with ON DELETE SET NULL
- QGISIntegrationService for link validation
- Graceful error handling when QGIS unavailable
- PostGIS data integrity maintenance

**Planning Considerations:**

- **Spike técnico recomendado**: Antes de comprometer todas las stories de este epic, se recomienda realizar un spike técnico enfocado en Story 4.1 (lectura de geometrías desde QGIS). Este spike validará:
  - Conexión a tablas QGIS en PostgreSQL/PostGIS
  - Rendimiento de lectura de geometrías
  - Manejo de errores cuando QGIS no está disponible
  - Visualización correcta de geometrías en el mapa
- Si el spike es exitoso, el resto del epic puede proceder con confianza. Si hay problemas, se pueden ajustar las stories antes de comprometer todo el epic.
- Este epic es crítico pero independiente; puede desarrollarse en paralelo con Epic 3 (Geographic Visualization) una vez que el spike valide la viabilidad.

#### Story 4.1: Implementar lectura de geometrías desde tablas QGIS

As a user,
I want the system to read geometries from QGIS tables,
So that I can visualize geometries created in QGIS.

**FRs covered:** FR45

**Acceptance Criteria:**

**Given** QGIS tables exist in PostgreSQL/PostGIS database
**When** The system reads geometries
**Then** QGIS tables are accessed via read-only queries
**And** Geometries are retrieved using PostGIS functions
**And** No modifications are made to QGIS tables
**And** QGIS can continue operating normally

**Given** QGISIntegrationService is implemented
**When** I request geometries from QGIS tables
**Then** Service connects to configured QGIS schema
**And** Geometries are returned as GeoJSON
**And** Spatial reference system (SRS) is preserved
**And** Geometry types are correctly identified

**Given** QGIS connection is configured
**When** QGIS tables are unavailable or connection fails
**Then** Error is handled gracefully
**And** User receives informative error message
**And** System continues operating (entities without geometries still accessible)
**And** Error is logged for diagnostics

#### Story 4.2: Implementar visualización de geometrías QGIS en el mapa

As a user,
I want to see geometries created in QGIS on the interactive map,
So that I can work with spatial data from QGIS.

**FRs covered:** FR46

**Acceptance Criteria:**

**Given** Geometries exist in QGIS tables
**When** The map loads
**Then** QGIS geometries are displayed on the map
**And** Geometries are correctly positioned and scaled
**And** Spatial reference system matches map projection
**And** All geometry types (points, lines, polygons) render correctly

**Given** QGIS geometries are displayed
**When** I navigate the map
**Then** Geometries remain correctly positioned
**And** Performance is smooth
**And** No rendering errors occur

**Given** New geometries are added in QGIS
**When** I refresh the map
**Then** New geometries appear
**And** Existing geometries remain visible
**And** No duplicate geometries are shown

#### Story 4.3: Implementar enlace entre entidades y geometrías QGIS

As a user,
I want entities to be linked to QGIS geometries,
So that I can associate entity information with spatial data.

**FRs covered:** FR47

**Acceptance Criteria:**

**Given** Separate tables architecture is implemented (ADR 1)
**When** I create an entity linked to a QGIS geometry
**Then** Entity table has Foreign Key to QGIS geometry table
**And** Foreign Key uses ON DELETE SET NULL
**And** Link is stored in database
**And** Link can be queried efficiently

**Given** An entity is linked to a QGIS geometry
**When** I view the entity
**Then** Associated geometry is displayed on the map
**And** Geometry is highlighted when entity is selected
**And** Link information is available in entity details

**Given** A QGIS geometry is deleted
**When** The link is accessed
**Then** Foreign Key constraint handles deletion (ON DELETE SET NULL)
**And** Entity remains in system (geometry link is null)
**And** No referential integrity errors occur
**And** User is notified that geometry is no longer available

#### Story 4.4: Implementar operación paralela sin interferencias

As a user,
I want QGIS and UrbaGIStory to operate in parallel,
So that I can use both tools without conflicts.

**FRs covered:** FR48

**Acceptance Criteria:**

**Given** QGIS is running and modifying geometries
**When** UrbaGIStory reads geometries
**Then** No locks are placed on QGIS tables
**And** QGIS operations are not blocked
**And** UrbaGIStory reads latest available data
**And** No data corruption occurs

**Given** UrbaGIStory is running
**When** QGIS modifies geometries
**Then** UrbaGIStory continues operating normally
**And** Changes in QGIS are visible after map refresh
**And** No conflicts occur between systems

**Given** Both systems are operating
**When** I perform operations in either system
**Then** Each system operates independently
**And** No interference occurs
**And** Data integrity is maintained in both systems

#### Story 4.5: Implementar selección de entidades enlazadas a geometrías QGIS

As a user,
I want to select entities that are linked to QGIS geometries,
So that I can view entity information for spatial features.

**FRs covered:** FR49

**Acceptance Criteria:**

**Given** Entities are linked to QGIS geometries
**When** I click on a geometry on the map
**Then** Linked entity is selected
**And** Entity selection completes within 1 second (NFR1)
**And** Entity information is displayed
**And** No errors occur during selection

**Given** An entity is linked to a QGIS geometry
**When** I select the entity from a list
**Then** Associated geometry is highlighted on the map
**And** Map centers on the geometry if needed
**And** Geometry is clearly visible

**Given** A geometry has multiple entities linked
**When** I click on the geometry
**Then** A list of linked entities is shown
**And** I can select which entity to view
**And** Selected entity information is displayed

**Given** An entity's linked geometry is deleted in QGIS
**When** I attempt to select the entity
**Then** Entity is still accessible
**And** No geometry is displayed (link is null)
**And** User is informed that geometry is no longer available
**And** No errors occur

### Epic 5: Entity Management

Users can create, view, edit, and manage urban entities. This epic enables the core entity CRUD operations and multi-user collaboration.

**FRs covered:** FR6-FR14, FR57-FR60, FR61-FR62, FR77

**User Outcome:** Users can create entities from QGIS geometries, assign entity types, view and edit entity information, see information from multiple specialists, and the system maintains data integrity during concurrent access.

**Implementation Notes:**

- Entity model with optional geometry link
- Support for entities without geometries
- Multiple entities per geometry support
- Dynamic property display based on categories (requires Epic 6)
- Optimistic concurrency (ADR 5)
- Soft delete strategy
- Unified view of multi-specialist information

**Planning Considerations:**

- **Ejecución flexible sin categorías**: Este epic puede funcionar sin categorías inicialmente. Stories 5.1-5.3 (crear entidades, asignar tipo, visualizar) pueden ejecutarse independientemente con propiedades básicas.
- **Story 5.4 depende de Epic 6**: Story 5.4 (agregar información con propiedades dinámicas) requiere que Epic 6 (Category & Template System) esté completo para generar propiedades dinámicas basadas en categorías.
- **Orden recomendado**: Ejecutar Stories 5.1-5.3 primero, luego Epic 6, y finalmente Story 5.4. Esto permite progreso paralelo y validación temprana del modelo de entidades.
- El modelo híbrido (SQL + JSONB) permite esta flexibilidad: las entidades pueden tener propiedades básicas primero y luego extenderse con propiedades dinámicas cuando las categorías estén disponibles.

#### Story 5.1: Implementar creación de entidades basadas en geometrías QGIS

As a user,
I want to create entities from existing QGIS geometries,
So that I can associate entity information with spatial features.

**FRs covered:** FR6, FR13

**Acceptance Criteria:**

**Given** QGIS geometries are available
**When** I create a new entity
**Then** I can select a QGIS geometry to link
**And** Entity is created with link to selected geometry
**And** Link is stored using Foreign Key (ON DELETE SET NULL)
**And** Entity is saved to database
**And** Entity appears on the map at geometry location

**Given** I want to create an entity
**When** I select a geometry
**Then** Available geometries from QGIS tables are listed
**And** I can preview geometry on map before linking
**And** Geometry information (type, area, etc.) is displayed

**Given** An entity is created with geometry link
**When** I view the entity
**Then** Associated geometry is displayed on map
**And** Geometry is highlighted when entity is selected

#### Story 5.2: Implementar asignación de tipo de entidad

As a user,
I want to assign entity types to created entities,
So that entities are properly categorized.

**FRs covered:** FR7

**Acceptance Criteria:**

**Given** Entity types are defined in the system
**When** I create or edit an entity
**Then** I can select entity type from predefined list (building, plaza, street, boulevard, facade line, block, etc.)
**And** Entity type is saved to database
**And** Entity type is displayed in entity information

**Given** Entity types are available
**When** I assign an entity type
**Then** Available categories for that entity type are determined (requires Epic 6)
**And** Entity type assignment is validated
**And** Entity type cannot be null

**Given** An entity has an entity type assigned
**When** I view the entity
**Then** Entity type is clearly displayed
**And** I can filter entities by type

#### Story 5.3: Implementar visualización de información de entidades

As a user,
I want to view information from existing entities,
So that I can access entity data.

**FRs covered:** FR8, FR11, FR58, FR59

**Acceptance Criteria:**

**Given** Entities exist in the system
**When** I select an entity on the map or from a list
**Then** Entity information is displayed
**And** All information from multiple specialists/departments is shown in unified view
**And** Information is grouped by category (when categories are assigned)
**And** Entity type and basic properties are displayed
**And** Linked geometry information is shown (if applicable)

**Given** Multiple specialists have added information to an entity
**When** I view the entity
**Then** All information is displayed together
**And** Information from different departments appears in one place
**And** Each information entry shows author/specialist
**And** Information is organized clearly

**Given** An entity has no information yet
**When** I view the entity
**Then** Entity basic information is displayed (type, creation date, etc.)
**And** Empty state message indicates no additional information
**And** I can add information to the entity

#### Story 5.4: Implementar agregar información a entidades con propiedades dinámicas

As a user,
I want to add information to entities,
So that I can populate entity data with variables, metrics, and metadata.

**FRs covered:** FR9, FR10, FR25, FR27

**Acceptance Criteria:**

**Given** An entity exists and has categories assigned (requires Epic 6)
**When** I add information to the entity
**Then** Available properties are automatically displayed based on assigned categories
**And** Properties are shown as form fields
**And** I can populate properties defined within categories
**And** Dynamic properties are stored in JSONB column
**And** Information is saved with my user ID and timestamp

**Given** Categories define property types (text, number, date, etc.)
**When** I populate properties
**Then** Property values are validated according to category definitions
**And** Required properties are enforced
**And** Invalid values are rejected with clear error messages

**Given** I add information to an entity
**When** I save the information
**Then** Data is persisted to database
**And** Information is immediately available for viewing
**And** My authorship is recorded
**And** Timestamp is stored

#### Story 5.5: Implementar consulta de información de entidades

As a user,
I want to query information from existing entities,
So that I can find specific entities and data.

**FRs covered:** FR12

**Acceptance Criteria:**

**Given** Entities exist in the system
**When** I search for entities
**Then** I can search by entity type, properties, or text fields
**And** Search results are displayed
**And** I can select an entity from results to view details
**And** Selected entity is highlighted on map

**Given** I want to query entities
**When** I use the query interface
**Then** I can filter by multiple criteria
**And** Query results update dynamically
**And** Results show entity type, location, and key information
**And** I can sort results by various fields

**Given** Query results are displayed
**When** I select an entity from results
**Then** Entity information panel opens
**And** Entity is highlighted on map
**And** Map centers on entity if needed

#### Story 5.6: Implementar soporte para entidades sin geometrías

As a user,
I want to create entities without linking them to geometries,
So that I can manage entities that don't have spatial representation yet.

**FRs covered:** FR13

**Acceptance Criteria:**

**Given** I want to create an entity
**When** I create an entity without selecting a geometry
**Then** Entity is created successfully
**And** Geometry link field is null
**And** Entity is saved to database
**And** No errors occur

**Given** An entity exists without geometry
**When** I view the entity
**Then** Entity information is displayed
**And** No geometry is shown on map
**And** Message indicates entity has no spatial representation
**And** I can link a geometry later if needed

**Given** An entity without geometry exists
**When** I later want to link a geometry
**Then** I can update the entity to link a QGIS geometry
**And** Link is established
**And** Geometry appears on map after linking

#### Story 5.7: Implementar soporte para múltiples entidades por geometría

As a user,
I want to link multiple entities to the same geometry,
So that I can represent container relationships (e.g., block containing multiple buildings).

**FRs covered:** FR14

**Acceptance Criteria:**

**Given** A geometry exists in QGIS
**When** I create multiple entities
**Then** I can link multiple entities to the same geometry
**And** Each entity maintains its own information
**And** All entities are stored independently
**And** Foreign Key relationship allows multiple entities per geometry

**Given** Multiple entities are linked to the same geometry
**When** I click on the geometry on the map
**Then** A list of linked entities is displayed
**And** I can select which entity to view
**And** Selected entity information is shown

**Given** Multiple entities share a geometry
**When** I view any of the linked entities
**Then** The shared geometry is displayed
**And** Entity-specific information is shown
**And** I can see that other entities are linked to the same geometry

#### Story 5.8: Implementar edición de información de entidades

As a user,
I want to edit information previously added to entities,
So that I can update entity data.

**FRs covered:** FR61

**Acceptance Criteria:**

**Given** An entity has information added
**When** I edit the information
**Then** I can modify property values
**And** Changes are validated according to category definitions
**And** I can update my own information
**And** I can update information added by others (if I have permission)
**And** Edit history is maintained (who edited, when)

**Given** Optimistic concurrency is implemented (ADR 5)
**When** I edit an entity that was modified by another user
**Then** Version conflict is detected
**And** I am notified of the conflict
**And** I can view the latest version and merge changes
**And** No data loss occurs

**Given** I edit entity information
**When** I save changes
**Then** Updates are persisted to database
**And** Changes are immediately visible
**And** Edit timestamp is updated
**And** Author information is preserved (original author + last editor)

#### Story 5.9: Implementar eliminación de información de entidades (soft delete)

As a user,
I want to delete information from entities,
So that I can remove incorrect or outdated data.

**FRs covered:** FR62, FR77

**Acceptance Criteria:**

**Given** An entity has information added
**When** I delete information
**Then** Information is marked as deleted (soft delete)
**And** `IsDeleted` flag is set to true
**And** Information is not permanently removed from database
**And** Deleted information is not displayed in normal views
**And** Deletion is logged with user and timestamp

**Given** Soft delete is implemented
**When** I delete information
**Then** Referential integrity is maintained
**And** No foreign key constraint errors occur
**And** Related data (documents, links) remains accessible
**And** Deleted information can be restored if needed

**Given** Deleted information exists
**When** I view entity history or admin view
**Then** Deleted information is visible (marked as deleted)
**And** I can see who deleted it and when
**And** I can restore deleted information if I have permission

#### Story 5.10: Implementar integridad de datos con acceso concurrente

As a user,
I want the system to maintain data integrity when multiple users work on the same entity,
So that collaboration is safe and reliable.

**FRs covered:** FR60

**Acceptance Criteria:**

**Given** Optimistic concurrency is implemented (ADR 5)
**When** Multiple users edit the same entity
**Then** Version numbers are used to detect conflicts
**And** First user's changes are saved successfully
**And** Second user receives conflict notification
**And** Second user can view updated data and retry
**And** No data corruption occurs

**Given** Concurrent access is supported
**When** Multiple users view the same entity
**Then** All users see the latest data
**And** Changes from one user are visible to others (on refresh or real-time)
**And** No performance degradation occurs

**Given** Data integrity mechanisms are in place
**When** Concurrent operations occur
**Then** All database transactions are properly isolated
**And** ACID properties are maintained
**And** No partial updates or inconsistent states occur

### Epic 6: Category & Template System

Office managers can create and manage categories that function as work templates, and specialists can use these categories to collect structured information. This epic enables the dynamic categorization system.

**FRs covered:** FR20-FR28, FR70

**User Outcome:** Office managers can create, edit, and assign categories to entity types. Specialists can use assigned categories and populate properties. The system automatically displays available properties based on categories.

**Implementation Notes:**

- Runtime property generation from category definitions (ADR 4)
- Category-to-entity-type assignment
- Property definition within categories
- Dynamic form generation
- Soft delete for category unassignment
- Predefined categories loaded at startup

#### Story 6.1: Implementar carga de categorías predefinidas al inicio

As a technical administrator,
I want predefined categories to be loaded at application startup,
So that the system has default work templates available.

**FRs covered:** FR20, FR53

**Acceptance Criteria:**

**Given** Predefined categories are configured (seed data or configuration file)
**When** The application starts
**Then** Predefined categories are loaded into the database
**And** Categories are available for use immediately
**And** Loading process completes successfully
**And** Loading status is logged

**Given** Predefined categories exist
**When** I view available categories
**Then** Predefined categories are listed
**And** Categories show their properties and definitions
**And** Categories are marked as predefined (cannot be deleted)

**Given** Application startup occurs
**When** Category loading fails
**Then** Error is logged
**And** Application continues with existing categories
**And** Administrator is notified of loading failure

#### Story 6.2: Implementar creación de categorías por jefe de oficina

As an office manager,
I want to create new categories that function as work templates,
So that I can define what information specialists should collect.

**FRs covered:** FR21, FR24, FR41

**Acceptance Criteria:**

**Given** I am an office manager
**When** I create a new category
**Then** I can provide category name and description
**And** I can define properties within the category
**And** Each property has a name, type (text, number, date, etc.), and validation rules
**And** I can mark properties as required or optional
**And** Category is saved to database
**And** Category creation is logged

**Given** I create a category with properties
**When** I define property types
**Then** Supported types are available (text, number, integer, date, boolean, etc.)
**And** Validation rules can be set (min/max values, patterns, etc.)
**And** Property definitions are stored in category configuration
**And** Properties can be reordered

**Given** A category is created
**When** I view the category
**Then** Category name, description, and all properties are displayed
**And** Property types and validation rules are shown
**And** I can edit the category

#### Story 6.3: Implementar edición de categorías existentes

As an office manager,
I want to edit existing categories,
So that I can update work templates as needs change.

**FRs covered:** FR22, FR41

**Acceptance Criteria:**

**Given** A category exists in the system
**When** I edit the category
**Then** I can modify category name and description
**And** I can add new properties
**And** I can modify existing property definitions
**And** I can remove properties (with confirmation)
**And** Changes are validated before saving
**And** Category update is logged

**Given** I edit a category that is assigned to entity types
**When** I save changes
**Then** Changes are applied to the category
**And** Existing entities using the category are not affected (properties remain in JSONB)
**And** New entities will use updated category definition
**And** Users are notified if category structure changed significantly

**Given** I want to edit a predefined category
**When** I attempt to edit
**Then** I can create a copy of the predefined category
**And** I can edit the copy
**And** Original predefined category remains unchanged

#### Story 6.4: Implementar asignación de categorías a tipos de entidad

As an office manager,
I want to assign categories to entity types,
So that specialists know which templates to use for each entity type.

**FRs covered:** FR23, FR26

**Acceptance Criteria:**

**Given** Categories and entity types exist
**When** I assign a category to an entity type
**Then** Assignment is saved to database
**And** Category becomes available for that entity type
**And** Multiple categories can be assigned to the same entity type
**And** Assignment is logged

**Given** Categories are assigned to entity types
**When** A specialist creates or edits an entity of that type
**Then** Assigned categories are available for selection
**And** Properties from assigned categories are displayed
**And** Specialist can use categories to populate information

**Given** I want to view category assignments
**When** I access the assignment interface
**Then** I can see which categories are assigned to each entity type
**And** I can modify assignments
**And** I can see assignment history

#### Story 6.5: Implementar generación dinámica de propiedades basadas en categorías

As a specialist,
I want the system to automatically display available properties based on categories assigned to entity type,
So that I know what information to collect.

**FRs covered:** FR10, FR25, FR28

**Acceptance Criteria:**

**Given** Runtime property generation is implemented (ADR 4)
**When** I view an entity with assigned categories
**Then** Properties from assigned categories are automatically displayed
**And** Properties are shown as form fields with appropriate input types
**And** Required properties are marked
**And** Property labels and help text are shown

**Given** An entity type has multiple categories assigned
**When** I add information to an entity
**Then** Properties from all assigned categories are available
**And** Properties are grouped by category
**And** I can populate properties from any assigned category

**Given** Category definitions include property types and validation
**When** Properties are generated
**Then** Form fields match property types (text input, number input, date picker, etc.)
**And** Validation rules are applied
**And** Invalid input is prevented or flagged

#### Story 6.6: Implementar uso de categorías por especialistas

As a specialist,
I want to use existing categories assigned to entity types,
So that I can collect information according to defined templates.

**FRs covered:** FR26, FR27, FR42

**Acceptance Criteria:**

**Given** Categories are assigned to an entity type
**When** I work with an entity of that type
**Then** Assigned categories are available
**And** I can select which category to use for adding information
**And** Properties from selected category are displayed
**And** I can populate properties defined within the category

**Given** I am a specialist (normal privileges)
**When** I access category management
**Then** I can view available categories
**And** I cannot create or edit categories (only office managers can)
**And** I can see category definitions and properties

**Given** I use a category to add information
**When** I populate properties
**Then** Property values are validated according to category definitions
**And** Required properties must be filled
**And** Information is saved with category association
**And** My authorship is recorded

#### Story 6.7: Implementar desasignación de categorías de tipos de entidad (soft delete)

As an office manager,
I want to unassign categories from entity types,
So that I can update which templates are available without losing category definitions.

**FRs covered:** FR70

**Acceptance Criteria:**

**Given** A category is assigned to an entity type
**When** I unassign the category
**Then** Assignment is marked as inactive (soft delete)
**And** Category definition remains in system
**And** Category is no longer available for new entities of that type
**And** Existing entities using the category are not affected
**And** Unassignment is logged

**Given** A category is unassigned
**When** I view entity type assignments
**Then** Unassigned categories are not shown in active assignments
**And** I can see assignment history
**And** I can reassign the category if needed

**Given** Soft delete strategy is used
**When** I unassign a category
**Then** No referential integrity errors occur
**And** Category can be reactivated
**And** Data integrity is maintained

### Epic 7: Document Management

Users can attach, manage, and search documents related to urban entities with temporal metadata. This epic enables comprehensive document handling.

**FRs covered:** FR15-FR19, FR63-FR66

**User Outcome:** Users can attach documents to entities, add temporal metadata, view documents from multiple specialists, preview documents, and search by datetime.

**Implementation Notes:**

- Document storage (file system or database)
- Temporal metadata (dates, types, authors)
- Datetime field for all documents (fixed or estimated)
- Document preview functionality
- Multi-specialist document linking
- Performance: Upload < 5s, Preview < 2s (NFR4)

#### Story 7.1: Implementar adjuntar documentos a entidades urbanas

As a user,
I want to attach documents to urban entities,
So that I can associate files with spatial features.

**FRs covered:** FR15

**Acceptance Criteria:**

**Given** An entity exists in the system
**When** I attach a document
**Then** I can select a file from my computer
**And** File is uploaded to server
**And** Upload completes within 5 seconds for files up to 10MB (NFR4)
**And** Document is linked to the entity
**And** Document metadata is stored in database
**And** File is stored securely (file system or database)
**And** Upload progress is shown to user

**Given** I want to attach a document
**When** I select a file
**Then** File type and size are validated
**And** Supported file types are accepted (PDF, images, etc.)
**And** File size limits are enforced
**And** Invalid files are rejected with clear error message

**Given** A document is attached
**When** I view the entity
**Then** Attached document is listed
**And** Document name, type, and size are displayed
**And** Document metadata is shown

#### Story 7.2: Implementar gestión de documentos relacionados a entidades

As a user,
I want to manage documents related to each entity,
So that I can organize and maintain document associations.

**FRs covered:** FR16

**Acceptance Criteria:**

**Given** Documents are attached to entities
**When** I view entity documents
**Then** All documents linked to the entity are listed
**And** Documents are organized clearly (by date, type, or author)
**And** I can see document metadata (name, type, size, upload date, author)
**And** I can download or preview documents

**Given** I want to manage documents
**When** I access document management
**Then** I can view all documents for an entity
**And** I can delete documents (if I have permission)
**And** I can update document metadata
**And** Document operations are logged

**Given** Multiple documents exist for an entity
**When** I manage documents
**Then** I can sort documents by various criteria
**And** I can filter documents by type, date, or author
**And** Document list is paginated if needed

#### Story 7.3: Implementar metadata temporal de documentos

As a user,
I want to add temporal metadata to documents,
So that I can track when documents were created or relate to.

**FRs covered:** FR17, FR65

**Acceptance Criteria:**

**Given** I attach a document to an entity
**When** I add temporal metadata
**Then** I can specify a datetime (fixed or estimated)
**And** I can indicate if the date is fixed (known) or estimated
**And** I can add document type and author information
**And** Temporal metadata is stored with the document
**And** Metadata is displayed when viewing documents

**Given** All documents have datetime fields
**When** I view document metadata
**Then** Datetime is displayed (fixed or estimated indicator)
**And** Document type and author are shown
**And** Upload date is also recorded
**And** Temporal information is clearly presented

**Given** I want to add temporal metadata
**When** I specify a date
**Then** Date can be fixed (exact date known)
**Or** Date can be estimated (approximate date)
**And** Date format is validated
**And** Estimated dates are clearly marked

#### Story 7.4: Implementar visualización de documentos vinculados a entidades

As a user,
I want to view documents linked to each entity,
So that I can access relevant files.

**FRs covered:** FR18

**Acceptance Criteria:**

**Given** Documents are attached to an entity
**When** I view the entity
**Then** Linked documents are displayed in a documents section
**And** Document list shows name, type, size, and metadata
**And** I can click on a document to preview or download
**And** Documents are accessible based on my permissions

**Given** I want to view documents
**When** I access the documents section
**Then** Documents are listed with clear organization
**And** Document metadata is visible
**And** I can preview documents before downloading
**And** Download is available for all document types

**Given** Documents from multiple specialists exist
**When** I view documents
**Then** All documents are shown together
**And** Author/specialist information is displayed
**And** Documents are grouped or sorted appropriately

#### Story 7.5: Implementar visualización de documentos de múltiples especialistas

As a user,
I want to see documents from different specialists linked to the same entity,
So that I can access all relevant documentation.

**FRs covered:** FR19

**Acceptance Criteria:**

**Given** Multiple specialists have attached documents to an entity
**When** I view the entity documents
**Then** Documents from all specialists are displayed
**And** Each document shows author/specialist information
**And** Documents are organized clearly (by specialist, date, or type)
**And** All documents are accessible (based on permissions)

**Given** Documents from different departments exist
**When** I view documents
**Then** I can see which department/specialist added each document
**And** Documents are linked in one place for the entity
**And** No documents are hidden or filtered out (unless by permissions)

**Given** I want to filter documents
**When** I use filter options
**Then** I can filter by specialist/author
**And** I can filter by document type
**And** I can filter by date range
**And** Filtered results update dynamically

#### Story 7.6: Implementar eliminación de documentos adjuntos

As a user,
I want to delete documents attached to entities,
So that I can remove incorrect or outdated files.

**FRs covered:** FR63

**Acceptance Criteria:**

**Given** A document is attached to an entity
**When** I delete the document
**Then** I am asked to confirm deletion
**And** Document is removed from entity
**And** File is deleted from storage (or marked for deletion)
**And** Document record is removed from database (or soft deleted)
**And** Deletion is logged with user and timestamp

**Given** I want to delete a document
**When** I have appropriate permissions
**Then** I can delete documents I uploaded
**And** I can delete documents uploaded by others (if I have permission)
**And** Unauthorized deletion attempts are blocked

**Given** A document is deleted
**When** I view entity documents
**Then** Deleted document is no longer listed
**And** No broken links or errors occur
**And** Deletion is permanent (or soft delete with restore option for admins)

#### Story 7.7: Implementar vista previa de documentos

As a user,
I want to preview documents before downloading,
So that I can verify content without downloading.

**FRs covered:** FR64

**Acceptance Criteria:**

**Given** A document is attached to an entity
**When** I click to preview the document
**Then** Document preview opens within 2 seconds (NFR4)
**And** Preview is displayed in a modal or new tab
**And** Supported file types (PDF, images) are previewed
**And** Preview shows document content clearly

**Given** I want to preview a document
**When** Document type supports preview
**Then** Preview is available
**And** I can zoom or navigate preview (for PDFs)
**And** I can download from preview if needed

**Given** Document type doesn't support preview
**When** I attempt to preview
**Then** Download option is offered instead
**And** Message indicates preview not available for this file type

#### Story 7.8: Implementar búsqueda de documentos por datetime

As a user,
I want to search documents by datetime,
So that I can find documents from specific time periods.

**FRs covered:** FR65, FR66

**Acceptance Criteria:**

**Given** Documents have datetime fields (fixed or estimated)
**When** I search documents by datetime
**Then** I can search by date range
**And** I can search by specific date
**And** Search works for both fixed and estimated dates
**And** Search results show matching documents
**And** Results are displayed regardless of document type

**Given** I want to search documents
**When** I use datetime search
**Then** I can specify start and end dates
**And** I can search across all entities or filter by entity
**And** Search completes quickly
**And** Results are clearly displayed

**Given** Documents have estimated dates
**When** I search by datetime
**Then** Estimated dates are included in search results
**And** Estimated dates are clearly marked in results
**And** Search works correctly with estimated date ranges

### Epic 8: Filtering & Analysis

Users can filter entities by multiple criteria and generate reports for analysis. This epic enables powerful filtering and reporting capabilities.

**FRs covered:** FR29-FR35, FR69, FR76

**User Outcome:** Users can filter entities by categories, properties, entity types, or combinations. They can correlate variables, generate reports, export reports, and see authorship/traceability.

**Implementation Notes:**

- Hybrid PostGIS + JSONB filtering (ADR 6)
- GIST indexes for spatial queries
- GIN indexes for JSONB property queries
- Map updates with filtered results
- Report generation and export
- Performance: Filter application < 2 seconds (NFR3)

**Planning Considerations:**

- **Posicionamiento al final es apropiado**: Este epic depende de que Epic 5 (Entity Management) y Epic 6 (Category & Template System) estén funcionando, ya que necesita entidades con categorías y propiedades para filtrar. El orden actual (Epic 8 después de Epic 7) es correcto.
- **Filtrado básico podría adelantarse**: Si hay tiempo y recursos disponibles, Stories 8.1-8.3 (filtrado por categorías, propiedades, y tipos de entidad) podrían adelantarse una vez que Epic 5 y Epic 6 estén completos, antes de Epic 7 (Document Management). Esto proporcionaría valor temprano a los usuarios.
- **Filtrado avanzado requiere datos**: Stories 8.6-8.9 (correlación, reportes, exportación) requieren datos suficientes para ser útiles, por lo que mantenerlas al final es apropiado.
- Los índices GIST/GIN ya están planificados en Epic 1, por lo que la infraestructura estará lista cuando este epic se ejecute.

#### Story 8.1: Implementar filtrado de entidades por categorías

As a user,
I want to filter entities by categories,
So that I can find entities with specific category assignments.

**FRs covered:** FR29

**Acceptance Criteria:**

**Given** Entities have categories assigned
**When** I filter entities by category
**Then** I can select one or more categories
**And** Filter is applied
**And** Results show only entities with selected categories
**And** Filter application completes within 2 seconds (NFR3)
**And** Results are displayed in list and on map

**Given** I want to filter by categories
**When** I use the filter interface
**Then** Available categories are listed
**And** I can select multiple categories
**And** Selected categories are clearly indicated
**And** I can clear category filters

**Given** Category filter is applied
**When** I view results
**Then** Filtered entities are highlighted on map
**And** Entity list shows only matching entities
**And** Filter criteria are displayed
**And** I can see count of matching entities

#### Story 8.2: Implementar filtrado de entidades por propiedades dentro de categorías

As a user,
I want to filter entities by properties within categories,
So that I can find entities with specific property values.

**FRs covered:** FR30

**Acceptance Criteria:**

**Given** Entities have properties populated from categories
**When** I filter by property values
**Then** I can select a category and its properties
**And** I can specify property value criteria (equals, contains, greater than, etc.)
**And** Filter uses JSONB queries with GIN indexes for performance
**And** Filter application completes within 2 seconds (NFR3)
**And** Results show entities matching property criteria

**Given** I want to filter by properties
**When** I use the property filter
**Then** Available categories and their properties are shown
**And** I can specify value criteria for each property
**And** Multiple property filters can be combined
**And** Property types determine available filter operators

**Given** Property filter is applied
**When** I view results
**Then** Filtered entities are displayed
**And** Map updates to show only matching entities
**And** Property filter criteria are shown
**And** Results are accurate and complete

#### Story 8.3: Implementar filtrado de entidades por tipo de entidad

As a user,
I want to filter entities by entity type,
So that I can focus on specific types of urban features.

**FRs covered:** FR31

**Acceptance Criteria:**

**Given** Entities have different types (building, plaza, street, etc.)
**When** I filter by entity type
**Then** I can select one or more entity types
**And** Filter is applied
**And** Results show only entities of selected types
**And** Filter application completes within 2 seconds (NFR3)
**And** Results are displayed in list and on map

**Given** I want to filter by entity type
**When** I use the filter interface
**Then** Available entity types are listed
**And** I can select multiple types
**And** Selected types are clearly indicated
**And** I can clear type filters

**Given** Entity type filter is applied
**When** I view results
**Then** Filtered entities are highlighted on map
**And** Entity list shows only matching entities
**And** Filter criteria are displayed

#### Story 8.4: Implementar filtrado combinado por múltiples criterios

As a user,
I want to filter entities by any combination of criteria,
So that I can perform complex searches.

**FRs covered:** FR32

**Acceptance Criteria:**

**Given** Multiple filter types are available
**When** I combine filters (categories, properties, entity types, etc.)
**Then** All selected filters are applied together (AND logic)
**And** Results match all filter criteria
**And** Filter application completes within 2 seconds (NFR3)
**And** Combined filter criteria are displayed

**Given** I want to combine filters
**When** I add multiple filter criteria
**Then** Each filter is independent
**And** I can remove individual filters
**And** I can clear all filters at once
**And** Filter state is maintained during session

**Given** Hybrid filtering is implemented (ADR 6)
**When** I combine spatial and property filters
**Then** PostGIS spatial queries are combined with JSONB property queries
**And** GIST and GIN indexes are used for performance
**And** Results are accurate and complete
**And** Performance remains within limits

#### Story 8.5: Implementar actualización del mapa con resultados filtrados

As a user,
I want the map to update to show filtered results,
So that I can see geographic distribution of filtered entities.

**FRs covered:** FR35

**Acceptance Criteria:**

**Given** I apply filters to entities
**When** Filter results are available
**Then** Map updates to show only filtered entities
**And** Non-matching entities are hidden or dimmed
**And** Map update completes smoothly
**And** Filtered entities are clearly visible

**Given** Map shows filtered results
**When** I navigate the map
**Then** Filtered entities remain visible
**And** Map performance is smooth
**And** I can select filtered entities to view details

**Given** I clear filters
**When** Filters are removed
**Then** Map updates to show all entities
**And** All entities are visible again
**And** Map state returns to normal

#### Story 8.6: Implementar correlación de variables de diferentes departamentos

As a user,
I want to correlate variables from different departments,
So that I can perform cross-departmental analysis.

**FRs covered:** FR33

**Acceptance Criteria:**

**Given** Entities have information from multiple departments
**When** I perform correlation analysis
**Then** I can select variables from different categories/departments
**And** System correlates variables across entities
**And** Correlation results are displayed
**And** I can see relationships between variables

**Given** I want to correlate variables
**When** I use the correlation interface
**Then** Available variables from all departments are shown
**And** I can select variables to correlate
**And** Correlation analysis is performed
**And** Results show relationships (correlation coefficients, trends, etc.)

**Given** Correlation analysis is performed
**When** I view results
**Then** Results are displayed clearly
**And** I can export correlation data
**And** Results can be visualized (charts, graphs)

#### Story 8.7: Implementar generación de reportes combinando diferentes perspectivas

As a user,
I want to generate reports combining different perspectives,
So that I can analyze data from architectural, heritage, and urbanistic viewpoints.

**FRs covered:** FR34

**Acceptance Criteria:**

**Given** Entities have information from multiple perspectives
**When** I generate a report
**Then** I can select which perspectives to include (architectural, heritage, urbanistic)
**And** I can select entities to include (filtered or all)
**And** Report is generated with selected data
**And** Report includes information from selected perspectives

**Given** I want to generate a report
**When** I use the report generation interface
**Then** I can configure report parameters
**And** I can select data fields to include
**And** I can choose report format
**And** Report generation completes successfully

**Given** A report is generated
**When** I view the report
**Then** Report shows combined perspectives clearly
**And** Data is organized logically
**And** Report includes metadata (date, author, criteria)
**And** I can export the report

#### Story 8.8: Implementar trazabilidad y autoría en reportes

As a user,
I want to see authorship and traceability of information in reports,
So that I can verify data sources and contributors.

**FRs covered:** FR69

**Acceptance Criteria:**

**Given** Information in entities has authorship recorded
**When** I generate a report
**Then** Report includes authorship information for each data point
**And** Specialist/department who added information is shown
**And** Timestamps of information addition are included
**And** Traceability chain is maintained

**Given** I view a report
**When** Report shows information from multiple sources
**Then** Each data point shows its author
**And** I can see when information was added or modified
**And** Edit history is available if applicable

**Given** Traceability is implemented
**When** Information is included in reports
**Then** All authorship information is preserved
**And** No data appears without source attribution
**And** Traceability information is clearly displayed

#### Story 8.9: Implementar exportación y guardado de reportes generados

As a user,
I want to export or save generated reports,
So that I can share and archive analysis results.

**FRs covered:** FR76

**Acceptance Criteria:**

**Given** A report is generated
**When** I export the report
**Then** I can export in multiple formats (PDF, Excel, CSV, etc.)
**And** Export includes all report data and metadata
**And** Export completes successfully
**And** File is downloaded to my computer

**Given** I want to save a report
**When** I save the report
**Then** Report is saved with a name
**And** Report configuration is saved (filters, perspectives, etc.)
**And** Saved reports can be viewed later
**And** Saved reports can be re-exported

**Given** Reports are saved
**When** I view saved reports
**Then** I can see list of saved reports
**And** I can view saved report details
**And** I can delete saved reports
**And** I can regenerate reports from saved configurations

### Epic 9: Reporting & Analytics Dashboard

Office managers can view summaries and statistics of specialist activity and information completeness. This epic provides administrative insights.

**FRs covered:** FR67-FR68

**User Outcome:** Office managers can view a dashboard showing specialist activity and statistics about information completeness across entities.

**Implementation Notes:**

- Dashboard component for office managers
- Activity summaries
- Completeness statistics
- Role-based access (Office Manager only)

#### Story 9.1: Implementar dashboard de resumen de actividad de especialistas

As an office manager,
I want to view a summary/dashboard of specialist activity,
So that I can monitor work progress and productivity.

**FRs covered:** FR67

**Acceptance Criteria:**

**Given** I am an office manager
**When** I access the dashboard
**Then** Dashboard shows summary of specialist activity
**And** Activity metrics are displayed (entities created, information added, documents uploaded, etc.)
**And** Activity is grouped by specialist or department
**And** Time period can be selected (daily, weekly, monthly)
**And** Dashboard loads within reasonable time

**Given** I want to view specialist activity
**When** I use the dashboard
**Then** I can see activity by individual specialist
**And** I can see activity by department
**And** I can filter by date range
**And** Activity trends are shown (charts, graphs)

**Given** Dashboard shows activity summaries
**When** I view the data
**Then** Activity counts are accurate
**And** Data is up-to-date
**And** I can drill down into details
**And** I can export activity reports

#### Story 9.2: Implementar estadísticas de completitud de información

As an office manager,
I want to view statistics of information completeness,
So that I can identify gaps and prioritize work.

**FRs covered:** FR68

**Acceptance Criteria:**

**Given** Entities have information from categories
**When** I view completeness statistics
**Then** Statistics show percentage of completeness for entities
**And** Completeness is calculated based on required properties in categories
**And** Statistics are grouped by entity type, category, or department
**And** Completeness trends are shown over time

**Given** I want to analyze information completeness
**When** I use the statistics dashboard
**Then** I can see overall completeness percentage
**And** I can see completeness by entity type
**And** I can see completeness by category
**And** I can identify entities with low completeness

**Given** Completeness statistics are displayed
**When** I view the data
**Then** Statistics are accurate and up-to-date
**And** Completeness calculation considers required vs optional properties
**And** I can drill down to see which entities need more information
**And** I can export completeness reports

**Given** Dashboard is implemented
**When** I access it
**Then** Only office managers can access (role-based access)
**And** Unauthorized users receive 403 Forbidden
**And** Dashboard is clearly organized and easy to understand
