---
stepsCompleted: [1]
inputDocuments: []
session_topic: 'UrbaGIStory - Sistema GIS para patrimonio histórico y planeamiento urbano'
session_goals: 'Explorar funcionalidades innovadoras, desafíos técnicos, UX, métricas para inmuebles, integraciones, y alcance MVP'
selected_approach: 'progressive-flow'
techniques_used: []
ideas_generated: []
context_file: ''
project_name: 'UrbaGIStory'
tech_stack: '.NET 10 + Blazor WASM + PostgreSQL/PostGIS + OpenLayers + QGIS'
---

# Brainstorming Session Results - UrbaGIStory

**Facilitador:** AllTech  
**Fecha:** 2025-12-25  
**Proyecto:** UrbaGIStory  

---

## Session Overview

**Topic:** Sistema de Información Geográfica para gestión del patrimonio histórico y planeamiento urbano

**Goals:**
- Explorar funcionalidades innovadoras para el sistema
- Resolver desafíos técnicos específicos
- Diseñar experiencia de usuario y flujos de trabajo
- Definir tipos de análisis y métricas para los inmuebles
- Identificar integraciones con otras herramientas
- Clarificar el alcance del MVP

### Project Context

**Stack Tecnológico:**
- Backend: .NET 10 Web API
- Frontend: Blazor WebAssembly
- Base de datos: PostgreSQL + PostGIS
- Mapas web: OpenLayers
- GIS Desktop: QGIS (herramienta externa)
- Autenticación: ASP.NET Identity + Roles

**Dominio:**
- Gestión de patrimonio histórico
- Planeamiento urbano
- Ciudades patrimoniales de Cuba
- Inmuebles con valor histórico

---

## Technique Selection

**Approach:** Progressive Technique Flow  
**Journey Design:** Systematic development from exploration to action

**Progressive Techniques:**
- **Phase 1 - Exploration:** What If Scenarios + Mind Mapping
- **Phase 2 - Pattern Recognition:** Six Thinking Hats
- **Phase 3 - Development:** Role Playing (User Perspectives)
- **Phase 4 - Action Planning:** Resource Constraints + Decision Tree

---

## 🌊 FASE 1: EXPLORACIÓN EXPANSIVA

**Técnicas:** What If Scenarios + Mind Mapping  
**Objetivo:** Generar 30+ ideas sin restricciones  
**Regla Principal:** ¡NO HAY IDEAS MALAS! Todo vale.

---

### 💡 IDEAS GENERADAS - Ronda 1

#### 🏛️ Modelo de Datos Flexible (CONCEPTO CLAVE)

**Insight Principal:** NO estructura fija - métricas y variables dinámicas

- **Variables dinámicas:** Crear métricas personalizadas sin modificar esquema de BD
- **Asignación flexible:** Cada edificio puede tener diferentes conjuntos de variables
- **Esquema adaptable:** El sistema debe permitir crear nuevos tipos de datos sobre la marcha
- **Metadata rica:** Los documentos/información tienen metadata embebida (fechas, autores, contexto)

**Implicación técnica:** Posible uso de JSON/JSONB en PostgreSQL para datos dinámicos, o patrón EAV (Entity-Attribute-Value)

---

#### 📅 Modelo Temporal/Cronológico (CONCEPTO CLAVE)

**Insight Principal:** La HISTORIA es una dimensión fundamental del sistema

- **Documentos fechados:** Cada documento/información tiene fecha histórica asociada
- **Línea temporal:** Trazar comportamientos y cambios a lo largo del tiempo
- **Análisis temporal:** Evaluar qué ha cambiado entre fechas
- **Metadata temporal:** Todo dato tiene contexto temporal

**Ideas derivadas:**
- Timeline visual por inmueble
- Comparación antes/después
- Alertas de cambios significativos
- Reportes de evolución histórica

---

#### 🔀 Agregación/Desagregación de Inmuebles (CONCEPTO CLAVE)

**Insight Principal:** Los edificios NO son estáticos - cambian su estructura administrativa/física

- **Fusión histórica:** 3 inmuebles actuales = 1 edificio hace 150 años
- **División histórica:** 1 edificio antiguo → múltiples inmuebles hoy
- **Documentos heredados:** Un documento puede referirse a estructura que ya no existe
- **Relaciones padre-hijo temporales:** Contenedores que cambian en el tiempo

**Implicación técnica:** Modelo de datos que soporte:
- Relaciones jerárquicas con vigencia temporal
- Herencia de documentos entre inmuebles relacionados
- Consultas que respeten el contexto histórico

---

#### 🎯 Predicciones y Planeamiento Urbano

**Insight Principal:** El sistema debe AYUDAR en decisiones de planeamiento

- **Predicción de deterioro:** ¿Qué edificios están en riesgo?
- **Análisis de tendencias:** Patrones de cambio en zonas
- **Soporte a decisiones:** Datos para planificación urbana
- **Disposición de espacios:** Análisis espacial para urbanismo

**Excluido explícitamente:**
- ❌ Modelos 3D
- ❌ Realidad aumentada

---

#### 👥 Usuarios y Roles

**Usuarios objetivo:**
- Investigadores
- Trabajadores de la Red de Oficinas del Historiador de Cuba
- Todos serán CAPACITADOS para usar el sistema

**Sistema de permisos:**
- Diferentes roles con diferentes capacidades
- Prestaciones según nivel de acceso
- Capacitación específica por rol

---

### 💡 IDEAS GENERADAS - Ronda 2

#### 🏢 Problema de Múltiples Pisos/Usos (IDENTIFICADO)

**El Desafío:**
- Un edificio de 4 plantas puede tener usos diferentes por piso
- Ej: Piso 2 = actividad turística, Piso 1 = clínica veterinaria
- Mapa 2D tradicional (vista desde arriba) no discrimina por altura
- Filtrar por "actividad económica" sería problemático

**Decisión Estratégica:**
- ⚠️ NO es prioridad resolver la visualización 3D ahora
- ✅ PRIORIDAD: Modelo de datos consistente que CAPTURE esta información
- 💡 Visualización dinámica se resuelve después con el modelo correcto

**Implicación:** El modelo de datos debe soportar:
- Múltiples "unidades" dentro de un inmueble
- Cada unidad con su piso/nivel
- Cada unidad con sus propias métricas/actividades

---

#### 📊 Tipos de Métricas (CONFIRMADO)

**Flexibilidad Total - Tipos soportados:**
- **Cuantitativas:** Números, mediciones, índices (1-10, metros², años)
- **Cualitativas:** Categorías, estados, clasificaciones
- **Descriptivas:** Texto libre, narrativas, observaciones
- **Calculadas:** Fórmulas basadas en otras métricas (futuro)

---

#### 🗺️ Flujo de Trabajo: Levantamiento Inicial

**Proceso de Digitalización:**
1. Abrir QGIS con mapa base de la ciudad
2. En otra ventana: perfiles urbanos (fotos de manzanas)
3. Identificar edificios visualmente
4. Marcar/dibujar en el mapa (crear geometrías)
5. Los datos van a PostgreSQL/PostGIS
6. UrbaGIStory lee esos datos y permite agregar métricas

**Herramientas paralelas:**
- QGIS → Geometrías, capas, edición espacial
- UrbaGIStory → Métricas, documentos, análisis

---

#### 🏙️ ENTIDADES URBANÍSTICAS (CONCEPTO CLAVE - NUEVO)

**Insight Principal:** NO es solo gestión de EDIFICIOS, es gestión URBANA completa

**Entidades del dominio urbanístico:**

| Entidad | Descripción | Relaciones |
|---------|-------------|------------|
| **Inmueble/Edificio** | Construcción individual | Pertenece a manzana |
| **Manzana** | Agrupación de edificios | Contiene inmuebles, delimitada por calles |
| **Calle** | Vía de circulación | Delimita manzanas, tiene fachadas |
| **Línea de Fachada** | Concepto urbanístico | Frente de edificios hacia calle |
| **Plaza** | Espacio público | Tiene características propias, rodeada de edificios |
| **Unidad (dentro de inmueble)** | Piso/local específico | Pertenece a inmueble, tiene uso propio |

**Implicación:** El modelo debe ser JERÁRQUICO y RELACIONAL:
```
Ciudad
  └── Zona/Distrito
       └── Manzana
            └── Inmueble
                 └── Unidad (piso/local)
                      └── Métricas/Documentos
```

**También necesitamos:**
- Calles como entidades con atributos
- Plazas como espacios con características
- Relaciones espaciales (qué da a qué calle, qué rodea qué plaza)

---

### 💡 IDEAS GENERADAS - Ronda 3

#### 🏠 Relación Inmueble ↔ Línea de Fachada (CLARIFICADO)

**Concepto Clave:** Un inmueble NO "da a una calle" directamente, sino a una **LÍNEA DE FACHADA**

**Definición:**
> **Línea de Fachada** = Intersección entre la CALLE y la MANZANA

```
         CALLE
    ═══════════════════
    ┌─────────────────┐  ← Línea de Fachada (frente de manzana hacia calle)
    │     MANZANA     │
    │  ┌───┬───┬───┐  │
    │  │Ed1│Ed2│Ed3│  │  ← Cada edificio "da" a la línea de fachada
    │  └───┴───┴───┘  │
    └─────────────────┘
```

**Casos especiales:**
- **Edificio en esquina:** Da a 2+ líneas de fachada
- **Edificio que atraviesa manzana:** Da a líneas de fachada opuestas
- **Edificio interior:** No da a ninguna línea de fachada (acceso por pasillo/patio)

**Relación en el modelo:**
```
Inmueble ←──(da a)──→ Línea de Fachada ←──(pertenece a)──→ Calle
                                       ←──(delimita)──→ Manzana
```

---

#### 🎯 ALCANCE DEL MVP (DEFINIDO)

**Decisión del Usuario:** TODO el modelo urbano desde la primera versión

**Entidades para MVP v1.0:**
- ✅ Ciudad
- ✅ Zona/Distrito  
- ✅ Manzana
- ✅ Calle
- ✅ Línea de Fachada
- ✅ Plaza
- ✅ Inmueble
- ✅ Unidad (piso/local dentro de inmueble)
- ✅ Métricas dinámicas para todas las entidades
- ✅ Documentos con metadata temporal
- ✅ Sistema de agregación/desagregación histórica
- ✅ Roles y permisos

**Nota:** Es un MVP ambicioso pero el usuario confirma que es necesario para el dominio.

---

## 🔍 FASE 2: RECONOCIMIENTO DE PATRONES

**Técnica:** Six Thinking Hats (Seis Sombreros para Pensar)

---

### ⚪ SOMBRERO BLANCO - Hechos y Datos

**Escala del Sistema:**
- **Área por oficina:** 80-100 manzanas máximo (área pequeña/manejable)
- **Usuarios potenciales:** Máximo 10 por oficina
- **Sistema existente:** NINGUNO - empezando desde cero
- **Uso previo de QGIS:** Muy limitado, pocas cosas hechas

**Implicaciones Técnicas:**
| Factor | Implicación |
|--------|-------------|
| Pocos usuarios | No necesita optimización extrema de rendimiento |
| Área pequeña | Base de datos manejable, no Big Data |
| Sin sistema previo | No hay migración de datos legacy |
| Greenfield total | Libertad para diseñar correctamente desde inicio |

**Contexto Organizacional:**
- Red de Oficinas del Historiador de Cuba
- Cada oficina tiene su área de trabajo definida
- Usuarios serán capacitados específicamente

---

### 🔴 SOMBRERO ROJO - Emociones e Intuición

**Lo que EMOCIONA al usuario:**
- 💚 Ofrecer un producto útil a la comunidad
- 💚 Saber que lo van a utilizar mucho y bien
- 💚 Contribuir a la preservación del patrimonio

**Lo que PREOCUPA al usuario:**
- ⚠️ No tener tiempo para dar soporte continuo
- ⚠️ Mantenimiento a largo plazo

**DECISIÓN ESTRATÉGICA CLAVE:**

> **NO es un MVP/POC iterativo** - Es un producto SÓLIDO, COMPLETO, de BAJO MANTENIMIENTO

| Enfoque Tradicional | Enfoque UrbaGIStory |
|---------------------|---------------------|
| MVP → Iterar → Expandir | Completo desde inicio |
| Soporte continuo | Mínimo soporte post-lanzamiento |
| Features incrementales | Scope fijo, no expandir |
| "Move fast, break things" | "Hazlo bien una vez" |

**Meta:** Capacitar usuarios, entregar producto sólido, mantenimiento mínimo.

---

### 📅 MODELO TEMPORAL CLARIFICADO

**NO es:** Versiones estructuradas de cada entidad (v1, v2, v3...)

**SÍ es:** Documentos con metadata temporal que el especialista interpreta

```
INMUEBLE "Casa Colonial #42"
    │
    ├── 📄 Documento: "Plano original" (1892)
    ├── 📄 Documento: "Foto fachada" (1920)
    ├── 📄 Documento: "Acta de división" (1955)
    ├── 📄 Documento: "Informe estado" (1987)
    ├── 📄 Documento: "Restauración" (2010)
    └── 📄 Documento: "Evaluación actual" (2024)
         │
         └── La LÍNEA TEMPORAL emerge de los documentos
             No es estructurada, es interpretativa
```

**Características:**
- Cantidad variable de documentos por inmueble
- Documentos de diferentes tipos
- Cada documento tiene su metadata (fecha, tipo, autor, etc.)
- El especialista interpreta la historia a partir de los documentos
- NO hay "versiones" fijas del inmueble

---

### ⚫ SOMBRERO NEGRO - Riesgos Identificados

| Riesgo | Mitigación |
|--------|------------|
| Modelo complejo desde inicio | Diseño cuidadoso, documentación |
| Poco tiempo para soporte | Código limpio, tests, documentación |
| Integración QGIS ↔ App | PostGIS como puente común |
| Agregación/desagregación compleja | Prototipo temprano de este módulo |

---

### 🟡 SOMBRERO AMARILLO - Beneficios Confirmados

- ✅ Sin legacy - diseño limpio desde cero
- ✅ Escala manejable (80-100 manzanas, ≤10 usuarios)
- ✅ Usuarios definidos y serán capacitados
- ✅ Producto con impacto real en la comunidad
- ✅ Enfoque SQL + JSONB permite flexibilidad sin complejidad

---

### 🔵 SOMBRERO AZUL - Conclusión Fase 2

**Patrones Clave Identificados:**
1. Producto completo, no iterativo
2. Bajo mantenimiento como prioridad
3. Modelo híbrido SQL + JSONB
4. Documentos como base de la línea temporal
5. Scope fijo desde el inicio

---

## 🏗️ FASE 3: DESARROLLO DE IDEAS

**Técnica:** Role Playing (Perspectivas de Usuario)
**Objetivo:** Refinar las ideas pensando como los usuarios reales

---

### 🔬 ROL: INVESTIGADOR (María)

**Flujo Principal: Búsqueda por Mapa**

```
┌─────────────────────────────────────────────────────────────────┐
│                         MAPA INTERACTIVO                        │
│                    (Punto de entrada principal)                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                                                         │   │
│  │     María navega el mapa, hace zoom, se ubica          │   │
│  │                                                         │   │
│  │              ┌──────────────┐                          │   │
│  │              │  📍 CLICK    │                          │   │
│  │              └──────┬───────┘                          │   │
│  │                     ▼                                   │   │
│  │         ┌─────────────────────┐                        │   │
│  │         │   POPUP/TOOLTIP     │                        │   │
│  │         │ ┌─────────────────┐ │                        │   │
│  │         │ │ 📷 Foto fachada │ │                        │   │
│  │         │ │    (actual)     │ │                        │   │
│  │         │ └─────────────────┘ │                        │   │
│  │         │ 📍 Calle X #123     │                        │   │
│  │         │ 🏘️ Manzana: 45      │                        │   │
│  │         │ 📐 L. Fachada: Norte│                        │   │
│  │         │                     │                        │   │
│  │         │ [Ver Detalle →]     │                        │   │
│  │         └─────────────────────┘                        │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼ (Click "Ver Detalle")
┌─────────────────────────────────────────────────────────────────┐
│                    VISTA DETALLE INMUEBLE                       │
│                                                                 │
│  ┌──────────────────┐  ┌────────────────────────────────────┐  │
│  │                  │  │ 📋 INFORMACIÓN GENERAL             │  │
│  │   📷 Galería     │  │ • Dirección completa               │  │
│  │   de fotos       │  │ • Manzana, Línea Fachada          │  │
│  │                  │  │ • Métricas actuales                │  │
│  └──────────────────┘  └────────────────────────────────────┘  │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ 🔀 RELACIONES (Agregación/Desagregación)                 │  │
│  │                                                          │  │
│  │  Este inmueble ANTES fue parte de:                       │  │
│  │  └── "Palacio González" (hasta 1955) [Ver →]            │  │
│  │                                                          │  │
│  │  Este inmueble AHORA contiene:                          │  │
│  │  └── Unidad A: Piso 1-2 (comercial)                     │  │
│  │  └── Unidad B: Piso 3-4 (residencial)                   │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ 📄 DOCUMENTOS (Línea Temporal)                           │  │
│  │ [Timeline visual o lista ordenada por fecha]             │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

**Funcionalidades para María:**
- ✅ Mapa como navegación principal
- ✅ Popup con info básica + foto fachada
- ✅ Confirmación visual antes de entrar al detalle
- ✅ Vista de relaciones (agregación/desagregación)
- ✅ Acceso a documentos históricos
- ✅ Métricas del inmueble

---

### 📝 ROL: TÉCNICO DE CAMPO (Carlos)

**Flujo de Trabajo: Levantamiento de Inmuebles**

```
┌─────────────────────────────────────────────────────────────────┐
│                    FASE 1: EN QGIS                              │
│                 (Trabajo geoespacial)                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. Conectar a BD PostgreSQL/PostGIS                           │
│                    ▼                                            │
│  2. Cargar imagen satelital + fotos tomadas en campo           │
│                    ▼                                            │
│  3. Crear polígonos (dibujar edificios)                        │
│     • Intersectar fotos con imagen satelital                   │
│     • Definir geometrías de cada inmueble                      │
│                    ▼                                            │
│  4. GUARDAR en BD (solo geometrías)                            │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼ (Cambio de herramienta)
┌─────────────────────────────────────────────────────────────────┐
│                 FASE 2: EN URBAGISTORY                          │
│              (Asignación de datos/metadata)                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. Ver en mapa los polígonos creados en QGIS                  │
│     (aparecen automáticamente, misma BD)                       │
│                    ▼                                            │
│  2. Seleccionar inmueble (click en polígono)                   │
│                    ▼                                            │
│  3. Asignar METADATA:                                          │
│     • Nombre/código del edificio                               │
│     • Dirección                                                │
│     • Manzana, línea de fachada                                │
│     • Tipo de inmueble                                         │
│                    ▼                                            │
│  4. Asignar MÉTRICAS:                                          │
│     • Estado de conservación                                   │
│     • Uso actual                                               │
│     • Variables personalizadas                                 │
│                    ▼                                            │
│  5. ATACHAR DOCUMENTOS:                                        │
│     • Fotos tomadas en campo                                   │
│     • Informes, planos, etc.                                   │
│     • Con su metadata (fecha, tipo, fuente)                    │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

**SEPARACIÓN DE RESPONSABILIDADES:**

| QGIS | UrbaGIStory |
|------|-------------|
| Crear geometrías | Asignar datos a geometrías |
| Editar polígonos | Asignar métricas |
| Trabajo espacial pesado | Gestión de documentos |
| Análisis GIS avanzado | Consultas y reportes |
| Capas y estilos | Interfaz amigable |

**Conexión entre ambos:**
```
QGIS ←──── PostgreSQL/PostGIS ────→ UrbaGIStory
           (Base de datos común)
```

**UrbaGIStory NO edita geometrías** - solo lee y muestra lo que QGIS creó.

---

### 📊 CONCLUSIÓN FASE 3

**Roles Explorados:**
- ✅ Investigador (María) - Navegación por mapa, consulta de información
- ✅ Técnico de Campo (Carlos) - Levantamiento QGIS + asignación datos en app

**Roles no explorados (considerados no críticos para MVP):**
- Jefe de Oficina (reportes/supervisión)
- Usuario Nuevo (onboarding)

**Insights Clave de UX:**
1. El MAPA es el punto de entrada principal
2. UrbaGIStory NO edita geometrías (solo QGIS)
3. Flujo: Ver → Confirmar → Detalle → Documentos/Métricas
4. Separación clara de responsabilidades QGIS ↔ App

---

## 📋 FASE 4: PLANIFICACIÓN DE ACCIÓN

**Técnica:** Resource Constraints + Decision Tree
**Objetivo:** Crear plan de implementación concreto con prioridades

---

### 📅 Contexto del Proyecto

| Factor | Decisión |
|--------|----------|
| **Deadline** | Ninguno - regalo + aprendizaje personal |
| **Orden** | Backend completo PRIMERO, Frontend después |
| **Experiencia** | Programador .NET Backend, frontend mínimo |
| **Agregación** | Desde el inicio, es core del sistema |

---

### 🏗️ MODELO CONTENEDOR/UNIDAD (Clarificado)

**Concepto simplificado de Agregación/Desagregación:**

```
┌─────────────────────────────────────────────────────────────┐
│                      CONTENEDOR                             │
│  (Puede ser: edificio físico, manzana completa, etc.)      │
│                                                             │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐          │
│  │ Unidad  │ │ Unidad  │ │ Unidad  │ │ Unidad  │          │
│  │   A     │ │   B     │ │   C     │ │   D     │          │
│  │ Hostal  │ │Restaurant│ │  Casa   │ │  Casa   │          │
│  │ activo  │ │ activo  │ │ inactivo│ │ activo  │          │
│  └─────────┘ └─────────┘ └─────────┘ └─────────┘          │
└─────────────────────────────────────────────────────────────┘

HOTEL GRANDE:
┌─────────────────────────────────────────────────────────────┐
│                      CONTENEDOR                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              Unidad: Hotel Completo                 │   │
│  │                    (activo)                         │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

**Manejo Temporal:**
- Cada unidad tiene estado: `activo` / `inactivo`
- Un edificio "muere" (inactivo) para que "nazcan" otros
- NO necesitan relación directa entre sí
- Solo importa: qué contenedor y qué unidades hay

**Modelo de Datos:**
```
Contenedor (tiene geometría de QGIS)
    └── Unidades[] (no tienen geometría propia)
            └── estado: activo/inactivo
            └── fecha_desde / fecha_hasta (opcional)
            └── métricas JSONB
            └── documentos[]
```

---

### 📊 MÉTRICAS FLEXIBLES - Ejemplos Confirmados

Todo esto entra en el campo JSONB de métricas:

**Actividades Económicas:**
```json
{
  "actividad_economica": "hostal",
  "tipologia": "alojamiento turístico",
  "capacidad": 12,
  "licencia": "activa"
}
```

**Datos Demográficos:**
```json
{
  "habitantes": 8,
  "edades": [45, 42, 18, 15],
  "ocupaciones": ["médico", "profesora", "estudiante", "estudiante"]
}
```

**Estado Físico:**
```json
{
  "estado_conservacion": 7,
  "ultimo_mantenimiento": "2023-05",
  "riesgo_estructural": "bajo"
}
```

**✅ CONFIRMADO:** El modelo JSONB permite cualquier tipo de métrica sin cambiar la BD.

---

### 🚀 PLAN DE IMPLEMENTACIÓN

**Filosofía:** Backend completo y sólido → Frontend después

---

#### FASE 1: Base de Datos y Modelo de Dominio

**Objetivo:** Crear el esquema completo en PostgreSQL/PostGIS

```
1.1 Entidades Geográficas (las que vienen de QGIS)
    ├── Ciudad
    ├── Zona/Distrito  
    ├── Manzana
    ├── Calle
    ├── LineaFachada
    ├── Plaza
    └── Contenedor (antes "Inmueble")

1.2 Entidades de Negocio
    ├── Unidad (dentro de Contenedor)
    │   ├── estado (activo/inactivo)
    │   ├── fecha_desde / fecha_hasta
    │   └── metricas JSONB
    ├── Documento
    │   ├── tipo, fecha_documento
    │   ├── archivo_url
    │   └── metadata JSONB
    └── TipoMetrica (catálogo opcional)

1.3 Seguridad
    ├── Usuario (ASP.NET Identity)
    ├── Rol
    └── Permisos
```

---

#### FASE 2: API REST (.NET 10 Web API)

**Objetivo:** Exponer toda la funcionalidad como API

```
2.1 Endpoints de Lectura (para el mapa)
    ├── GET /api/contenedores (con geometrías GeoJSON)
    ├── GET /api/contenedores/{id}
    ├── GET /api/contenedores/{id}/unidades
    ├── GET /api/manzanas
    └── GET /api/entidades-por-bbox (para el mapa)

2.2 Endpoints de Escritura
    ├── POST/PUT /api/unidades
    ├── POST/PUT /api/unidades/{id}/metricas
    ├── POST /api/documentos (upload)
    └── PUT /api/contenedores/{id} (solo metadata, no geometría)

2.3 Endpoints de Consulta
    ├── GET /api/buscar (búsqueda general)
    ├── GET /api/contenedores/por-manzana/{id}
    └── GET /api/estadisticas (agregados)

2.4 Autenticación/Autorización
    ├── POST /api/auth/login
    ├── POST /api/auth/refresh
    └── Middleware de permisos por rol
```

---

#### FASE 3: Lógica de Negocio

**Objetivo:** Reglas de negocio en servicios

```
3.1 Servicio de Métricas
    ├── Crear/editar métricas dinámicas
    ├── Validar tipos de datos
    └── Historial de cambios (opcional)

3.2 Servicio de Documentos
    ├── Upload de archivos
    ├── Generación de thumbnails (fotos)
    └── Búsqueda por metadata

3.3 Servicio de Unidades
    ├── Activar/desactivar unidades
    ├── Crear nuevas unidades en contenedor
    └── Consultas temporales
```

---

#### FASE 4: Frontend (Blazor WebAssembly)

**Objetivo:** Interfaz de usuario

```
4.1 Mapa Interactivo (OpenLayers)
    ├── Visualizar contenedores
    ├── Popup al hacer click
    └── Filtros visuales

4.2 Vistas CRUD
    ├── Detalle de contenedor
    ├── Lista/gestión de unidades
    ├── Gestión de documentos
    └── Editor de métricas

4.3 Administración
    ├── Gestión de usuarios
    └── Configuración de tipos de métricas
```

---

### 📊 RESUMEN DEL PLAN

| Fase | Componente | Tu Experiencia | Complejidad |
|------|------------|----------------|-------------|
| 1 | Base de Datos | ✅ Conoces | Media |
| 2 | API .NET | ✅ Tu fuerte | Media |
| 3 | Lógica Negocio | ✅ Tu fuerte | Media-Alta |
| 4 | Frontend Blazor | ⚠️ Nuevo | Alta (para ti) |

**Ventaja:** Las fases 1-3 son tu zona de confort. Cuando llegues al frontend, ya tendrás un backend sólido y probado.

---

## ✅ CONCLUSIONES DEL BRAINSTORMING

### Decisiones Clave Tomadas

| Área | Decisión |
|------|----------|
| **Modelo de datos** | Híbrido SQL + JSONB para flexibilidad |
| **Arquitectura** | Contenedor → Unidades (no relaciones directas entre unidades) |
| **Temporalidad** | Estado activo/inactivo + documentos con fechas |
| **Geometrías** | Solo QGIS las crea, UrbaGIStory solo lee |
| **Orden desarrollo** | Backend completo → Frontend |
| **Scope** | Completo desde v1, bajo mantenimiento |

### Stack Tecnológico Confirmado

| Componente | Tecnología |
|------------|------------|
| Backend | .NET 10 Web API |
| Frontend | Blazor WebAssembly |
| Base de datos | PostgreSQL + PostGIS |
| Mapas web | OpenLayers |
| Autenticación | ASP.NET Identity |
| GIS Desktop | QGIS (externo) |

### Entidades del Dominio

1. Ciudad
2. Zona/Distrito
3. Manzana
4. Calle
5. Línea de Fachada
6. Plaza
7. Contenedor (inmueble físico)
8. Unidad (espacio dentro de contenedor)
9. Documento (con metadata temporal)
10. Métrica (dinámica, JSONB)

### Próximos Pasos

1. ✅ **Completado:** Brainstorming
2. ⏳ **Siguiente:** Research (opcional) o Product Brief
3. ⏳ **Después:** PRD (Documento de Requisitos)
4. ⏳ **Luego:** Arquitectura técnica detallada

---

**Sesión finalizada: 2025-12-25**
**Duración: ~60 minutos**
**Ideas generadas: 50+**
**Decisiones tomadas: 12**


