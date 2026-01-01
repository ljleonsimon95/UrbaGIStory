---
stepsCompleted: [1, 2, 3, 4, 5, 6]
inputDocuments:
  - ../prd.md
  - product-brief-UrbaGIStory-2025-12-25.md
workflowType: 'ux-design'
lastStep: 4
project_name: 'UrbaGIStory'
user_name: 'AllTech'
date: '2025-12-25'
---

# UX Design Specification UrbaGIStory

**Author:** AllTech
**Date:** 2025-12-25

---

<!-- UX design content will be appended sequentially through collaborative workflow steps -->

## Executive Summary

### Project Vision

UrbaGIStory es un Sistema de Información Geográfica (GIS) especializado diseñado para la gestión del patrimonio histórico y el planeamiento urbano en la Red de Oficinas del Conservador de Cuba. El sistema resuelve un problema crítico: actualmente no existe ninguna herramienta que permita a los especialistas agrupar información de forma homogénea, analizar datos y tener una visión geográfica unificada para la toma de decisiones.

**Core Value Proposition:**
- Plataforma centralizada donde especialistas acceden a través de un mapa interactivo a toda su zona de trabajo
- Consulta de información de cualquier entidad urbana (inmuebles, plazas, calles, boulevards, líneas de fachada, manzanas, etc.)
- Gestión de documentos históricos con metadata temporal
- Filtrado por múltiples variables dinámicas para análisis geográficos y toma de decisiones
- Información de múltiples especialistas/departamentos enlazada en un mismo lugar

**What Makes This Special:**
- Facilidad de uso: Diseñado para ser intuitivo y fácil de entender
- Personalización total: Sistema de categorías dinámicas que funcionan como plantillas de trabajo
- Dominio específico: Construido por y para especialistas de patrimonio y urbanismo
- Producto completo: Solución sólida y de bajo mantenimiento desde el inicio

### Target Users

**Primary Users:**

1. **Ernesto - Especialista en Planeamiento Urbano**
   - Enfoque: Análisis arquitectónico y urbanístico
   - Necesidad: Estructurar trabajo, ver información de otros departamentos, correlacionar datos
   - Nivel técnico: Experiencia previa con computadoras, necesita capacitación específica

2. **Amalia - Especialista en Patrimonio Histórico**
   - Enfoque: Aspecto cultural y patrimonial
   - Necesidad: Preservar memoria histórica, trabajar colaborativamente, ver evolución temporal
   - Nivel técnico: Experiencia previa con computadoras, necesita capacitación específica

3. **Carlos - Jefe de Oficina**
   - Enfoque: Moderación, toma de decisiones, visión global
   - Necesidad: Ver información consolidada, moderar categorías y permisos, generar reportes
   - Nivel técnico: Experiencia previa con computadoras, necesita capacitación específica

4. **Roberto - Administrador Técnico**
   - Enfoque: Setup, configuración, soporte
   - Necesidad: Configurar sistema, gestionar usuarios, resolver problemas técnicos
   - Nivel técnico: Alto (administrador técnico)

**User Context:**
- **Frustraciones actuales:** No tienen ninguna solución de este tipo. Información dispersa entre departamentos, falta de herramientas unificadas, dificultad para correlacionar datos
- **Momento de éxito:** Cuando puedan ver información múltiple enlazada, cuando sientan indicaciones claras sobre qué recopilar, cuando puedan filtrar y analizar datos geográficamente
- **Nivel técnico:** Tienen experiencia previa, saben trabajar con computadora, necesitan capacitación para cosas bien específicas
- **Dispositivos:** Desktop (desktop-first), no tablets inicialmente
- **Contexto de uso:** Fundamentalmente en oficina

**Key Insight:** Aunque hay diferentes tipos de usuarios, el flujo base es consistente para todos: **acceder al archivo de la entidad → consultar lo que hay → añadir datos**. El sistema de categorías y permisos dinámicos permite variaciones sin necesidad de journeys separados.

### Key Design Challenges

**1. Mapa Interactivo Complejo**
- Integración de OpenLayers con Blazor WebAssembly requiere JS Interop
- Mapa debe ser intuitivo y fácil de usar para usuarios sin experiencia técnica previa
- Visualización de geometrías QGIS debe ser clara y seleccionable
- Navegación geográfica debe ser natural (zoom, pan, etc.)
- **Desafío:** Hacer que un mapa GIS complejo sea accesible para usuarios no técnicos

**2. Sistema de Categorías Dinámicas**
- Categorías funcionan como plantillas de trabajo que dictan qué información recopilar
- **Complejidad principal:** Vista del jefe de oficina que debe asignar categorías a tipos de entidad
- **Comportamiento dinámico:** Cuando un usuario selecciona una entidad, el sistema busca automáticamente qué categorías le corresponden y muestra las propiedades relevantes
- **Ejemplo:** Si seleccionas una "unidad habitacional" (categoría: vivienda)", aparece la propiedad "cantidad de personas que viven". Pero si seleccionas una "biblioteca", esa propiedad no aparece porque no tiene sentido para ese tipo de entidad
- Las propiedades cambian dinámicamente en función de lo que el usuario selecciona/cliquea
- **Desafío:** 
  - Diseñar una vista para el jefe que permita asignar categorías de forma clara
  - Diseñar cómo se muestran las propiedades dinámicamente cuando seleccionas una entidad
  - Mostrar claramente qué categorías están activas y por qué ciertas propiedades aparecen
  - Hacer que el cambio dinámico de propiedades se sienta natural, no confuso

**3. Flujo Consistente con Variaciones**
- Flujo base es el mismo para todos: acceder → consultar → añadir
- Pero el contenido y propiedades disponibles varían según categorías y permisos
- **Desafío:** Mantener consistencia visual mientras se adapta a variaciones dinámicas

**4. Desktop-First con Claridad**
- Aplicación desktop-first, no responsive inicialmente
- Usuarios necesitan capacitación específica pero deben poder usar el sistema intuitivamente
- **Desafío:** Diseñar para desktop de forma que sea autoexplicativa donde sea posible, minimizando necesidad de capacitación

**5. Información Enlazada y Colaborativa**
- Múltiples especialistas trabajan en la misma entidad
- Información de diferentes departamentos aparece enlazada
- **Desafío:** Mostrar claramente la colaboración y autoría sin abrumar al usuario

**6. Filtrado y Análisis Geográfico**
- Filtrado dinámico robusto basado en categorías y propiedades
- Mapa debe actualizarse para mostrar resultados filtrados
- **Desafío:** Hacer que el filtrado sea poderoso pero fácil de usar, con feedback visual claro

### Design Opportunities

**1. Mapa como Punto Central de Entrada**
- El mapa interactivo puede ser el punto focal de la aplicación
- Visualización geográfica es natural e intuitiva para usuarios de patrimonio/urbanismo
- **Oportunidad:** Crear una experiencia centrada en el mapa que guíe naturalmente al usuario

**2. Categorías como Guía Visual**
- Sistema de categorías puede funcionar como guía paso a paso
- Propiedades automáticas pueden hacer el trabajo más estructurado
- **Oportunidad:** 
  - Diseñar categorías como "asistentes visuales" que guíen al usuario en qué información recopilar
  - Cuando seleccionas una entidad, las propiedades relevantes aparecen automáticamente - esto puede sentirse como magia positiva
  - Vista del jefe para asignar categorías puede ser poderosa pero clara, mostrando la relación entre tipos de entidad, categorías, y propiedades
  - El cambio dinámico de propiedades según selección puede hacer que el sistema se sienta inteligente y contextual

**3. Colaboración Visible**
- Información enlazada de múltiples especialistas puede mostrarse visualmente
- Autoría y trazabilidad pueden hacer la colaboración transparente
- **Oportunidad:** Mostrar la colaboración como una característica, no como complejidad

**4. Filtrado Geográfico Poderoso**
- Filtrado con actualización del mapa puede ser muy visual e impactante
- Correlación de variables puede mostrarse geográficamente
- **Oportunidad:** Hacer que el análisis geográfico sea accesible y visualmente poderoso

**5. Flujo Simple y Consistente**
- Flujo base consistente (acceder → consultar → añadir) simplifica el diseño
- Misma experiencia para todos reduce complejidad
- **Oportunidad:** Crear una experiencia unificada que se sienta familiar independientemente del tipo de usuario

**6. Desktop-First Optimizado**
- Sin restricciones de responsive permite diseño optimizado para desktop
- Más espacio para mostrar información compleja
- **Oportunidad:** Aprovechar el espacio de desktop para crear una interfaz rica pero organizada

### Additional UX Insights

**Categorías Dinámicas - Comportamiento Detallado:**

- **Vista del Jefe (Asignación de Categorías):**
  - El jefe de oficina necesita una vista clara para asignar categorías a tipos de entidad
  - Debe poder ver la relación entre: Tipo de Entidad → Categorías Asignadas → Propiedades Disponibles
  - Esta es la vista más compleja del sistema desde perspectiva de UX

- **Vista del Usuario (Uso de Categorías):**
  - Cuando un usuario selecciona una entidad en el mapa, el sistema busca automáticamente qué categorías le corresponden
  - El sistema muestra solo las propiedades relevantes según las categorías asignadas
  - **Ejemplo concreto:** 
    - Seleccionar "unidad habitacional" (categoría: vivienda) → muestra propiedad "cantidad de personas que viven"
    - Seleccionar "biblioteca" → NO muestra "cantidad de personas que viven" (no tiene sentido)
  - Las propiedades cambian dinámicamente según lo que el usuario selecciona/cliquea
  - Esto debe sentirse natural y contextual, no confuso

- **Diseño UX Implicaciones:**
  - Necesitamos indicadores visuales claros de qué categorías están activas para la entidad seleccionada
  - Necesitamos mostrar por qué ciertas propiedades aparecen (conexión visual entre categoría y propiedad)
  - El cambio dinámico debe tener transiciones suaves para no confundir
  - La vista del jefe debe ser poderosa pero organizada, permitiendo ver y gestionar la complejidad de asignaciones

---

## Core User Experience

### Defining Experience

**Core User Action (Most Critical and Frequent):**

La acción más crítica y frecuente que los usuarios realizarán es:

1. **Asignar variables a las categorías y a los indicadores determinados**
   - Los usuarios asignan valores a las propiedades definidas en las categorías
   - Esta es la acción principal que realizan los especialistas al trabajar con entidades

2. **Filtrado de datos de las categorías y visualización en el mapa**
   - Los usuarios filtran entidades basándose en categorías, propiedades, y otros criterios
   - El mapa se actualiza para mostrar las entidades que cumplen los criterios de filtrado
   - Los filtros pueden ser específicos o genéricos, permitiendo diferentes vistas

3. **Generación de diferentes visualizaciones**
   - Los filtros permiten generar diferentes vistas en los mapas
   - También permiten generar tablas, gráficos, agrupaciones basadas en los datos filtrados
   - Las entidades tienen su JSON de categorías que permite este filtrado dinámico

**Core Interaction Loop:**
1. Usuario selecciona entidad en el mapa (o navega a ella)
2. Sistema muestra propiedades disponibles según categorías asignadas
3. Usuario asigna valores a las variables/propiedades
4. Usuario aplica filtros para analizar datos
5. Sistema visualiza resultados en mapa, tablas, gráficos

**Critical Insight:**
Los mapas están enlazados con las entidades, y las entidades tienen su JSON de categorías. El sistema debe permitir definir filtros específicos o genéricos que permitan generar diferentes vistas y análisis.

### Platform Strategy

**Platform Requirements:**

- **Platform Type:** Web Application (Blazor WebAssembly)
- **Primary Device:** Desktop (desktop-first design)
- **Input Method:** Mouse and keyboard (no touch initially)
- **Usage Context:** Primarily in office environment
- **Network Requirements:** 
  - Application runs locally on internal network (localhost)
  - Users do not need external internet connectivity
  - Users always need access to the server (local network connection)
  - No offline functionality required (not traditional offline mode - always connected to local server)

**Platform Constraints:**
- Desktop-first means we can optimize for larger screens
- Mouse/keyboard interaction allows for precise interactions
- No need to consider touch gestures or mobile constraints
- Local network means fast response times (NFR1: entity selection < 1 second)

**Device-Specific Capabilities:**
- Can leverage full desktop screen real estate
- Can use hover states and right-click context menus
- Can support keyboard shortcuts for power users
- Can display complex information layouts without mobile constraints

### Effortless Interactions

**What Should Feel Completely Natural:**

1. **General Application Management**
   - Todo el manejo general de la aplicación debe ser sin esfuerzo
   - Navigation between different sections should be intuitive
   - Common actions should be easily accessible

2. **Map Interaction (Highest Priority)**
   - La parte del mapa tiene que ser lo más fácil posible
   - Users must be able to linkear fácilmente la geometría con lo que están trabajando
   - Selecting entities on the map should be immediate and clear
   - Map navigation (zoom, pan) should feel natural
   - Visual feedback when selecting entities should be instant

3. **Dynamic Property Display**
   - Properties appearing automatically based on categories should feel natural
   - No confusion about why certain properties appear
   - Smooth transitions when properties change based on selection

4. **Filtering and Visualization**
   - Applying filters should be straightforward
   - Seeing results update on the map should be immediate
   - Switching between map, table, and graph views should be seamless

5. **Information Linking**
   - Seeing information from multiple specialists should be obvious
   - Understanding what information belongs to which specialist should be clear
   - No cognitive load to understand the relationships

**Automatic Behaviors:**
- System automatically displays available properties based on categories assigned to entity type
- System automatically searches for corresponding categories when entity is selected
- System automatically links information from multiple specialists to the same entity
- Map automatically updates to show filtered results

### Critical Success Moments

**Make-or-Break User Flows:**

1. **First-Time Entity Selection**
   - When user selects an entity for the first time and sees properties appear automatically
   - This is the moment they understand "this system knows what I need"
   - **Critical:** If properties don't appear correctly, user loses trust

2. **Seeing Linked Information**
   - When Ernesto sees Amalia's historical photo linked to the same entity
   - When Carlos sees information from multiple specialists in one place
   - **Critical:** This is the core value proposition - if this doesn't work, the product fails

3. **Filtering and Visualization**
   - When user applies filters and sees results update on the map
   - When user generates different views (map, table, graph) from the same filtered data
   - **Critical:** This is the most frequent action - if filtering is confusing, users won't use the system

4. **Assigning Variables to Categories**
   - When user successfully assigns values to properties
   - When the system saves and displays the information correctly
   - **Critical:** This is the core data entry action - must be effortless

5. **Category Assignment (Jefe)**
   - When jefe successfully assigns categories to entity types
   - When the system correctly shows properties based on those assignments
   - **Critical:** If category assignment is confusing, the whole system breaks down

**First-Time User Success:**
- User opens the application
- Sees the interactive map of their work zone
- Selects an entity
- Sees properties appear automatically
- Understands what to do next without extensive training

### Experience Principles

**Guiding Principles for UX Decisions:**

1. **Map-Centric Experience**
   - The map is the primary entry point and focal point
   - All interactions should feel connected to the geographic context
   - Linking geometry to entities should be effortless and visual

2. **Dynamic Adaptation**
   - The interface adapts automatically to show relevant information
   - Properties appear based on context (entity type, categories)
   - Changes feel natural, not jarring

3. **Separation of Concerns (FUNDAMENTAL)**
   - **CRITICAL:** Two distinct flows must be clearly separated:
     - **Flow 1: Edit/Add Information** - For adding and editing data
     - **Flow 2: Consult Entity File** - For viewing and consulting entity information
   - **IMPORTANT:** Both flows require the map view - the map is necessary in both modes
   - These flows serve different purposes and must not be confused
   - This separation is FUNDAMENTAL to the user experience
   - The map provides geographic context in both consultation and editing modes

4. **Effortless Filtering and Visualization**
   - Filtering should be powerful but simple
   - Results should update immediately and visually
   - Multiple visualization options (map, table, graph) should be easily accessible

5. **Clear Information Linking**
   - Collaboration should be visible, not hidden
   - It should be obvious when information comes from different specialists
   - The system should make relationships clear

6. **Desktop-Optimized Clarity**
   - Take advantage of desktop screen space
   - Organize information clearly without mobile constraints
   - Make the interface rich but not overwhelming

**Core Principle:**
The system should feel intelligent and contextual - showing users exactly what they need when they need it, without requiring them to figure out complex navigation or remember where things are.

### Success Metrics for Core Experience

**Primary Success Metric:**
- **Data Scalability:** How well the application scales in terms of data that users add to it
- This metric measures the system's ability to handle increasing amounts of data as users populate the system
- Success is demonstrated when the application performs well and remains usable as data volume grows
- This includes: number of entities, documents attached, properties filled, categories created, etc.

**Experience Quality Indicators:**
- Users can complete core actions (assign variables, filter, visualize) without performance degradation
- Map remains responsive as more entities are added
- Filtering and visualization work efficiently with large datasets
- System maintains good performance as data scales up

### User Mental Model

**How Users Currently Solve This Problem:**

Users currently work with scattered information across different departments:
- **Physical Files:** Information stored in physical archives, different locations
- **Separate Systems:** Each department (Patrimonio, Planeamiento) manages their own information
- **Manual Correlation:** When they need to correlate data, they do it manually, if at all
- **No Geographic Context:** Information exists without easy geographic visualization
- **Inconsistent Methodology:** Each department has different ways of collecting and organizing data

**User Expectations and Mental Model:**

**1. Geographic-First Thinking:**
- Users think in terms of geographic locations first
- They expect to see a map and navigate geographically
- They expect to select entities on the map, not from a list
- **Mental Model:** "I see the area, I click on what I'm interested in"

**2. Contextual Information Display:**
- Users expect to see relevant information when they select something
- They don't expect to see ALL information, just what's relevant
- They expect the system to "know" what information is relevant
- **Mental Model:** "When I select this building, show me what I need to know about it"

**3. Dynamic Property Understanding:**
- Users understand that different types of entities have different properties
- They expect properties to appear/disappear based on what they're working with
- They may be confused if properties appear that don't make sense
- **Mental Model:** "A house has different information than a plaza, the system should know this"

**4. Collaborative Information:**
- Users understand that multiple people work on the same entity
- They expect to see information from different specialists
- They may be surprised if information is missing or hidden
- **Mental Model:** "I should see what others have added to this entity"

**5. Filtering and Analysis:**
- Users expect to filter data to find patterns
- They expect filters to be visual and understandable, not technical
- They expect to see results immediately on the map
- **Mental Model:** "I should be able to filter and see results on the map instantly"

**Potential Confusion Points:**

1. **Why Properties Appear/Disappear:**
   - Users may not understand why certain properties appear for some entities but not others
   - **Mitigation:** Clear visual indicators showing which categories are active, tooltips explaining why properties appear

2. **Category System Complexity:**
   - Users may find the category system confusing, especially the relationship between categories and properties
   - **Mitigation:** Clear visual hierarchy, progressive disclosure, help text

3. **Two Distinct Flows:**
   - Users may confuse "Edit/Add Information" mode with "Consult Entity File" mode
   - **Mitigation:** Clear visual separation, different UI states, clear mode indicators

4. **Filtering Complexity:**
   - Users may not understand how to create complex filters
   - **Mitigation:** Start with simple filters, progressive disclosure for advanced options, examples

5. **Map Interaction:**
   - Users familiar with QGIS may expect more technical controls
   - Users new to GIS may find map interaction intimidating
   - **Mitigation:** Simplify map controls, provide clear visual feedback, tooltips for map actions

### Success Criteria for Core Experience

**What Makes Users Say "This Just Works":**

1. **Instant Property Display:**
   - When user selects an entity, properties appear immediately (< 1 second)
   - Properties are relevant and make sense for the entity type
   - No confusion about why certain properties appear
   - **Success Indicator:** User doesn't need to think about why properties are shown

2. **Effortless Data Entry:**
   - User can assign values to properties without friction
   - System saves automatically or with clear confirmation
   - User sees their changes reflected immediately
   - **Success Indicator:** User completes data entry without frustration

3. **Intuitive Filtering:**
   - User can create filters without technical knowledge
   - Results update on map immediately
   - User understands what filters are applied
   - **Success Indicator:** User successfully filters data and finds what they're looking for

4. **Clear Information Linking:**
   - User sees information from multiple specialists clearly
   - User understands what information belongs to whom
   - User can see the full picture of an entity
   - **Success Indicator:** User feels they have complete information about an entity

5. **Smooth Map Interaction:**
   - User can navigate map naturally (zoom, pan)
   - User can select entities easily
   - Visual feedback is immediate and clear
   - **Success Indicator:** User feels comfortable using the map

**When Users Feel Smart or Accomplished:**
- When they successfully filter data and discover patterns
- When they see information from multiple sources linked together
- When they complete data entry quickly and efficiently
- When they generate visualizations that help them make decisions

**Feedback That Tells Users They're Doing It Right:**
- Properties appear automatically when entity is selected
- Map updates immediately when filters are applied
- Save confirmations are clear and reassuring
- Visual indicators show what's active, what's selected, what's filtered

**Speed Expectations:**
- Entity selection: < 1 second (NFR1)
- Property display: Immediate (< 500ms)
- Filter application: < 1 second
- Map updates: < 1 second
- Data save: < 2 seconds with clear confirmation

**What Should Happen Automatically:**
- Properties appear based on categories (no manual selection needed)
- Map updates when filters are applied (no manual refresh)
- Information from multiple specialists is linked automatically
- System remembers user's last selection/view state

### Novel UX Patterns

**Established Patterns We're Using:**
- **Map Selection:** Standard GIS pattern (QGIS, Google Maps)
- **Panel Lateral:** Common pattern (QGIS, Notion)
- **Filtering:** Standard data filtering pattern
- **Tab Navigation:** Standard pattern for switching views

**Novel Patterns We're Introducing:**

**1. Dynamic Property Display Based on Categories:**
- **What's Novel:** Properties appear/disappear automatically based on categories assigned to entity type
- **Why It's Different:** Most systems show all properties or require manual filtering. We show only relevant properties automatically.
- **User Education Needed:** 
  - Visual indicators showing which categories are active
  - Tooltips explaining why properties appear
  - Smooth transitions when properties change
- **Familiar Metaphor:** Similar to Notion's dynamic properties, but applied to GIS entities

**2. Category System as Work Templates:**
- **What's Novel:** Categories function as work templates that dictate what information specialists collect
- **Why It's Different:** Most systems have fixed schemas. Our system allows dynamic category assignment that changes what information is collected.
- **User Education Needed:**
  - Clear explanation of how categories work as templates
  - Visual representation of category → property relationships
  - Help text for jefe when assigning categories
- **Familiar Metaphor:** Similar to form builders or template systems, but applied to urban entities

**3. Two Distinct Flows with Same Map Context:**
- **What's Novel:** Two fundamentally different flows (Edit/Add vs. Consult) but both require map view
- **Why It's Different:** Most systems separate editing and viewing into different screens. We keep map visible in both modes.
- **User Education Needed:**
  - Clear visual separation between modes
  - Different UI states for each mode
  - Clear mode indicators
- **Familiar Metaphor:** Similar to "edit mode" vs "view mode" but with geographic context always visible

**4. Geographic Filtering with Visual Feedback:**
- **What's Novel:** Filters update map immediately, showing geographic patterns
- **Why It's Different:** Most GIS systems require technical knowledge to filter. Our system makes filtering visual and immediate.
- **User Education Needed:**
  - Examples of how to use filters
  - Visual feedback showing what's filtered
  - Progressive disclosure for advanced filtering
- **Familiar Metaphor:** Similar to e-commerce filtering, but applied to geographic data

**5. Collaborative Information Linking:**
- **What's Novel:** Information from multiple specialists automatically linked to same entity, visible in one place
- **Why It's Different:** Most systems keep information separate. Our system shows collaborative information together.
- **User Education Needed:**
  - Clear visual indicators of information source (specialist, department)
  - Understanding of how information is linked
  - Trust that information is correctly associated
- **Familiar Metaphor:** Similar to collaborative documents (Google Docs), but applied to geographic entities

**Innovation Within Familiar Patterns:**
- **Map-Centric:** We use familiar map interaction, but make it more accessible
- **Panel Lateral:** We use familiar panel pattern, but make it dynamic and contextual
- **Filtering:** We use familiar filtering, but make it visual and geographic

### Experience Mechanics

**Detailed Step-by-Step Flow for Core Experience:**

**1. Initiation - How User Starts:**

**Scenario A: First-Time User Opening Application**
- User opens application
- Sees interactive map of their work zone
- Map shows all entities (or filtered view if saved preference)
- User is invited to explore by seeing entities on map
- **Trigger:** Map is visible and interactive immediately

**Scenario B: Returning User**
- User opens application
- System remembers last view (zoom level, selected entity, active filters)
- User continues from where they left off
- **Trigger:** Familiar interface, last state restored

**Scenario C: User Searching for Specific Entity**
- User can search by name, address, or other identifier
- System highlights entity on map
- User can click to select
- **Trigger:** Search functionality or direct map interaction

**2. Interaction - What User Actually Does:**

**Step 1: Entity Selection**
- User clicks on entity in map (or selects from list/table)
- System immediately highlights entity visually (color change, border, etc.)
- System queries database for entity information
- System identifies which categories are assigned to this entity type
- **User Action:** Single click on map or list item
- **System Response:** Visual highlight + query execution

**Step 2: Property Display**
- System automatically displays properties based on categories assigned
- Properties appear in panel lateral with smooth transition
- Properties are organized logically (grouped by category if multiple)
- Empty properties show placeholders indicating they can be filled
- **User Action:** None required - automatic
- **System Response:** Properties appear in panel, organized and clear

**Step 3: Value Assignment (Edit/Add Mode)**
- User sees properties that need values
- User clicks on property field
- User enters value (text, number, date, selection from dropdown, etc.)
- System validates input in real-time
- User can save individual property or all at once
- **User Action:** Click field, enter value, save
- **System Response:** Validation, save confirmation, visual update

**Step 4: Filtering (Consult Mode)**
- User opens filter panel
- User selects filter criteria (category, property value, date range, etc.)
- User applies filter
- System queries database
- Map updates to show only matching entities
- Table/graph views also update
- **User Action:** Select filters, apply
- **System Response:** Query execution, map update, view synchronization

**3. Feedback - How User Knows It's Working:**

**Visual Feedback:**
- **Entity Selection:** Immediate highlight (color change, border, animation)
- **Property Display:** Smooth fade-in animation, organized layout
- **Value Entry:** Real-time validation (green checkmark, red error)
- **Filter Application:** Loading indicator, then map update
- **Save Confirmation:** Success message, visual confirmation

**Haptic/Audio Feedback:**
- Optional: Subtle sound on entity selection (can be disabled)
- Keyboard feedback for data entry

**Error Feedback:**
- Clear error messages in user's language
- Visual indicators (red borders, error icons)
- Helpful suggestions for fixing errors
- No blame language, supportive tone

**4. Completion - How User Knows They're Done:**

**Data Entry Completion:**
- Clear "Save" button or auto-save indicator
- Success message: "Information saved successfully"
- Visual confirmation that data is persisted
- User can see saved information immediately

**Filter Application Completion:**
- Map updates showing filtered results
- Count indicator: "Showing X of Y entities"
- Clear visual that filters are active
- User can see results in map, table, or graph

**Task Completion:**
- User feels accomplished when they've completed their task
- System shows what they've accomplished (properties filled, filters applied)
- User can move to next task or entity
- System maintains context for next action

**What's Next:**
- After data entry: User can continue with same entity or select another
- After filtering: User can refine filters, export results, or analyze data
- After consultation: User can switch to edit mode or navigate to different entity
- System maintains state, allowing seamless continuation

**Error Recovery:**
- If entity selection fails: Clear error message, suggestion to try again
- If property display fails: Fallback to showing all properties, error logged
- If save fails: Retry option, data preserved in form
- If filter fails: Clear error, suggestion to simplify filter criteria

---

## Desired Emotional Response

### Primary Emotional Goals

**Core Emotional Objectives:**

1. **Confianza y Claridad (Primary - Most Critical)**
   - Users must feel **confianza (trust)** and **claridad (clarity)** when using UrbaGIStory
   - This is particularly important because the application is challenging for people in this domain who don't necessarily have great technical knowledge
   - Users must feel confident in the application despite not being technical experts
   - The system should make complex GIS operations feel accessible and clear

2. **Logro y Satisfacción**
   - Users should feel **logro (accomplishment)** and **satisfacción (satisfaction)** after completing their tasks
   - Successfully assigning variables, filtering data, and generating visualizations should feel rewarding
   - Completing work that was previously impossible should create a sense of achievement

3. **Organización vs. Caos**
   - Users should feel **organización (organization)** instead of the current **caos (chaos)**
   - The system transforms their work from scattered, inconsistent information to organized, structured data
   - This emotional shift from chaos to organization is a key differentiator

**Why Users Would Recommend:**
- **Nuevas capacidades (New Capabilities):** Users are excited about capabilities they never had before
- **Confianza en la herramienta (Trust in the Tool):** Users trust that the system works correctly and reliably

**Emotional Context:**
- Users are domain experts (heritage, urban planning) but not necessarily technical experts
- The application must bridge this gap - making technical GIS capabilities accessible to non-technical users
- Confidence and clarity are essential because users need to trust a system they don't fully understand technically

### Emotional Journey Mapping

**Emotional States Across User Experience:**

1. **First Discovery (Primera Vez): Curiosos (Curious)**
   - When users first discover UrbaGIStory, they should feel **curiosos (curious)**
   - The map and interface should spark curiosity about what's possible
   - First-time users should want to explore and understand the system
   - **Design Implication:** Make the first view inviting and intriguing, not overwhelming

2. **During Core Experience (Experiencia Central): Confianza (Confidence)**
   - During the core experience (assigning variables, filtering, visualizing), users should feel **confianza (confidence)**
   - Users should trust that the system is showing them the right information
   - Properties appearing automatically should feel reliable, not confusing
   - **Design Implication:** Clear feedback, consistent behavior, visual indicators that build trust

3. **After Completing Task (Después de Completar): Seguros (Secure/Safe)**
   - After completing their task, users should feel **seguros (secure/safe)**
   - Users should feel confident that their information is saved correctly
   - They should feel secure that their work is properly stored and accessible
   - **Design Implication:** Clear confirmation of saves, visible data persistence, reassurance that work is safe

4. **When Something Goes Wrong (Si Algo Sale Mal): Tranquilidad (Calm/Tranquility)**
   - When something goes wrong, users should feel **tranquilidad (calm/tranquility)**
   - Error messages should be reassuring, not alarming
   - Users should feel that problems can be resolved without panic
   - **Design Implication:** Clear, helpful error messages, recovery paths, no blame language

5. **When Returning to Use (Al Volver a Usar): Alegres (Happy/Joyful)**
   - When users return to use the system, they should feel **alegres (happy/joyful)**
   - Returning users should feel positive about using the system
   - The system should feel like a helpful tool, not a burden
   - **Design Implication:** Pleasant interface, smooth interactions, positive reinforcement

### Micro-Emotions

**Critical Emotional States:**

1. **Confianza vs. Confusión (Trust vs. Confusion)**
   - **Target:** Confianza
   - Users must trust the system even though they may not understand all technical aspects
   - The system should feel predictable and reliable
   - **Avoid:** Confusion about why properties appear, what categories mean, how filters work

2. **Claridad vs. Desorden (Clarity vs. Disorder)**
   - **Target:** Claridad
   - Information should be clearly organized and easy to understand
   - The interface should make sense without extensive explanation
   - **Avoid:** Visual clutter, unclear relationships, disorganized information

3. **Logro vs. Frustración (Accomplishment vs. Frustration)**
   - **Target:** Logro
   - Users should feel accomplished when they complete tasks
   - The system should enable work that was previously impossible
   - **Avoid:** Frustration from unclear workflows, confusing interfaces, or technical barriers

4. **Organización vs. Caos (Organization vs. Chaos)**
   - **Target:** Organización
   - The system transforms chaotic, scattered information into organized, structured data
   - Users should feel that their work is now organized and accessible
   - **Avoid:** Feeling that the system adds to the chaos instead of reducing it

### Design Implications

**UX Design Choices to Support Emotional Goals:**

1. **Confianza y Claridad:**
   - **Clear Visual Hierarchy:** Information organized in a way that's immediately understandable
   - **Consistent Patterns:** Same actions work the same way throughout the system
   - **Visual Feedback:** Clear indicators of what's happening and why
   - **Progressive Disclosure:** Show complexity gradually, not all at once
   - **Helpful Labels and Tooltips:** Explain technical concepts in domain language (not technical jargon)
   - **Error Prevention:** Design to prevent mistakes rather than just catch them
   - **Domain Language:** Use heritage and urban planning terminology, not technical GIS jargon
   - **Familiar Iconography:** Use icons and visual elements familiar to domain experts
   - **White Space:** Use white space effectively to create sense of organization
   - **Visual Grouping:** Group related information visually to reduce cognitive load
   - **Clear Hierarchy:** Visual hierarchy that makes structure immediately apparent

2. **Logro y Satisfacción:**
   - **Success Indicators:** Clear confirmation when tasks are completed
   - **Visual Progress:** Show users what they've accomplished
   - **Positive Reinforcement:** Celebrate successful actions
   - **Capability Showcase:** Make it obvious when users achieve something new
   - **Progress Feedback:** Show clear progress indicators during task completion
   - **Save Confirmations:** Clear, reassuring confirmation that work is saved correctly

3. **Organización vs. Caos:**
   - **Structured Layouts:** Information organized in logical, predictable ways
   - **Clear Relationships:** Visual connections between related information
   - **Unified Views:** All information about an entity in one place
   - **Clean Interface:** No visual clutter that adds to confusion
   - **Visual Transformation:** Interface should visually reflect the transformation from chaos to order
   - **White Space and Grouping:** Use white space and visual grouping to create sense of organization
   - **Structured Information Display:** Information displayed in structured, predictable patterns

4. **Curiosos (First Discovery):**
   - **Inviting First View:** Map and interface should spark curiosity
   - **Discoverable Features:** Users can explore and find capabilities
   - **Onboarding:** Guide users without overwhelming them

5. **Confianza (During Core Experience):**
   - **Reliable Behavior:** System behaves consistently and predictably
   - **Clear Feedback:** Users always know what's happening
   - **Trust Indicators:** Visual cues that build confidence (e.g., "Saved successfully")

6. **Seguros (After Completing):**
   - **Save Confirmations:** Clear indication that work is saved
   - **Data Visibility:** Users can see their saved information
   - **Recovery Options:** Users know they can undo or recover if needed

7. **Tranquilidad (When Errors Occur):**
   - **Calm Error Messages:** Errors explained clearly without alarm
   - **Recovery Paths:** Clear steps to resolve issues
   - **Supportive Language:** Helpful, not blaming tone
   - **Non-Alarming States:** Error states should be reassuring, not alarming
   - **Informative Loading:** Loading states should be informative, not anxiety-inducing
   - **Stable Feel:** System should feel stable and reliable even when errors occur

8. **Alegres (When Returning):**
   - **Pleasant Interface:** Visually appealing, not sterile
   - **Smooth Interactions:** No friction in common actions
   - **Positive Associations:** System feels like a helpful tool

### Emotional Design Principles

**Guiding Principles for Emotional Design:**

1. **Build Trust Through Consistency**
   - Consistent behavior builds confidence
   - Predictable patterns reduce anxiety
   - Reliable performance creates trust

2. **Create Clarity Through Organization**
   - Clear visual hierarchy reduces cognitive load
   - Organized information feels manageable
   - Structured layouts create sense of order

3. **Enable Achievement Through Capability**
   - Make impossible tasks possible
   - Show users what they've accomplished
   - Celebrate new capabilities

4. **Transform Chaos into Order**
   - Visual organization reflects data organization
   - Unified views replace scattered information
   - Clear relationships replace confusion

5. **Support Non-Technical Users**
   - Use domain language, not technical jargon
   - Explain technical concepts clearly
   - Make complex operations feel simple

6. **Maintain Calm in All Situations**
   - Error states should be reassuring
   - Loading states should be informative, not alarming
   - System should feel stable and reliable

**Core Emotional Principle:**
The system should make users feel **confident and clear** about what they're doing, even if they don't understand all the technical complexity behind it. Users should feel that the system is **helping them organize their work** and **enabling new capabilities**, not adding complexity or confusion.

### Emotional Success Metrics

**Measuring Emotional Goals:**

To validate that we're achieving the desired emotional responses, consider:

1. **Confianza y Claridad Metrics:**
   - User satisfaction surveys asking about confidence and clarity
   - Observation of users to see if they feel confident using the system
   - Time to complete tasks without confusion
   - Frequency of help requests or confusion indicators

2. **Logro y Satisfacción Metrics:**
   - User reports of feeling accomplished after completing tasks
   - Frequency of successful task completions
   - User testimonials about new capabilities enabled

3. **Organización vs. Caos Metrics:**
   - User perception of organization vs. previous chaos
   - Time saved compared to previous methods
   - User reports of feeling organized and structured

4. **Emotional Journey Metrics:**
   - **Curiosos:** User engagement on first use
   - **Confianza:** User confidence during core experience (surveys, observations)
   - **Seguros:** User confidence that work is saved (post-task surveys)
   - **Tranquilidad:** User calmness when errors occur (error recovery success rate)
   - **Alegres:** User satisfaction when returning (return usage frequency, positive feedback)

---

## UX Pattern Analysis & Inspiration

### Inspiring Products Analysis

**Primary Inspiration: Notion + QGIS Hybrid Approach**

UrbaGIStory se inspira en una combinación estratégica de dos productos complementarios que resuelven diferentes aspectos del problema:

#### 1. Notion - Gestión de Información Estructurada

**What Notion Does Well:**
- **Jerarquía de Categorías Anidadas:** Sistema visual de anidamiento que permite categorías padre con subcategorías, creando una estructura organizada y navegable
- **Propiedades Dinámicas Contextuales:** Las propiedades aparecen automáticamente según el tipo de página/plantilla seleccionada, adaptándose al contexto sin confusión
- **Filtrado Visual e Intuitivo:** Sistema de filtros accesible con controles visuales (checkboxes, dropdowns) que no requieren conocimiento técnico
- **Panel Lateral Contextual:** Panel que muestra información relevante según la selección, con transiciones suaves
- **Vista de Base de Datos Flexible:** Múltiples vistas (tabla, kanban) de los mismos datos, permitiendo diferentes perspectivas
- **Enlaces entre Entidades:** Relaciones visibles entre diferentes elementos, mostrando colaboración y conexiones

**Why It's Relevant:**
- El sistema de categorías dinámicas de UrbaGIStory funciona de manera similar a las plantillas de Notion
- Los usuarios necesitan filtrado entendible, no técnico
- La jerarquía de categorías es esencial para organizar tipos de entidades urbanas

#### 2. QGIS - Visualización Geográfica Profesional

**What QGIS Does Well:**
- **Mapa como Centro Visual:** El mapa ocupa el espacio principal, siendo el punto focal de la aplicación
- **Visualización de Capas Superpuestas:** Múltiples capas se superponen visualmente, permitiendo ver diferentes tipos de información geográfica simultáneamente
- **Selección Clara de Geometrías:** Feedback visual inmediato al seleccionar elementos en el mapa
- **Navegación Geográfica Natural:** Zoom, pan, y otras interacciones de mapa se sienten naturales e intuitivas
- **Panel de Propiedades Contextual:** Al seleccionar una geometría, aparece información relevante en un panel lateral
- **Gestión de Capas Organizada:** Sistema claro para activar/desactivar y organizar diferentes capas de información

**Why It's Relevant:**
- Los usuarios objetivo ya conocen QGIS, reduciendo la curva de aprendizaje
- UrbaGIStory necesita un mapa interactivo como punto central
- La integración con QGIS requiere patrones familiares para los usuarios

### Transferable UX Patterns

**Patterns to Adopt from Notion:**

1. **Jerarquía Visual de Categorías**
   - **Aplicación:** Sistema de categorías anidadas para tipos de entidades urbanas
   - **Ejemplo:** Categoría "Vivienda" → Subcategorías "Unidad Habitacional", "Casa Individual"
   - **Visualización:** Árbol colapsable o vista jerárquica con indentación clara
   - **Beneficio:** Organiza la complejidad de tipos de entidades de forma intuitiva

2. **Propiedades Dinámicas Contextuales**
   - **Aplicación:** Propiedades que aparecen automáticamente según categorías asignadas a la entidad seleccionada
   - **Ejemplo:** Seleccionar "Unidad Habitacional" → muestra "Cantidad de personas que viven"
   - **Visualización:** Panel lateral que se actualiza dinámicamente con transiciones suaves
   - **Beneficio:** Reduce confusión, muestra solo información relevante

3. **Filtrado Visual e Intuitivo**
   - **Aplicación:** Sistema de filtros con controles visuales (checkboxes, dropdowns) en lenguaje del dominio
   - **Ejemplo:** Filtrar por "Tipo de Vivienda" con checkboxes, no con queries técnicas
   - **Visualización:** Panel de filtros accesible, resultados actualizados en tiempo real
   - **Beneficio:** Filtrado poderoso pero entendible para usuarios no técnicos

4. **Vista de Base de Datos Flexible**
   - **Aplicación:** Múltiples vistas de las mismas entidades (mapa, tabla, gráfico)
   - **Ejemplo:** Ver entidades filtradas en mapa, tabla, o gráfico según necesidad
   - **Visualización:** Tabs o botones para cambiar entre vistas, sincronización automática
   - **Beneficio:** Diferentes perspectivas de los mismos datos para diferentes análisis

**Patterns to Adopt from QGIS:**

1. **Mapa como Centro Visual**
   - **Aplicación:** Mapa interactivo ocupa 60-70% del ancho de pantalla, siendo el punto focal
   - **Ejemplo:** Al abrir la aplicación, el mapa es lo primero que se ve
   - **Visualización:** Layout con mapa grande a la izquierda, panel lateral a la derecha
   - **Beneficio:** Contexto geográfico siempre visible, navegación natural

2. **Visualización de Capas Superpuestas**
   - **Aplicación:** Diferentes tipos de entidades (inmuebles, plazas, calles) como capas superpuestas
   - **Ejemplo:** Ver inmuebles y plazas simultáneamente en el mismo mapa
   - **Visualización:** Sistema de capas con checkboxes para activar/desactivar
   - **Beneficio:** Análisis geográfico completo, visión integrada

3. **Selección Clara con Feedback Visual**
   - **Aplicación:** Al seleccionar una entidad en el mapa, feedback visual inmediato (highlight, borde)
   - **Ejemplo:** Click en inmueble → se resalta visualmente, panel lateral muestra información
   - **Visualización:** Cambio de color/borde, animación sutil, panel se actualiza
   - **Beneficio:** Conexión clara entre mapa e información, reduce confusión

4. **Panel de Propiedades Contextual**
   - **Aplicación:** Panel lateral que muestra información de la entidad seleccionada
   - **Ejemplo:** Seleccionar entidad → panel muestra propiedades según categorías asignadas
   - **Visualización:** Panel fijo a la derecha, scrollable, se actualiza dinámicamente
   - **Beneficio:** Información contextual siempre visible, sin navegación adicional

**Hybrid Patterns (Combining Both):**

1. **Mapa Central + Panel Lateral Dinámico**
   - **Aplicación:** Mapa grande (QGIS-style) + Panel lateral con propiedades dinámicas (Notion-style)
   - **Layout:** CSS Grid o Flexbox, mapa 60-70%, panel 30-40%
   - **Beneficio:** Combina contexto geográfico con información estructurada

2. **Filtrado Geográfico Visual**
   - **Aplicación:** Filtros tipo Notion (visuales, entendibles) que actualizan el mapa tipo QGIS
   - **Ejemplo:** Checkbox "Solo Viviendas" → mapa se actualiza mostrando solo viviendas
   - **Visualización:** Panel de filtros arriba o lateral, mapa responde inmediatamente
   - **Beneficio:** Filtrado poderoso con feedback geográfico inmediato

### Anti-Patterns to Avoid

**From QGIS (What NOT to Copy):**

1. **Interfaz Técnica y Compleja**
   - **Anti-pattern:** Menús técnicos, jerga GIS, opciones avanzadas visibles
   - **Por qué evitar:** Usuarios no técnicos se confunden, aumenta curva de aprendizaje
   - **Solución:** Simplificar interfaz, usar lenguaje del dominio, ocultar opciones avanzadas

2. **Filtrado con Queries Técnicas**
   - **Anti-pattern:** Requerir conocimiento de SQL o sintaxis técnica para filtrar
   - **Por qué evitar:** Usuarios no pueden usar filtrado sin capacitación técnica
   - **Solución:** Filtros visuales tipo Notion, controles intuitivos

3. **Múltiples Ventanas Flotantes**
   - **Anti-pattern:** Ventanas modales que se superponen, pérdida de contexto
   - **Por qué evitar:** Confusión, pérdida de orientación geográfica
   - **Solución:** Panel lateral fijo, no modales, mantener contexto siempre visible

**From Notion (What NOT to Copy):**

1. **Exceso de Flexibilidad**
   - **Anti-pattern:** Demasiadas opciones de personalización, usuarios se pierden
   - **Por qué evitar:** UrbaGIStory necesita estructura clara, no flexibilidad infinita
   - **Solución:** Estructura guiada, categorías predefinidas, personalización controlada

2. **Navegación por Páginas Separadas**
   - **Anti-pattern:** Cada entidad como página separada, pérdida de contexto geográfico
   - **Por qué evitar:** El mapa es esencial, no puede perderse
   - **Solución:** Panel lateral contextual, mapa siempre visible

**General Anti-Patterns:**

1. **Cambios Bruscos sin Transición**
   - **Anti-pattern:** Propiedades que aparecen/desaparecen sin animación
   - **Por qué evitar:** Confusión, pérdida de contexto visual
   - **Solución:** Transiciones suaves, indicadores visuales de cambio

2. **Información Ocultada en Menús Profundos**
   - **Anti-pattern:** Acciones importantes escondidas en menús de 3+ niveles
   - **Por qué evitar:** Usuarios no encuentran funcionalidades esenciales
   - **Solución:** Acciones principales visibles, menús solo para opciones secundarias

### Design Inspiration Strategy

**What to Adopt:**

1. **Jerarquía Visual de Categorías (Notion)**
   - **Por qué:** Organiza la complejidad de tipos de entidades de forma intuitiva
   - **Cómo:** Árbol colapsable con indentación, vista jerárquica clara
   - **Aplicación:** Sistema de categorías anidadas para tipos de entidades urbanas

2. **Mapa Central con Panel Lateral (QGIS + Notion)**
   - **Por qué:** Combina contexto geográfico (QGIS) con información estructurada (Notion)
   - **Cómo:** Layout 60-70% mapa, 30-40% panel lateral fijo
   - **Aplicación:** Mapa siempre visible, panel con propiedades dinámicas

3. **Filtrado Visual e Intuitivo (Notion)**
   - **Por qué:** Filtrado poderoso pero entendible para usuarios no técnicos
   - **Cómo:** Controles visuales (checkboxes, dropdowns), lenguaje del dominio
   - **Aplicación:** Sistema de filtros que actualiza mapa en tiempo real

4. **Propiedades Dinámicas Contextuales (Notion)**
   - **Por qué:** Reduce confusión, muestra solo información relevante
   - **Cómo:** Panel lateral que se actualiza según categorías asignadas
   - **Aplicación:** Propiedades aparecen automáticamente al seleccionar entidad

5. **Selección Clara con Feedback Visual (QGIS)**
   - **Por qué:** Conexión clara entre mapa e información, reduce confusión
   - **Cómo:** Highlight visual inmediato, panel se actualiza automáticamente
   - **Aplicación:** Click en entidad → resaltado visual + panel muestra información

**What to Adapt:**

1. **Sistema de Capas (QGIS) → Simplificado**
   - **Adaptación:** Capas visibles pero simplificadas, no todas las opciones técnicas de QGIS
   - **Razón:** Usuarios no técnicos, mantener simplicidad
   - **Aplicación:** Checkboxes para activar/desactivar tipos de entidades, sin opciones avanzadas

2. **Vista de Base de Datos (Notion) → Con Contexto Geográfico**
   - **Adaptación:** Tabla sincronizada con mapa, selección en tabla resalta en mapa
   - **Razón:** Mantener contexto geográfico siempre visible
   - **Aplicación:** Vista de tabla que se sincroniza bidireccionalmente con mapa

3. **Filtrado (Notion) → Con Actualización Geográfica**
   - **Adaptación:** Filtros visuales tipo Notion que actualizan mapa tipo QGIS
   - **Razón:** Combinar facilidad de uso con feedback geográfico
   - **Aplicación:** Panel de filtros que actualiza mapa, tabla, y gráficos simultáneamente

**What to Avoid:**

1. **Interfaz Técnica de QGIS**
   - **Razón:** Usuarios no técnicos, necesita ser intuitivo
   - **Solución:** Simplificar, usar lenguaje del dominio, ocultar complejidad técnica

2. **Filtrado con Queries Técnicas**
   - **Razón:** Usuarios no pueden usar sin capacitación técnica
   - **Solución:** Filtros visuales, controles intuitivos, lenguaje del dominio

3. **Exceso de Flexibilidad de Notion**
   - **Razón:** UrbaGIStory necesita estructura guiada, no flexibilidad infinita
   - **Solución:** Categorías predefinidas, personalización controlada por jefe de oficina

4. **Navegación por Páginas Separadas**
   - **Razón:** El mapa es esencial, no puede perderse
   - **Solución:** Panel lateral contextual, mapa siempre visible, no modales

**Core Strategy:**

UrbaGIStory combina lo mejor de ambos mundos:
- **Familiaridad de QGIS:** Mapa central, visualización geográfica, selección clara
- **Intuitividad de Notion:** Propiedades dinámicas, filtrado visual, jerarquía clara
- **Resultado:** Sistema GIS poderoso pero accesible, que se siente familiar pero es más fácil de usar

---

## Design System Foundation

### Design System Choice

**Selected Design System: MudBlazor**

MudBlazor es un framework de componentes UI completo y gratuito diseñado específicamente para Blazor. Proporciona una biblioteca de componentes preconstruidos, estilizados y listos para usar, ideal para desarrollo rápido y consistente en aplicaciones Blazor WebAssembly.

**Key Characteristics:**
- **Open Source & Free:** Completamente gratuito, sin costos de licencia
- **Blazor-Native:** Diseñado específicamente para Blazor, no es un wrapper
- **Component-Rich:** Más de 60 componentes listos para usar
- **Well-Documented:** Documentación extensa con ejemplos de código
- **Active Community:** Comunidad grande y activa para soporte
- **Themeable:** Sistema de temas personalizables

### Rationale for Selection

**1. Requirements Alignment:**
   - ✅ **Free:** Completamente gratuito, sin costos ocultos
   - ✅ **Easy to Learn:** Ideal para desarrolladores nuevos en frontend
   - ✅ **Blazor WebAssembly Compatible:** Diseñado específicamente para Blazor WASM
   - ✅ **Desktop-First:** Componentes optimizados para desktop, no mobile-first

**2. Developer Experience:**
   - **Beginner-Friendly:** Sintaxis simple, no requiere conocimiento avanzado de CSS
   - **Fast Development:** Componentes preestilizados aceleran el desarrollo
   - **Good Documentation:** Ejemplos claros y documentación completa
   - **IntelliSense Support:** Excelente soporte en Visual Studio

**3. Technical Compatibility:**
   - **OpenLayers Integration:** Compatible con OpenLayers vía JS Interop sin conflictos
   - **Performance:** Optimizado para Blazor WebAssembly
   - **Component Library:** Incluye todos los componentes necesarios (tablas, formularios, paneles, navegación)

**4. Project-Specific Benefits:**
   - **Panel Lateral:** `MudDrawer` perfecto para panel lateral contextual
   - **Tablas:** `MudTable` ideal para vista de tabla de entidades
   - **Formularios:** `MudTextField`, `MudSelect` para propiedades dinámicas
   - **Layout:** Sistema de grid para layout mapa + panel
   - **Filtros:** Componentes de formulario para sistema de filtrado visual

**5. Long-Term Considerations:**
   - **Maintainability:** Framework estable y mantenido activamente
   - **Scalability:** Fácil de extender con componentes personalizados
   - **Community Support:** Comunidad grande para resolver problemas
   - **Future-Proof:** Alineado con el futuro de Blazor

### Technical Considerations & Risks

**1. Bundle Size Considerations:**
   - **Impact:** MudBlazor puede aumentar el tamaño del bundle WASM
   - **Mitigation:** Para aplicación local (localhost), el tamaño no es crítico
   - **Optimization:** Usar tree-shaking, cargar componentes solo cuando se necesiten
   - **Monitoring:** Monitorear tamaño del bundle durante desarrollo

**2. JS Interop Overhead:**
   - **Risk:** OpenLayers ya usa JS Interop extensivamente
   - **Mitigation:** MudBlazor y OpenLayers no compiten, se complementan
   - **Best Practice:** Optimizar llamadas JS Interop, agrupar cuando sea posible
   - **Performance:** Monitorear rendimiento de JS Interop en desarrollo

**3. Learning Curve Management:**
   - **Challenge:** Primera experiencia con frontend + Blazor + MudBlazor
   - **Strategy:** Aprendizaje gradual, empezar con componentes estándar
   - **Approach:** 
     - Fase 1: Usar componentes estándar de MudBlazor
     - Fase 2: Personalizar temas y estilos
     - Fase 3: Crear componentes personalizados cuando sea necesario
   - **Resources:** Aprovechar documentación y ejemplos de MudBlazor

**4. Visual Differentiation:**
   - **Risk:** MudBlazor puede verse "genérico" sin personalización
   - **Strategy:** Personalizar tema desde el inicio para crear identidad visual única
   - **Approach:**
     - Definir paleta de colores específica para UrbaGIStory
     - Personalizar tipografía y espaciado
     - Crear componentes personalizados para elementos únicos (categorías anidadas)
   - **Timeline:** Incluir personalización de tema en MVP para diferenciación visual

**5. Long-Term Maintenance:**
   - **Risk:** Dependencia de mantenimiento activo de MudBlazor
   - **Mitigation:** MudBlazor tiene comunidad grande y mantenimiento activo
   - **Strategy:** Monitorear actualizaciones, planificar migraciones si es necesario
   - **Backup Plan:** Si MudBlazor deja de mantenerse, migración a otro framework es posible (componentes Blazor son estándar)

### Implementation Approach

**1. Installation & Setup:**

```bash
# Install MudBlazor package
dotnet add package MudBlazor

# Add MudBlazor services in Program.cs
builder.Services.AddMudServices();
```

**2. Core Components for UrbaGIStory:**

**Layout Components:**
- `MudContainer`: Contenedor principal
- `MudGrid`: Sistema de grid para layout mapa + panel
- `MudDrawer`: Panel lateral para propiedades de entidades
- `MudAppBar`: Barra superior de navegación (opcional)

**Data Display:**
- `MudTable`: Vista de tabla de entidades con filtrado
- `MudCard`: Contenedores para información de entidades
- `MudChip`: Etiquetas para categorías
- `MudTreeView`: Vista jerárquica de categorías anidadas

**Form Components:**
- `MudTextField`: Campos de texto para propiedades
- `MudSelect`: Dropdowns para selección de valores
- `MudCheckbox`: Checkboxes para filtros
- `MudDatePicker`: Selector de fechas para metadata temporal
- `MudNumericField`: Campos numéricos para variables

**Navigation & Interaction:**
- `MudButton`: Botones de acción
- `MudTabs`: Tabs para cambiar entre vistas (mapa, tabla, gráfico)
- `MudMenu`: Menús contextuales
- `MudDialog`: Modales para confirmaciones

**3. OpenLayers Integration Pattern:**

```csharp
<MudContainer MaxWidth="MaxWidth.False" Class="pa-0">
    <MudGrid>
        <!-- Mapa con OpenLayers -->
        <MudItem xs="8">
            <div id="map" style="width: 100%; height: 100vh;"></div>
        </MudItem>
        
        <!-- Panel lateral con MudBlazor -->
        <MudItem xs="4">
            <MudDrawer Open="true" Variant="@DrawerVariant.Persistent">
                <MudNavMenu>
                    <!-- Contenido del panel -->
                </MudNavMenu>
            </MudDrawer>
        </MudItem>
    </MudGrid>
</MudContainer>

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("initMap");
        }
    }
}
```

**4. Component Strategy:**

**Standard MudBlazor Components:**
- Usar componentes estándar cuando sea posible
- Aprovechar estilos y comportamientos predefinidos
- Reducir desarrollo personalizado

**Custom Components (When Needed):**
- Componentes específicos de dominio (ej: selector de categorías anidadas)
- Integración específica con OpenLayers
- Componentes que combinen múltiples componentes MudBlazor

**5. Performance Considerations:**

- **Lazy Loading:** Cargar componentes pesados solo cuando se necesiten
- **Virtualization:** Usar `MudTable` con virtualización para grandes datasets
- **JS Interop Optimization:** Minimizar llamadas JS Interop con OpenLayers, agrupar cuando sea posible
- **Component Lifecycle:** Gestionar correctamente el ciclo de vida de componentes
- **Bundle Optimization:** Usar tree-shaking, monitorear tamaño del bundle

### Customization Strategy

**1. Theme Customization (Critical for Visual Differentiation):**

**Color Palette:**
- Personalizar colores primarios y secundarios según necesidades del dominio
- Mantener contraste adecuado para accesibilidad
- Usar colores que reflejen el contexto de patrimonio histórico y urbanismo
- **Timeline:** Definir paleta en MVP para diferenciación visual desde el inicio

**Typography:**
- Seleccionar fuentes legibles para desktop
- Tamaños de fuente apropiados para lectura prolongada
- Jerarquía tipográfica clara

**Spacing:**
- Espaciado consistente usando sistema de spacing de MudBlazor
- Aprovechar utilidades de padding y margin

**2. Component Customization:**

**Standard Components:**
- Usar variantes y tamaños predefinidos
- Aplicar clases CSS personalizadas cuando sea necesario
- Mantener consistencia con el sistema de diseño

**Custom Components (For Unique Features):**
- **Categorías Anidadas:** Componente personalizado que combine `MudTreeView` con lógica específica
- **Selector de Entidades:** Componente que integre selección en mapa con panel de propiedades
- **Filtros Dinámicos:** Componente que genere filtros basados en categorías asignadas
- Mantener la estética de MudBlazor para consistencia
- Documentar componentes personalizados

**3. Integration with OpenLayers:**

**Visual Consistency:**
- Asegurar que el mapa y los componentes MudBlazor se vean cohesivos
- Usar colores del tema en controles del mapa
- Mantener espaciado consistente

**Interaction Patterns:**
- Sincronizar interacciones entre mapa y componentes MudBlazor
- Feedback visual consistente
- Transiciones suaves

**4. Accessibility:**

**MudBlazor Built-in:**
- Componentes con soporte ARIA
- Navegación por teclado
- Contraste de colores

**Custom Enhancements:**
- Asegurar que componentes personalizados sean accesibles
- Probar con lectores de pantalla
- Mantener navegación por teclado

**5. Responsive Considerations (Future):**

**Desktop-First Approach:**
- Optimizar para desktop inicialmente
- Layout fijo para pantallas grandes
- Considerar responsive en futuras iteraciones

**Component Adaptation:**
- Usar sistema de grid de MudBlazor para layouts flexibles
- Preparar componentes para futura adaptación a diferentes tamaños

**6. Design Tokens:**

**Colors:**
- Primary: Color principal de la aplicación
- Secondary: Color secundario para acentos
- Success/Error/Warning/Info: Colores para estados

**Typography:**
- Font Family: Fuente principal legible
- Font Sizes: Escala de tamaños consistente
- Font Weights: Pesos de fuente para jerarquía

**Spacing:**
- Base Unit: Unidad base para espaciado (ej: 8px)
- Scale: Escala de espaciado consistente

**Border Radius:**
- Radio de borde consistente para componentes

**Shadows:**
- Elevación consistente para profundidad visual

### Learning & Development Strategy

**1. Phased Learning Approach:**

**Phase 1: Standard Components (Weeks 1-2)**
- Aprender componentes básicos de MudBlazor
- Implementar layout principal (mapa + panel)
- Usar componentes estándar sin personalización

**Phase 2: Theme Customization (Weeks 3-4)**
- Personalizar tema de MudBlazor
- Definir paleta de colores y tipografía
- Aplicar personalización a componentes existentes

**Phase 3: Custom Components (As Needed)**
- Crear componentes personalizados para características únicas
- Integrar componentes personalizados con estándar
- Documentar componentes personalizados

**2. Resources & Support:**

- **Documentation:** Usar documentación oficial de MudBlazor extensivamente
- **Examples:** Revisar ejemplos de código en documentación
- **Community:** Usar comunidad de MudBlazor para preguntas
- **Trial & Error:** Aprender haciendo, iterar rápidamente

**3. Best Practices:**

- **Start Simple:** Usar componentes estándar primero, personalizar después
- **Consistency:** Mantener consistencia con sistema de diseño de MudBlazor
- **Documentation:** Documentar decisiones de personalización
- **Testing:** Probar componentes en diferentes escenarios

