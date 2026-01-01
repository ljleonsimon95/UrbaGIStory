---
stepsCompleted: [1, 2, 3, 4, 6, 7, 8, 9, 10, 11]
inputDocuments:
  - project-planning-artifacts/product-brief-UrbaGIStory-2025-12-25.md
  - project-planning-artifacts/research/technical-stack-mapas-research-2025-12-25.md
  - analysis/brainstorming-session-2025-12-25.md
documentCounts:
  briefs: 1
  research: 1
  brainstorming: 1
  projectDocs: 0
workflowType: 'prd'
lastStep: 11
project_name: 'UrbaGIStory'
user_name: 'AllTech'
date: '2025-12-25'
functionalRequirementsCount: 77
nonFunctionalRequirementsCount: 18
status: 'complete'
---

# Product Requirements Document - UrbaGIStory

**Author:** AllTech
**Date:** 2025-12-25

---

## Executive Summary

**UrbaGIStory** es un Sistema de Información Geográfica (GIS) especializado diseñado para la gestión del patrimonio histórico y el planeamiento urbano en la Red de Oficinas del Conservador de Cuba.

El sistema resuelve un problema crítico en el sector gubernamental: actualmente no existe ninguna herramienta que permita a los especialistas de patrimonio histórico y planeamiento urbano agrupar información de forma homogénea, analizar datos y tener una visión geográfica unificada para la toma de decisiones. La información está dispersa entre departamentos (Patrimonio Histórico, Plan Maestro, Planificación), cada uno con sus propios métodos, resultando en datos fragmentados e inconsistentes sobre las mismas entidades urbanas.

UrbaGIStory proporciona una plataforma centralizada donde los especialistas pueden acceder a través de un mapa interactivo a toda su zona de trabajo, consultar información de cualquier entidad urbana (inmuebles, plazas, calles, boulevards, líneas de fachada, manzanas, etc.), gestionar documentos históricos con metadata temporal, y filtrar por múltiples variables dinámicas para análisis geográficos y toma de decisiones.

**Impacto:** El sistema facilita la preservación del patrimonio histórico y mejora la planificación urbana mediante información consolidada y accesible, abriendo un nuevo diapasón de trabajo para los departamentos y permitiendo capacidades que hasta ahora eran imposibles.

**Stack Tecnológico:** .NET 10 + Blazor WebAssembly + PostgreSQL/PostGIS + OpenLayers + QGIS

### What Makes This Special

UrbaGIStory se diferencia de las soluciones existentes a través de:

1. **Facilidad de uso**: Diseñado para ser intuitivo y fácil de entender, permitiendo que especialistas sin experiencia técnica previa puedan utilizarlo efectivamente después de capacitación.

2. **Personalización total**: Sistema de categorías dinámicas que funcionan como plantillas de trabajo, permitiendo variables dinámicas, tipos configurables y filtros flexibles que se adaptan a las necesidades específicas de cada oficina.

3. **Dominio específico**: Construido por y para especialistas de patrimonio y urbanismo, entendiendo profundamente las necesidades del sector gubernamental cubano y el trabajo de preservación del patrimonio histórico.

4. **Producto completo**: No es un MVP iterativo—es una solución sólida y de bajo mantenimiento diseñada para ser entregada completa desde el inicio, habilitando un nuevo diapasón de trabajo para los departamentos.

5. **Open Source**: Compartido con la comunidad para beneficio del sector, permitiendo que otras oficinas y organizaciones se beneficien de la solución.

6. **Sin competencia**: No existe ninguna herramienta similar en el mercado que aborde las necesidades específicas del urbanismo patrimonial cubano con el nivel de especialización requerido.

## Project Classification

**Technical Type:** Web App (Single Page Application)
- Blazor WebAssembly frontend
- .NET 10 Web API backend
- PostgreSQL/PostGIS database
- OpenLayers for interactive mapping

**Domain:** GovTech (Government Technology)
- Public sector application
- Municipal/civic management
- Heritage and urban planning domain
- Government office workflow integration

**Complexity:** High
- Integration with existing GIS tools (QGIS) - geometrías creadas en QGIS, visualizadas y gestionadas en UrbaGIStory
- Complex spatial data management (PostGIS)
- Dynamic categorization and templating system
- Multi-user collaboration with role-based permissions
- Historical document management with temporal metadata
- Government sector compliance and security considerations

**Project Context:** Greenfield - new project

**Validation Approach:** El sistema será validado con usuarios beta representativos de cada tipo de usuario (especialistas en planeamiento, patrimonio histórico, y jefe de oficina) para asegurar que resuelve el problema real y se integra efectivamente en su flujo de trabajo diario.

**Key Technical Considerations:**
- Spatial data processing and visualization
- Real-time collaboration capabilities
- Document storage and metadata management
- Dynamic form generation based on categories
- Integration with external GIS desktop tools (QGIS) - las geometrías se crean en QGIS y se gestionan en UrbaGIStory
- Role-based access control and permissions
- Performance optimization for large spatial datasets

---

## Success Criteria

### User Success

**Primary Success Indicators:**

1. **Information Linked Correctly**
   - Users can see documents from different specialists in the same entity view
   - Multiple departments' information appears linked in one place
   - Success moment: When Ernesto reviews an entity and sees information that Amalia uploaded, or when the boss consults an entity and sees valuable data from both specialists linked together

2. **Regular System Usage**
   - Users access the system monthly with consistent frequency
   - System becomes integrated into daily workflow
   - Success indicator: Regular and consistent use by specialists, indicating the system has become part of their daily work routine

3. **Document Digitalization**
   - Progressive increase in documents attached to the system
   - Historical documents are digitized and linked to entities
   - Success indicator: Constant increase in number of documents attached, indicating progressive digitalization and enrichment of system information

**User Success Metrics:**
- Percentage of urban entities with information from multiple specialists/departments
- Number of sessions per user per month
- Total documents attached per period (monthly/annual)
- Documents by type (historical photos, documents, regulations, etc.)

### Business Success

**Business Objectives:**

1. **Heritage Preservation Impact**
   - Facilitate heritage preservation through consolidated and accessible information
   - Enable identification of heritage at risk for prioritized interventions
   - Success metrics: State of conservation tracking, completeness of historical records

2. **Urban Planning Impact**
   - Facilitate urban planning decisions through geographic analysis and data correlation
   - Enable comprehensive analysis combining different perspectives (architectural, heritage, urbanistic)
   - Success metrics: Geographic analyses performed, time to access correlated information, coverage of information per entity

3. **Operational Efficiency Impact**
   - Improve department efficiency through unified methodology and centralized information access
   - Enable work that was previously impossible
   - Success metrics: Information linked correctly, frequency of use, number of documents attached

**Business Success Timeline:**
- **3 months:** Users beta can complete their daily workflow using the system
- **12 months:** System becomes the single source of truth for the department, with regular geographic analyses for decision-making

### Technical Success

**Critical Technical Requirements:**

1. **QGIS Integration (CRITICAL)**
   - Geometries created in QGIS are correctly visualized in UrbaGIStory map
   - Data synchronization between QGIS and UrbaGIStory works without issues
   - Users can work seamlessly without interruptions
   - Success criteria:
     - Geometries display correctly in the interactive map
     - No data loss during synchronization
     - Users can select entities created from QGIS geometries without errors

2. **Spatial Data Management**
   - PostGIS database handles spatial queries efficiently
   - Map performance remains acceptable with large number of entities
   - Spatial data integrity maintained

3. **System Reliability**
   - System availability sufficient for daily operations
   - Multiple users can work simultaneously without conflicts
   - Document storage and retrieval functions correctly

4. **Dynamic Categorization System**
   - Category system functions as work templates
   - Properties display automatically based on assigned categories
   - Filtering system works with dynamic categories

**Technical Success Metrics:**
- QGIS integration works correctly (geometries visible, no sync errors)
- Map loads and displays entities within acceptable time (< 3 seconds for initial load)
- Document upload and retrieval functions correctly
- Category system displays properties correctly based on entity type
- Multiple concurrent users can work without data conflicts

### Measurable Outcomes

**Validation Approach:**
- Users beta of each type (Ernesto - urban planning specialist, Amalia - heritage specialist, boss - administrator) can use the system
- Users beta can complete their daily workflow using the system
- Feedback from users beta indicates the system solves the problem

**Success Validation:**
- All core functionalities are implemented and working
- Users can complete the full flow: select entity → see available properties → add information → attach documents → filter results
- System dictates unified methodology through categories

---

## Product Scope

### MVP - Minimum Viable Product

**Core Features:**

1. **Interactive Map**
   - Visualization of geometries created in QGIS
   - Selection of urban entities on the map
   - Geographic navigation through work zone
   - **Note:** Map is NOT for major geometric modifications - geometries are created exclusively in QGIS

2. **Entity Management**
   - Create entities based on existing geometries (created in QGIS)
   - Assign entity type (building, plaza, street, boulevard, facade line, block, etc.)
   - Add information to entities (variables, metrics, metadata)
   - System automatically displays available properties based on categories assigned to entity type
   - Query information from existing entities

3. **Document Management**
   - Attach documents to urban entities
   - Manage documents related to each entity
   - Temporal metadata for documents (dates, types, authors)
   - Visualization of documents linked to each entity

4. **Category and Filtering System**
   - Dynamic category system that functions as work templates
   - Predefined categories loaded at application startup
   - Create and edit categories (only advanced privilege users - office manager)
   - Assign categories to entity types
   - Properties within each category that define what information to collect
   - System automatically displays available properties based on assigned categories
   - Robust dynamic filtering based on categories and properties
   - Filter by any criteria (categories, properties, entity type, etc.)

5. **Permissions and Roles System**
   - Differentiated roles: Office manager (advanced privileges) vs. Specialists (normal users)
   - Office manager can create/edit categories and assign them
   - Specialists can use existing categories and populate properties
   - Multiple specialists can work on the same entity simultaneously
   - Concurrency handled simply (synchronous application)

**Required Setup (Pre-MVP):**
- QGIS → PostgreSQL/PostGIS connection
- Database table creation
- Initial system configuration
- Loading of predefined categories at application startup

**MVP Success Criteria:**
- All core features implemented and functioning
- QGIS integration works correctly
- Users beta can complete their daily workflow
- System dictates unified methodology through categories

### Growth Features (Post-MVP)

**Enhancements for Future Versions:**

1. **Audit View**
   - Dashboard for office manager
   - Review of information uploaded by specialists
   - Completeness verification

2. **Advanced Analysis**
   - Automated executive reports
   - Predictive analysis (deterioration, trends)
   - Advanced visualizations

3. **Additional Integrations**
   - Integration with other GIS systems
   - APIs for integration with external systems
   - Data export in multiple formats

4. **Collaboration Features**
   - Notifications between specialists
   - Collaborative comments and annotations
   - Detailed change history

### Vision (Future)

**Long-term Product Vision:**

UrbaGIStory evolves into a comprehensive platform for heritage and urban management that:

- Becomes the standard tool for heritage offices across Cuba
- Enables cross-office collaboration and data sharing
- Provides advanced analytics and predictive capabilities
- Integrates with national heritage databases
- Supports mobile field data collection
- Enables public access to heritage information (where appropriate)

**Note:** The MVP is designed to be a complete and functional product that solves the core problem. Future enhancements will be added based on user beta feedback and emerging needs.

---

## User Journeys

### Journey 1: Ernesto - Encontrando la Metodología que Necesitaba

Ernesto es especialista en planeamiento urbano dentro de la Red de Oficinas del Conservador. Lleva semanas trabajando en el análisis de un boulevard histórico, pero no tiene indicaciones claras sobre cómo estructurar su trabajo. Recopila datos de campo y revisa documentos, pero todo queda en archivos dispersos. Sabe que Amalia, del departamento de patrimonio, está trabajando en el mismo boulevard, pero no puede ver su información ni correlacionar datos.

Un lunes por la mañana, después de la capacitación, Ernesto abre UrbaGIStory por primera vez. Ve el mapa interactivo de su zona de trabajo. Hace click en el boulevard que está analizando. El sistema muestra automáticamente las propiedades disponibles según las categorías que el jefe de oficina asignó a este tipo de entidad. Por primera vez, tiene indicaciones claras sobre qué información debe recopilar.

Ernesto añade las regulaciones urbanísticas del 2025 que recopiló en campo, adjunta el documento PDF con las normativas, y completa las propiedades requeridas. Al guardar, ve algo que nunca había visto antes: una foto histórica del boulevard de 1904 que Amalia subió la semana pasada. Por primera vez, puede ver información de otro departamento sobre la misma entidad, enlazada en un mismo lugar.

El momento clave llega cuando el jefe necesita hacer una investigación sobre el boulevard. Ernesto usa el módulo de filtros para correlacionar variables arquitectónicas con información patrimonial. Genera un reporte que combina ambas perspectivas. El jefe puede tomar decisiones basadas en datos consolidados que antes eran imposibles de obtener.

Seis meses después, Ernesto ya no imagina trabajar sin UrbaGIStory. Su trabajo es estructurado, metodológico, y puede colaborar efectivamente con otros especialistas sin duplicar esfuerzos.

---

### Journey 2: Amalia - Preservando la Memoria Histórica

Amalia es especialista en patrimonio histórico. Trabaja en el mismo boulevard que Ernesto, pero desde una perspectiva cultural y patrimonial. Tiene una foto histórica del boulevard de 1904 que encontró en el archivo, pero no sabe dónde guardarla ni cómo relacionarla con el trabajo de Ernesto. Maneja su archivo de forma independiente, separado del de planeamiento.

Después de la capacitación, Amalia abre UrbaGIStory y navega al boulevard en el mapa. Al seleccionarlo, ve algo inesperado: las regulaciones urbanísticas del 2025 que Ernesto añadió la semana pasada. Por primera vez, puede ver información de otro departamento sobre la misma entidad.

Amalia añade la foto histórica de 1904, completa las propiedades patrimoniales que el sistema muestra según las categorías asignadas, y adjunta el documento histórico con su metadata temporal. Al guardar, todo queda relacionado con la misma entidad. Cuando alguien busque el archivo del boulevard, verá tanto las regulaciones de Ernesto como la foto histórica de Amalia, todo enlazado en un mismo lugar.

El momento clave llega cuando necesita hacer un análisis de evolución del boulevard. Usa el módulo de filtros para correlacionar información patrimonial con datos arquitectónicos. Puede ver cómo ha cambiado el boulevard desde 1904 hasta las regulaciones actuales, combinando ambas perspectivas en un análisis integral que antes era imposible.

Ahora, Amalia puede trabajar de forma colaborativa sin duplicar esfuerzos. Su información patrimonial se preserva digitalmente y está accesible para otros especialistas, contribuyendo a un entendimiento completo del patrimonio urbano.

---

### Journey 3: Carlos - El Jefe que Finalmente Puede Tomar Decisiones Informadas

Carlos es el jefe de oficina. Necesita correlacionar datos de diferentes departamentos para una investigación sobre un área patrimonial, pero no puede tener una visión unificada de lo que están haciendo Ernesto, Amalia y otros especialistas. Cuando necesita información correlacionada, no es posible obtenerla.

Un día, Carlos necesita investigar un boulevard para una decisión de planeamiento. Antes de UrbaGIStory, habría tenido que pedirle a Ernesto sus archivos, luego a Amalia los suyos, y tratar de correlacionarlos manualmente. Ahora, abre UrbaGIStory, selecciona el boulevard en el mapa, y ve toda la información: las regulaciones urbanísticas de Ernesto, la foto histórica de Amalia, todo enlazado en un mismo lugar.

Carlos usa el módulo de análisis para filtrar y correlacionar variables de diferentes departamentos. Puede ver el estado de conservación, las regulaciones actuales, y el contexto histórico, todo en una vista unificada. Genera un reporte ejecutivo que combina todas las perspectivas, permitiéndole tomar una decisión informada basada en datos consolidados.

Pero el verdadero poder del sistema para Carlos es la moderación. Como jefe de oficina, puede crear y editar categorías que funcionan como plantillas de trabajo, asignar categorías a tipos de entidad, y moderar permisos entre grupos de usuarios y entidades. Esto le permite dictar la metodología unificada que el departamento necesita, asegurando que todos trabajen de forma estructurada y consistente.

Seis meses después, Carlos tiene una visión global del estado del trabajo de todo el departamento. Puede identificar áreas con información incompleta, priorizar intervenciones en patrimonio en riesgo, y tomar decisiones de planeamiento urbano basadas en datos reales y consolidados.

---

### Journey 4: Roberto - El Administrador Técnico que Hace que Todo Funcione

Roberto es el administrador técnico del sistema. Es responsable de que UrbaGIStory funcione correctamente. Antes del lanzamiento, configura la conexión QGIS → PostgreSQL/PostGIS, crea las tablas en la base de datos, y carga las categorías predefinidas al inicio de la aplicación.

El día del lanzamiento, Roberto crea los usuarios nuevos en el sistema: Ernesto, Amalia, Carlos (el jefe), y otros especialistas. Asigna roles y permisos según las necesidades del departamento. Configura la autenticación y gestiona las contraseñas iniciales.

Durante las primeras semanas, Roberto da soporte técnico cuando hay problemas. Un día, Ernesto reporta que no puede ver las geometrías en el mapa. Roberto verifica la conexión con QGIS y PostGIS, encuentra que hay un problema de sincronización, y lo resuelve. Las geometrías vuelven a aparecer correctamente.

A medida que el sistema se usa más, Roberto monitorea el rendimiento, verifica que la integración con QGIS funcione sin problemas, y asegura que múltiples usuarios puedan trabajar simultáneamente sin conflictos. Cuando hay actualizaciones o mantenimiento, Roberto las gestiona sin interrumpir el trabajo de los especialistas.

Roberto también capacita a los usuarios en el uso del sistema, asegurándose de que todos entiendan cómo funciona y puedan aprovecharlo al máximo. Su trabajo es invisible cuando todo funciona bien, pero crítico para que UrbaGIStory sea la herramienta confiable que el departamento necesita.

---

### Journey Requirements Summary

Estos journeys revelan los siguientes requisitos funcionales para UrbaGIStory:

**Core Capabilities:**
- **Onboarding y Capacitación**: Proceso de capacitación para nuevos usuarios del sistema
- **Gestión de Entidades**: Crear entidades basadas en geometrías, asignar tipos, añadir información basada en categorías
- **Gestión de Documentos**: Adjuntar documentos con metadata temporal a entidades urbanas
- **Sistema de Categorías Dinámicas**: Crear, editar, asignar categorías que funcionan como plantillas de trabajo
- **Sistema de Permisos Dinámicos**: Asignar permisos entre grupos de usuarios y entidades (lectura/escritura)
- **Filtrado y Análisis**: Filtrar y correlacionar variables para análisis geográficos y toma de decisiones
- **Integración QGIS**: Visualización y sincronización correcta de geometrías creadas en QGIS
- **Gestión de Usuarios**: Crear usuarios, asignar roles, gestionar autenticación y contraseñas
- **Soporte Técnico**: Resolución de problemas, monitoreo, mantenimiento del sistema

**Nota Importante:**
Aunque puede haber usuarios de diferentes tipos o categorías, el flujo base es consistente para todos: **acceder al archivo de la entidad → consultar lo que hay → añadir datos**. El sistema de categorías y permisos dinámicos permite variaciones sin necesidad de journeys separados, haciendo que el sistema sea fácil de usar y flexible para diferentes necesidades.

---

## Innovation & Novel Patterns

### Detected Innovation Areas

UrbaGIStory desafía el supuesto de que no puedes tener todas las potencialidades juntas en una solución GIS especializada para patrimonio histórico y planeamiento urbano. La innovación radica en la combinación única de:

1. **Aplicación Web Nativa (Blazor WebAssembly) con Control Total**
   - Aplicación nativa que el desarrollador puede personalizar completamente
   - Stack tecnológico moderno (.NET 10 + Blazor WASM) que permite control total sobre la experiencia de usuario
   - No está limitado por las restricciones de herramientas GIS genéricas

2. **Integración Simple y Paralela con QGIS**
   - **Arquitectura de Datos Separada**: Las tablas que utiliza QGIS/PostGIS para las geometrías se mantienen **aparte** en la misma base de datos
   - Las tablas del sistema UrbaGIStory se mantienen **también aparte**, funcionando paralelamente
   - **Enlace Simple**: La única relación crítica es un enlace entre la entidad y la tabla donde están las geometrías
   - **Operación Paralela**: QGIS y UrbaGIStory pueden funcionar paralelamente sin interferirse:
     - Desde QGIS no se tocan datos que se operan desde UrbaGIStory
     - Desde UrbaGIStory no se tocan datos o tablas que se operan desde QGIS
   - **Relación Flexible**: Una entidad puede tener una geometría O puede no tenerla. Una geometría puede tener muchas entidades (caso del contenedor con varios edificios adentro)
   - Esta arquitectura simplifica significativamente la integración y reduce el riesgo técnico

3. **Modelo de Datos Híbrido Flexible (SQL + JSONB)**
   - Estructura SQL para datos estructurados (entidades, relaciones, metadata)
   - JSONB para variables dinámicas y métricas configurables
   - Permite flexibilidad total sin sacrificar rendimiento ni integridad de datos

4. **Sistema de Categorías Dinámicas como Plantillas de Trabajo**
   - Las categorías funcionan como plantillas que dictan qué información recopilar
   - Permite metodología unificada sin hardcodear esquemas
   - Sistema completamente configurable por usuarios privilegiados

### Market Context & Competitive Landscape

**No existe ninguna herramienta similar en el mercado** que combine estas capacidades específicamente para el dominio de patrimonio histórico y planeamiento urbano:

- **Herramientas GIS genéricas** (ArcGIS, QGIS): No abordan el modelo de datos flexible necesario para patrimonio histórico, ni la gestión de documentos con contexto temporal, ni el concepto de agregación/desagregación de inmuebles a lo largo del tiempo.

- **Sistemas de gestión patrimonial**: No tienen capacidades GIS integradas ni el enfoque geográfico necesario para planeamiento urbano.

- **Soluciones de planeamiento urbano**: No manejan la complejidad del patrimonio histórico ni el modelo de datos flexible requerido.

UrbaGIStory es la primera solución que combina todas estas potencialidades en una herramienta especializada para el urbanismo patrimonial cubano.

### Validation Approach

La validación de los aspectos innovadores se realizará mediante:

1. **Pruebas de Integración QGIS**
   - Verificar que las geometrías creadas en QGIS se visualizan correctamente en UrbaGIStory
   - Validar que el enlace entre entidades y geometrías funciona correctamente
   - Asegurar que QGIS y UrbaGIStory pueden funcionar paralelamente sin interferencias
   - Probar que los usuarios pueden seleccionar entidades enlazadas a geometrías de QGIS sin errores

2. **Validación con Usuarios Beta**
   - Usuarios beta representativos de cada tipo de usuario (especialistas en planeamiento, patrimonio histórico, y jefe de oficina)
   - Validar que el sistema resuelve el problema real
   - Verificar que se integra efectivamente en su flujo de trabajo diario
   - Confirmar que el sistema de categorías dinámicas dicta metodología unificada como se espera

3. **Validación Técnica del Modelo de Datos Híbrido**
   - Probar que el modelo SQL + JSONB maneja variables dinámicas correctamente
   - Verificar rendimiento con grandes volúmenes de datos espaciales
   - Validar que el sistema de filtrado funciona con categorías dinámicas

4. **Pruebas de Carga**
   - Probar el sistema con grandes volúmenes de datos espaciales
   - Validar rendimiento de PostGIS con múltiples geometrías
   - Verificar que la arquitectura de datos separada mantiene buen rendimiento

### Risk Mitigation

Los aspectos más arriesgados y sus estrategias de mitigación:

1. **Sistema de Categorías Dinámicas**
   - **Riesgo**: Que el sistema no genere correctamente las propiedades basadas en categorías asignadas
   - **Mitigación**: Pruebas exhaustivas con diferentes combinaciones de categorías, validación con usuarios beta, y sistema de fallback que muestre propiedades básicas si hay errores

2. **Modelo de Datos Híbrido (SQL + JSONB)**
   - **Riesgo**: Problemas de rendimiento, integridad de datos, o complejidad en el filtrado
   - **Mitigación**: Pruebas de rendimiento con datos reales, índices optimizados en JSONB, y validación de integridad de datos en cada operación
   - **Documentación**: Documentar claramente qué va en SQL vs JSONB para mantener consistencia

3. **Integración QGIS (Riesgo Reducido)**
   - **Riesgo**: Problemas con el enlace entre entidades y geometrías
   - **Mitigación**: La arquitectura de datos separada y operación paralela reduce significativamente el riesgo. El enlace simple entre entidades y geometrías es más fácil de validar y mantener que una sincronización bidireccional compleja
   - **Validación**: Pruebas de enlace, verificación de que las geometrías se visualizan correctamente, y confirmación de que QGIS y UrbaGIStory pueden funcionar paralelamente

**Estrategia de Fallback:**
Si alguno de estos aspectos innovadores no funciona como se espera, el sistema tiene capacidades básicas que permiten continuar operando mientras se resuelven los problemas. Por ejemplo, si el sistema de categorías dinámicas falla, los usuarios pueden seguir trabajando con propiedades básicas predefinidas. Si hay problemas con el enlace QGIS, las entidades pueden funcionar sin geometrías asociadas.

**Criterios de Éxito Específicos:**
- **Integración QGIS**: 100% de geometrías enlazadas se visualizan correctamente, 0% de interferencias entre QGIS y UrbaGIStory
- **Categorías Dinámicas**: 95%+ de usuarios completan información usando categorías asignadas
- **Modelo Híbrido**: Tiempo de respuesta < 3 segundos en consultas con variables dinámicas

---

## Web App Specific Requirements

### Project-Type Overview

UrbaGIStory es una Single Page Application (SPA) construida con Blazor WebAssembly que se ejecuta localmente en un servidor dentro de la empresa, accesible vía localhost. La aplicación está diseñada para ser utilizada principalmente en desktop, con soporte para múltiples navegadores modernos.

**Arquitectura de Despliegue:**
- Despliegue local en servidor dentro de la empresa
- Acceso vía localhost (red local)
- Despliegue sencillo sin complejidades de infraestructura cloud
- Stack: .NET 10 + Blazor WebAssembly + PostgreSQL/PostGIS + OpenLayers

### Browser Support Matrix

**Estrategia de Soporte:**
- Soporte lo más genérico posible para cualquier navegador moderno
- No hay restricciones específicas de navegadores en el entorno gubernamental cubano
- Blazor WebAssembly es compatible con navegadores modernos que soportan WebAssembly

**Navegadores Soportados:**
- Chrome (últimas 2 versiones)
- Firefox (últimas 2 versiones)
- Edge (últimas 2 versiones)
- Safari (últimas 2 versiones) - si se usa en macOS

**Consideraciones Técnicas:**
- Blazor WebAssembly requiere navegadores con soporte WebAssembly
- No se requiere soporte para navegadores legacy o muy antiguos
- La aplicación debe funcionar correctamente en cualquier navegador moderno

### Responsive Design

**Enfoque Inicial:**
- **Desktop First**: La aplicación está diseñada inicialmente solo para desktop
- No se requiere diseño responsivo completo para móviles/tablets en la versión inicial
- Interfaz optimizada para pantallas de escritorio (mínimo 1024x768, recomendado 1920x1080)

**Consideraciones de Diseño:**
- Interfaz de mapa interactivo requiere espacio de pantalla adecuado
- Formularios y gestión de entidades optimizados para mouse y teclado
- Diseño responsivo puede considerarse para versiones futuras si hay necesidad

### Performance Targets

**Contexto de Red:**
- Aplicación ejecutándose en red local (localhost)
- No hay problemas de latencia de red externa
- Rendimiento debe ser bueno debido a la proximidad del servidor

**Objetivos de Rendimiento:**
- **Tiempo de Carga Inicial**: < 5 segundos para cargar la aplicación Blazor WASM
- **Tiempo de Respuesta de API**: < 1 segundo para operaciones estándar (red local)
- **Carga de Mapa**: < 3 segundos para visualizar geometrías iniciales
- **Operaciones de Filtrado**: < 2 segundos para aplicar filtros y actualizar mapa
- **Gestión de Entidades**: < 1 segundo para abrir/cerrar vistas de entidades

**Optimizaciones:**
- Carga lazy de componentes Blazor
- Caché de datos espaciales frecuentemente accedidos
- Optimización de consultas PostGIS para grandes volúmenes de datos
- Compresión de respuestas API cuando sea apropiado

**Volumen de Datos:**
- No se esperan muchos usuarios editando geometrías simultáneamente
- La mayoría de usuarios que trabajan con QGIS son minoría/parte técnica
- El sistema debe manejar eficientemente el volumen de entidades y geometrías sin problemas de rendimiento

### SEO Strategy

**Estrategia:**
- **SEO No Requerido**: La aplicación no estará publicada públicamente
- Aplicación interna accesible solo vía localhost
- No hay necesidad de optimización para motores de búsqueda
- No se requiere contenido indexable públicamente

**Consideraciones:**
- Meta tags básicos pueden incluirse para completitud, pero no son críticos
- No se requiere sitemap ni robots.txt
- No se requiere Open Graph o Twitter Cards

### Real-Time & Collaboration Features

**Enfoque Inicial:**
- **Colaboración Simultánea**: Manejada de forma sencilla (aplicación síncrona)
- Múltiples usuarios pueden trabajar en la misma entidad simultáneamente
- No se requiere sincronización en tiempo real para MVP

**Funcionalidades Futuras (Segunda Versión):**
- **Panel de Notificaciones**: Interesante tener notificaciones entre usuarios
- **Colaboración en Tiempo Real**: Ver hasta dónde se puede llegar con notificaciones y actualizaciones en tiempo real
- Estas funcionalidades pueden considerarse para versiones futuras basadas en feedback de usuarios beta

**Consideraciones Técnicas:**
- Para MVP, la colaboración se maneja de forma síncrona simple
- Si se implementan notificaciones en el futuro, se puede usar SignalR (compatible con Blazor)
- Las notificaciones pueden incluir: cambios en entidades, nuevos documentos, actualizaciones de categorías

### Accessibility Level

**Enfoque:**
- **Accesibilidad Básica**: No hay requisitos específicos de accesibilidad para software gubernamental en Cuba
- La aplicación debe ser funcional y usable, pero no se requiere cumplimiento estricto de estándares WCAG
- Se seguirán buenas prácticas básicas de accesibilidad web cuando sea posible sin agregar complejidad

**Consideraciones Básicas:**
- Navegación por teclado básica donde sea apropiado
- Contraste de colores razonable para legibilidad
- Etiquetas semánticas HTML cuando sea posible
- No se requiere soporte completo para lectores de pantalla o tecnologías asistivas avanzadas

### Technical Architecture Considerations

**Single Page Application (SPA):**
- Blazor WebAssembly proporciona experiencia SPA completa
- Navegación del lado del cliente sin recargas de página
- Estado de la aplicación mantenido en el cliente
- Comunicación con backend vía API REST

**Integración con QGIS:**
- Conexión QGIS → PostgreSQL/PostGIS para gestión de geometrías
- UrbaGIStory lee geometrías desde las mismas tablas PostGIS
- Operación paralela: QGIS y UrbaGIStory no interfieren entre sí
- Enlace simple entre entidades y geometrías

**OpenLayers Integration:**
- OpenLayers para visualización de mapas interactivos
- JS Interop para integración Blazor WASM con OpenLayers
- Renderizado de geometrías PostGIS en el mapa
- Interacción del usuario con el mapa (selección, zoom, pan)

### Implementation Considerations

**Despliegue Local:**
- Despliegue sencillo en servidor local
- Sin complejidades de infraestructura cloud
- Configuración de base de datos PostgreSQL/PostGIS en el mismo servidor o servidor cercano
- Acceso vía localhost simplifica configuración de red y seguridad

**Desarrollo y Mantenimiento:**
- Stack tecnológico moderno (.NET 10) facilita desarrollo y mantenimiento
- Blazor WebAssembly permite desarrollo full-stack con C#
- Código compartido entre frontend y backend cuando sea apropiado
- Herramientas de desarrollo estándar de .NET

**Escalabilidad:**
- Sistema diseñado para manejar el volumen esperado de usuarios y datos
- Arquitectura permite escalar si es necesario en el futuro
- Optimizaciones de rendimiento implementadas desde el inicio

---

## Project Scoping & Phased Development

### MVP Strategy & Philosophy

**MVP Approach: Complete Product from Day One**

UrbaGIStory sigue una filosofía de **"producto completo desde el inicio"** en lugar de un MVP mínimo iterativo. Esta estrategia asegura que:

- El sistema resuelve el problema completo desde el primer día
- La arquitectura está diseñada para escalar sin problemas futuros
- Los usuarios reciben una solución sólida y de bajo mantenimiento
- No hay necesidad de refactorización mayor después del lanzamiento
- El sistema habilita un nuevo diapasón de trabajo desde el inicio

**Racional:**
- Es la primera vez trabajando con este tipo de herramientas, por lo que es mejor construir correctamente desde el inicio
- Un MVP mínimo podría requerir refactorización costosa para escalar después
- El problema requiere una solución completa para ser realmente útil
- La arquitectura de datos separada (QGIS/UrbaGIStory) y el modelo híbrido (SQL + JSONB) permiten flexibilidad sin comprometer la estructura base

**Resource Requirements:**
- **Team Size**: Pequeño-mediano (1-3 desarrolladores)
- **Skills Needed**: Full-stack .NET, PostgreSQL/PostGIS, Blazor WebAssembly, OpenLayers, GIS knowledge
- **Timeline**: Producto completo diseñado para ser entregado en una fase, no iterativo

### MVP Feature Set (Phase 1 - Complete Product)

**Core User Journeys Supported:**
- ✅ Ernesto (Especialista en Planeamiento) - Añadir información arquitectónica/urbanística
- ✅ Amalia (Especialista en Patrimonio) - Añadir información patrimonial/cultural
- ✅ Carlos (Jefe de Oficina) - Moderar categorías, permisos, y tomar decisiones informadas
- ✅ Roberto (Administrador Técnico) - Setup, configuración, y soporte

**Must-Have Capabilities (MVP Completo):**

1. **Interactive Map (Core)**
   - Visualización de geometrías creadas en QGIS
   - Selección de entidades urbanas en el mapa
   - Navegación geográfica por zona de trabajo
   - Integración con OpenLayers vía JS Interop

2. **Entity Management (Core)**
   - Crear entidades basadas en geometrías existentes (QGIS)
   - Asignar tipo de entidad (inmueble, plaza, calle, boulevard, línea de fachada, manzana, etc.)
   - Añadir información a entidades (variables, métricas, metadata)
   - Sistema muestra automáticamente propiedades disponibles basadas en categorías asignadas
   - Consultar información de entidades existentes

3. **Document Management (Core)**
   - Adjuntar documentos a entidades urbanas
   - Gestionar documentos relacionados con cada entidad
   - Metadata temporal para documentos (fechas, tipos, autores)
   - Visualización de documentos enlazados a cada entidad

4. **Category and Filtering System (Core - Critical)**
   - Sistema de categorías dinámicas que funcionan como plantillas de trabajo
   - Categorías predefinidas cargadas al inicio de la aplicación
   - Crear y editar categorías (solo usuarios con privilegios avanzados - jefe de oficina)
   - Asignar categorías a tipos de entidad
   - Propiedades dentro de cada categoría que definen qué información recopilar
   - Sistema muestra automáticamente propiedades disponibles basadas en categorías asignadas
   - Filtrado dinámico robusto basado en categorías y propiedades
   - Filtrar por cualquier criterio (categorías, propiedades, tipo de entidad, etc.)

5. **Permissions and Roles System (Core)**
   - Roles diferenciados: Jefe de oficina (privilegios avanzados) vs. Especialistas (usuarios normales)
   - Jefe de oficina puede crear/editar categorías y asignarlas
   - Especialistas pueden usar categorías existentes y poblar propiedades
   - Múltiples especialistas pueden trabajar en la misma entidad simultáneamente
   - Concurrencia manejada de forma sencilla (aplicación síncrona)
   - Permisos dinámicos entre grupos de usuarios y entidades

6. **QGIS Integration (Core - Critical)**
   - Conexión QGIS → PostgreSQL/PostGIS para gestión de geometrías
   - Enlace simple entre entidades y geometrías
   - Operación paralela: QGIS y UrbaGIStory no interfieren entre sí
   - Visualización correcta de geometrías en el mapa

7. **User Management (Core)**
   - Crear usuarios nuevos en el sistema
   - Asignar roles y permisos
   - Gestionar autenticación y contraseñas
   - Configuración técnica (administrador técnico)

**Required Setup (Pre-MVP):**
- QGIS → PostgreSQL/PostGIS connection
- Database table creation (tablas separadas para QGIS y UrbaGIStory)
- Initial system configuration
- Loading of predefined categories at application startup
- Setup de servidor local y despliegue

**MVP Success Criteria:**
- Todas las features core implementadas y funcionando
- Integración QGIS funciona correctamente (geometrías visibles, sin errores de sincronización)
- Usuarios beta pueden completar su flujo de trabajo diario
- Sistema dicta metodología unificada a través de categorías
- Información de múltiples especialistas aparece enlazada en la misma entidad
- Sistema es estable y de bajo mantenimiento

**Launch Strategy:**
- **Soft Launch**: Lanzamiento suave con usuarios beta antes del lanzamiento completo
- Validación con usuarios beta representativos de cada tipo de usuario
- Feedback continuo durante el soft launch para ajustes rápidos
- Lanzamiento completo después de validación exitosa con usuarios beta

### Post-MVP Features (Phase 2 - Growth)

**Enhancements for Future Versions (Basadas en feedback de usuarios beta):**

1. **Audit View & Dashboard**
   - Dashboard para jefe de oficina
   - Revisión de información subida por especialistas
   - Verificación de completitud
   - Métricas de uso y cobertura

2. **Advanced Analysis & Reporting**
   - Reportes ejecutivos automatizados
   - Análisis predictivo (deterioro, tendencias)
   - Visualizaciones avanzadas
   - Correlación avanzada de variables

3. **Real-Time Collaboration Features**
   - Panel de notificaciones entre especialistas
   - Comentarios colaborativos y anotaciones
   - Historial detallado de cambios
   - Actualizaciones en tiempo real (SignalR)

4. **Additional Integrations**
   - Integración con otros sistemas GIS
   - APIs para integración con sistemas externos
   - Exportación de datos en múltiples formatos
   - Importación de datos desde otras fuentes

5. **Mobile & Field Collection**
   - Soporte para recolección de datos en campo
   - Aplicación móvil o PWA para tablets
   - Sincronización offline/online

### Phase 3 - Expansion (Future Vision)

**Long-term Product Vision:**

UrbaGIStory evoluciona hacia una plataforma integral para gestión patrimonial y urbana que:

- Se convierte en la herramienta estándar para oficinas de patrimonio en toda Cuba
- Habilita colaboración entre oficinas y compartición de datos
- Proporciona capacidades analíticas avanzadas y predictivas
- Se integra con bases de datos nacionales de patrimonio
- Soporta recolección de datos móvil en campo
- Habilita acceso público a información patrimonial (donde sea apropiado)

### Risk Mitigation Strategy

**Technical Risks:**

1. **Sistema de Categorías Dinámicas**
   - **Riesgo**: Que el sistema no genere correctamente las propiedades basadas en categorías asignadas
   - **Mitigación**: Pruebas exhaustivas con diferentes combinaciones de categorías, validación con usuarios beta, sistema de fallback con propiedades básicas
   - **Contingencia**: Si falla, usuarios pueden trabajar con propiedades básicas predefinidas mientras se resuelve

2. **Modelo de Datos Híbrido (SQL + JSONB)**
   - **Riesgo**: Problemas de rendimiento, integridad de datos, o complejidad en el filtrado
   - **Mitigación**: Pruebas de rendimiento con datos reales, índices optimizados en JSONB, validación de integridad de datos
   - **Contingencia**: Arquitectura permite migrar datos de JSONB a SQL si es necesario

3. **Integración QGIS**
   - **Riesgo**: Problemas con el enlace entre entidades y geometrías
   - **Mitigación**: Arquitectura de datos separada reduce riesgo significativamente. Pruebas de enlace, verificación de visualización correcta
   - **Contingencia**: Entidades pueden funcionar sin geometrías asociadas si hay problemas

**Market/User Risks:**

1. **Adopción de Usuarios**
   - **Riesgo**: Usuarios no adopten el sistema o encuentren dificultades
   - **Mitigación**: Validación con usuarios beta desde el inicio, capacitación adecuada, diseño intuitivo
   - **Contingencia**: Feedback continuo de usuarios beta para ajustes rápidos

2. **Complejidad del Sistema**
   - **Riesgo**: Sistema demasiado complejo para usuarios sin experiencia técnica
   - **Mitigación**: Diseño intuitivo, capacitación, documentación clara, flujo de usuario simple (acceder → consultar → añadir)
   - **Contingencia**: Simplificar flujos basado en feedback de usuarios beta

**Resource Risks:**

1. **Equipo Pequeño**
   - **Riesgo**: Si hay menos recursos de los planeados
   - **Mitigación**: Arquitectura bien diseñada desde el inicio, código mantenible, documentación clara
   - **Contingencia**: Priorizar features core, postergar features de crecimiento si es necesario

2. **Primera Vez con Este Tipo de Herramientas**
   - **Riesgo**: Curva de aprendizaje puede ser empinada
   - **Mitigación**: Stack tecnológico moderno pero estable (.NET 10, Blazor), buena documentación, comunidad activa
   - **Contingencia**: Enfoque en arquitectura sólida desde el inicio para evitar refactorización costosa después

**Architectural Scalability:**

- **Diseño Escalable desde el Inicio**: Arquitectura de datos separada (QGIS/UrbaGIStory) permite escalar sin problemas
- **Modelo Híbrido Flexible**: SQL + JSONB permite agregar nuevas variables sin cambiar esquema base
- **Sistema de Categorías Dinámico**: Permite adaptarse a nuevas necesidades sin cambios de código
- **Separación de Responsabilidades**: QGIS maneja geometrías, UrbaGIStory maneja información, operación paralela sin conflictos

### Development Phases Summary

**Phase 1: Complete MVP (Current Focus)**
- Todas las features core implementadas
- Producto completo y funcional desde el inicio
- Resuelve el problema completo
- Arquitectura escalable
- Validación con usuarios beta

**Phase 2: Growth (Post-MVP)**
- Features basadas en feedback de usuarios beta
- Audit View, Advanced Analysis, Real-Time Collaboration
- Integraciones adicionales
- Mejoras de rendimiento y UX

**Phase 3: Expansion (Future)**
- Plataforma integral para toda Cuba
- Colaboración entre oficinas
- Mobile field collection
- Acceso público (donde sea apropiado)

**Key Principle:** El MVP es un producto completo, no mínimo. La arquitectura está diseñada para escalar sin problemas, permitiendo agregar features futuras sin refactorización mayor.

---

## Functional Requirements

### Geographic Visualization & Navigation

- FR1: Users can view an interactive map showing their work zone
- FR2: Users can visualize geometries created in QGIS on the interactive map
- FR3: Users can select urban entities on the map to view their information
- FR4: Users can navigate geographically through their work zone using map controls (zoom, pan, etc.)
- FR5: Users can see entities displayed on the map based on their geographic location

### Entity Management

- FR6: Users can create entities based on existing geometries created in QGIS
- FR7: Users can assign entity type to created entities (building, plaza, street, boulevard, facade line, block, etc.)
- FR8: Users can view information from existing entities
- FR9: Users can add information to entities (variables, metrics, metadata)
- FR10: The system automatically displays available properties based on categories assigned to the entity type
- FR11: Users can see information from multiple specialists/departments linked to the same entity
- FR12: Users can query information from existing entities
- FR13: An entity can be linked to a geometry or exist without a geometry
- FR14: A geometry can be linked to multiple entities (e.g., container with multiple buildings inside)
- FR61: Users can edit information previously added to entities
- FR62: Users can delete information from entities

### Document Management

- FR15: Users can attach documents to urban entities
- FR16: Users can manage documents related to each entity
- FR17: Users can add temporal metadata to documents (dates, types, authors)
- FR18: Users can view documents linked to each entity
- FR19: Users can see documents from different specialists linked to the same entity
- FR63: Users can delete documents attached to entities
- FR64: Users can preview documents before downloading
- FR65: All documents have a datetime field (fixed or estimated) that can be used for searching
- FR66: Users can search documents by datetime (fixed or estimated) regardless of document type

### Category & Template System

- FR20: The system loads predefined categories at application startup
- FR21: Office manager can create new categories that function as work templates
- FR22: Office manager can edit existing categories
- FR23: Office manager can assign categories to entity types
- FR24: Each category contains properties that define what information to collect
- FR25: The system automatically displays available properties based on categories assigned to entity type
- FR26: Specialists can use existing categories assigned to entity types
- FR27: Specialists can populate properties defined within categories
- FR28: The system dictates unified methodology through categories
- FR70: Office manager can unassign categories from entity types (soft delete strategy - categories remain in system but are not active)

### Filtering & Analysis

- FR29: Users can filter entities by categories
- FR30: Users can filter entities by properties within categories
- FR31: Users can filter entities by entity type
- FR32: Users can filter entities by any combination of criteria (categories, properties, entity type, etc.)
- FR33: Users can correlate variables from different departments for analysis
- FR34: Users can generate reports combining different perspectives (architectural, heritage, urbanistic)
- FR35: The map updates to show filtered results based on selected criteria
- FR76: Users can export or save generated reports
- FR69: Users can see authorship/traceability of information in reports

### User & Permission Management

- FR36: Technical administrator can create new users in the system
- FR37: Technical administrator can assign roles to users (office manager, specialist, technical administrator)
- FR38: Technical administrator can manage user authentication and passwords
- FR39: Office manager can moderate permissions between groups of users and entities
- FR40: Office manager can assign dynamic permissions (read/write) to user groups for specific entities
- FR41: Users with advanced privileges (office manager) can create and edit categories
- FR42: Users with normal privileges (specialists) can use existing categories and populate properties
- FR43: Multiple specialists can work on the same entity simultaneously
- FR44: The system handles concurrent access to entities (synchronous application)
- FR71: Technical administrator can deactivate users without deleting them (soft delete strategy)

### QGIS Integration

- FR45: The system can read geometries from PostgreSQL/PostGIS tables created by QGIS
- FR46: The system displays geometries created in QGIS correctly on the interactive map
- FR47: The system maintains a link between entities and geometries stored in QGIS tables
- FR48: QGIS and UrbaGIStory can operate in parallel without interfering with each other's data
- FR49: Users can select entities that are linked to QGIS geometries without errors

### System Administration & Configuration

- FR50: Technical administrator can configure QGIS → PostgreSQL/PostGIS connection
- FR51: Technical administrator can create database tables for the system
- FR52: Technical administrator can perform initial system configuration
- FR53: Technical administrator can load predefined categories at application startup
- FR54: Technical administrator can monitor system performance
- FR55: Technical administrator can resolve technical issues and provide support
- FR56: Technical administrator can manage system updates and maintenance
- FR72: Technical administrator can backup the database
- FR73: Technical administrator can restore data from backup
- FR74: Technical administrator can view system logs for diagnostics
- FR75: The system provides guided process or documentation for initial setup

### Data & Information Linking

- FR57: The system links information from multiple specialists to the same entity
- FR58: Users can see all information related to an entity in a unified view
- FR59: Information from different departments appears linked in one place for each entity
- FR60: The system maintains data integrity when multiple users work on the same entity
- FR77: The system uses soft delete strategy (deactivate/hide instead of hard delete) to maintain data consistency and avoid referential integrity errors

### Reporting & Analytics

- FR67: Office manager can view summary/dashboard of specialist activity
- FR68: Office manager can view statistics of information completeness

---

## Non-Functional Requirements

### Performance

**Critical Performance Requirements:**

- **NFR1: Entity Selection Response Time**
  - Entity selection on the map must complete within 1 second
  - This is the highest priority performance requirement as it is the most frequently used action
  - Users must be able to select entities and view their information without noticeable delay

- **NFR2: Map Initial Load Time**
  - Interactive map must load and display initial geometries within 3 seconds
  - Map must be usable (zoom, pan, select) even if some geometries are still loading

- **NFR3: Filter Application Response Time**
  - Filtering operations must update the map and results within 2 seconds
  - Users must be able to apply multiple filters without performance degradation

- **NFR4: Document Operations Performance**
  - Document upload must complete within 5 seconds for files up to 10MB
  - Document preview must load within 2 seconds
  - Document download must initiate within 1 second

- **NFR5: Concurrent User Support**
  - System must support up to 10 concurrent users without performance degradation
  - Response times must remain within specified limits with maximum concurrent load
  - System must handle multiple users working on the same entity simultaneously

- **NFR6: Graceful Performance Degradation**
  - If performance is slower than expected, users must be able to continue working
  - System must provide loading indicators for operations that take longer than 2 seconds
  - No operation should block the user interface completely

**Performance Context:**
- Application runs locally on internal network (localhost)
- Minimal dependency on external internet connectivity
- Performance optimizations implemented from the start
- Local network latency is minimal, allowing for good response times

### Security

**Critical Security Requirements:**

- **NFR7: Document Protection**
  - Documents containing sensitive information must be protected from unauthorized access
  - Document access must be controlled by the permission system (FR39-FR40)
  - Documents must be stored securely with appropriate access controls

- **NFR8: Unauthorized Access Prevention**
  - System must prevent unauthorized access to entities, documents, and system configuration
  - Authentication must be required for all system access
  - Role-based access control (FR36-FR42) must be enforced at all access points

- **NFR9: Data Integrity Protection**
  - System must maintain data integrity when multiple users work on the same entity (FR60)
  - Soft delete strategy (FR77) must prevent referential integrity errors
  - System must prevent data corruption from concurrent operations
  - All data modifications must be traceable (FR69 - authorship/traceability)

- **NFR10: User Authentication Security**
  - User authentication must be secure (FR38)
  - Passwords must be stored using industry-standard hashing algorithms
  - User sessions must be managed securely

- **NFR11: Database Security**
  - Database access must be restricted to authorized users and applications only
  - Connection strings and credentials must be stored securely
  - Database backups (FR72) must be stored securely

**Security Context:**
- Government sector application handling heritage and urban planning data
- Documents may contain sensitive historical or planning information
- No specific compliance requirements in Cuba, but following security best practices
- Local deployment reduces some attack vectors but requires proper access controls

### Integration

**Critical Integration Requirements:**

- **NFR12: QGIS Integration Reliability**
  - QGIS integration (FR45-FR49) must be reliable and stable
  - System must handle QGIS connection failures gracefully
  - If QGIS is unavailable, entities without geometries must still be accessible
  - System must maintain data consistency when QGIS and UrbaGIStory operate in parallel (FR48)

- **NFR13: PostGIS Data Integrity**
  - System must correctly read geometries from PostgreSQL/PostGIS tables created by QGIS
  - Data integrity must be maintained when geometries are updated in QGIS
  - Link between entities and geometries (FR47) must remain consistent

- **NFR14: Integration Error Handling**
  - System must provide clear error messages if QGIS connection fails
  - System must log integration errors for diagnostics (FR74)
  - Technical administrator must be able to diagnose integration issues

**Integration Context:**
- QGIS integration is critical for the system to function
- Geometries are created exclusively in QGIS, managed in UrbaGIStory
- Parallel operation architecture reduces integration risks
- Integration must be reliable but not block system operation if temporarily unavailable

### Reliability

**Critical Reliability Requirements:**

- **NFR15: System Availability**
  - System must be available 24/7 for users
  - Planned maintenance should be scheduled during low-usage periods
  - System must recover gracefully from unexpected failures

- **NFR16: Backup and Recovery**
  - Database backups (FR72) must be performed regularly
  - System must support data restoration from backup (FR73)
  - Backup frequency must be sufficient to minimize data loss risk
  - Recovery procedures must be documented and tested

- **NFR17: System Monitoring**
  - Technical administrator must be able to monitor system performance (FR54)
  - System logs (FR74) must be available for diagnostics
  - System must provide alerts for critical failures

- **NFR18: Data Persistence**
  - All user data must be persisted reliably
  - No data loss should occur during normal operations
  - System must handle database connection failures gracefully

**Reliability Context:**
- System is critical for daily operations but not life-critical
- Local deployment simplifies some reliability concerns
- Backup and restore capabilities are essential for data protection
- 24/7 availability ensures users can access the system when needed
