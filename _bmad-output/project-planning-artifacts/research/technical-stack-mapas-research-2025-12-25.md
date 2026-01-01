---
stepsCompleted: ['init', 'comparison', 'integration', 'performance', 'summary']
inputDocuments: ['brainstorming-session-2025-12-25.md']
workflowType: 'research'
lastStep: 5
research_type: 'technical'
research_topic: 'Stack de Mapas Web para GIS'
research_goals: 'Comparar OpenLayers vs Leaflet vs MapLibre, integración .NET + PostGIS, rendimiento Blazor WASM'
user_name: 'AllTech'
date: '2025-12-25'
web_research_enabled: true
source_verification: true
project: 'UrbaGIStory'
---

# Technical Research: Stack de Mapas Web para UrbaGIStory

**Fecha:** 2025-12-25  
**Autor:** AllTech  
**Tipo de Investigación:** Técnica  
**Proyecto:** UrbaGIStory - Sistema GIS para gestión de patrimonio histórico y planeamiento urbano

---

## Resumen Ejecutivo

Esta investigación técnica evalúa las opciones de tecnología para el componente de mapas interactivos de UrbaGIStory, un sistema GIS gubernamental para la gestión del patrimonio histórico y planeamiento urbano.

**Decisiones Clave:**

| Componente | Decisión | Justificación |
|------------|----------|---------------|
| Librería de mapas | **OpenLayers** | WMS/WFS nativo, proyecciones, herramientas GIS completas |
| Base de datos espacial | PostgreSQL + PostGIS | Estándar industria, ya definido en stack |
| ORM espacial | NetTopologySuite + Npgsql | Integración nativa con EF Core |
| Integración Blazor | JS Interop custom | No existen wrappers maduros |

---

## Tabla de Contenidos

1. [Comparativa de Librerías de Mapas Web](#1-comparativa-de-librerías-de-mapas-web)
   - [Leaflet](#leaflet)
   - [OpenLayers](#openlayers)
   - [MapLibre GL JS](#maplibre-gl-js)
   - [Matriz Comparativa](#matriz-comparativa)
   - [Recomendación](#recomendación-para-urbagistory)
2. [Integración .NET + PostGIS](#2-integración-net--postgis)
   - [Arquitectura de Datos Espaciales](#arquitectura-de-datos-espaciales)
   - [Configuración de Paquetes](#configuración-de-paquetes)
   - [Modelo de Datos Espaciales](#modelo-de-datos-espaciales)
   - [Consultas Espaciales](#consultas-espaciales)
3. [Rendimiento Blazor WASM con Mapas](#3-rendimiento-blazor-wasm-con-mapas)
   - [Arquitectura de Integración](#arquitectura-de-integración)
   - [Patrón JS Interop](#patrón-js-interop)
   - [Consideraciones de Rendimiento](#consideraciones-de-rendimiento)
   - [Mejores Prácticas](#mejores-prácticas)
4. [Conclusiones y Próximos Pasos](#4-conclusiones-y-próximos-pasos)

---

## 1. Comparativa de Librerías de Mapas Web

### Leaflet

**Descripción:** Librería JavaScript ligera y fácil de usar para mapas interactivos.

**Pros:**
- ✅ Ligera (~40 KB)
- ✅ Curva de aprendizaje suave
- ✅ Amplia comunidad (41k+ estrellas GitHub)
- ✅ Ecosistema extenso de plugins
- ✅ Compatible con React, Vue, Angular

**Contras:**
- ❌ Funcionalidad GIS limitada por defecto
- ❌ Sin soporte nativo de proyecciones (requiere Proj4Leaflet)
- ❌ Sin WMS/WFS nativo (requiere plugins)
- ❌ Rendimiento degrada con >10k features
- ❌ Depende de plugins para características avanzadas

**Mejor para:** Aplicaciones simples, prototipos rápidos, dashboards con mapas básicos.

*Fuente: [Geoapify](https://www.geoapify.com/leaflet-vs-openlayers/)*

---

### OpenLayers

**Descripción:** Librería GIS completa con soporte nativo para estándares OGC.

**Pros:**
- ✅ Rica en funcionalidades GIS desde el inicio
- ✅ Soporte nativo WMS, WFS, KML, GML, GeoJSON
- ✅ Manejo de proyecciones y reproyecciones integrado
- ✅ Renderizado eficiente con Canvas y WebGL
- ✅ Herramientas de dibujo/edición geométrica
- ✅ Buen rendimiento con grandes datasets (>50k features)
- ✅ Soporte TypeScript robusto
- ✅ Actualizaciones frecuentes

**Contras:**
- ⚠️ Curva de aprendizaje pronunciada
- ⚠️ Paquete más pesado (~200 KB+)
- ⚠️ API más compleja

**Mejor para:** Aplicaciones GIS profesionales, sistemas gubernamentales, análisis geoespacial avanzado.

*Fuente: [Geoapify](https://www.geoapify.com/leaflet-vs-openlayers/)*

---

### MapLibre GL JS

**Descripción:** Fork open source de Mapbox GL JS, enfocado en mapas vectoriales con WebGL.

**Pros:**
- ✅ Renderizado WebGL de alta calidad
- ✅ Excelente rendimiento con mapas vectoriales
- ✅ Soporte 3D nativo
- ✅ Estilización dinámica avanzada
- ✅ Comunidad activa y creciente (8.5k estrellas)
- ✅ Sin dependencias externas

**Contras:**
- ⚠️ Menos maduro que Leaflet/OpenLayers
- ⚠️ Sin soporte nativo WMS/WFS
- ⚠️ Documentación en desarrollo
- ⚠️ Requiere conocimiento de WebGL
- ⚠️ Soporte de proyecciones limitado

**Mejor para:** Aplicaciones con visualizaciones 3D, mapas vectoriales personalizados, aplicaciones móviles.

*Fuente: [psanxiao.com](https://psanxiao.com/posts/2025-08-12-leaflet-openlayers-y-maplibre-en-2025.html)*

---

### Matriz Comparativa

| Aspecto | Leaflet | OpenLayers | MapLibre GL JS |
|---------|---------|------------|----------------|
| **GitHub Stars** | 41k+ ⭐ | 13k ⭐ | 8.5k ⭐ |
| **Tamaño bundle** | ~40 KB | ~200 KB+ | ~200 KB |
| **Curva aprendizaje** | ✅ Fácil | ⚠️ Pronunciada | ⚠️ Moderada |
| **WMS/WFS nativo** | ❌ Plugin | ✅ Sí | ❌ No |
| **Proyecciones** | ❌ Plugin | ✅ Nativo | ⚠️ Limitado |
| **KML/GML** | ❌ Plugin | ✅ Nativo | ❌ No |
| **Dibujo/Edición** | ⚠️ Plugin | ✅ Nativo | ⚠️ Limitado |
| **Rendimiento <10k** | ✅ Excelente | ✅ Excelente | ✅ Excelente |
| **Rendimiento >50k** | ❌ Degrada | ✅ Bueno | ✅✅ Mejor |
| **Renderizado 3D** | ❌ No | ⚠️ Limitado | ✅ Sí |
| **TypeScript** | ⚠️ Tipos externos | ✅ Nativo | ✅ Nativo |
| **Documentación** | ✅ Excelente | ✅ Buena | ⚠️ Creciendo |

*Fuente: [MDPI Research 2024](https://www.mdpi.com/2220-9964/14/9/336)*

---

### Recomendación para UrbaGIStory

**Decisión: OpenLayers**

| Requisito UrbaGIStory | OpenLayers | Justificación |
|-----------------------|------------|---------------|
| Consumir WMS/WFS oficiales | ✅ | Catastro, ortofotos, cartografía base |
| Proyecciones ETRS89/UTM | ✅ | Sistema de referencia español sin plugins |
| Importar KML/GML/Shapefile | ✅ | Intercambio con QGIS y otros sistemas |
| Dibujar/editar geometrías | ✅ | Delimitación de zonas, perímetros |
| Herramientas de medición | ✅ | Distancias, áreas, buffers |
| Compatibilidad QGIS | ✅ | Mismo ecosistema OGC |
| Volumen de datos esperado | ✅ | Miles de bienes patrimoniales |

**Alternativa considerada:** Leaflet sería viable pero requeriría múltiples plugins (Proj4Leaflet, leaflet-wms, leaflet-draw, etc.), aumentando complejidad y puntos de fallo.

---

## 2. Integración .NET + PostGIS

### Arquitectura de Datos Espaciales

```
┌─────────────────────────────────────────────────────────────────┐
│                        Arquitectura                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────────┐  │
│  │ Blazor WASM  │◄──►│ .NET Web API │◄──►│ PostgreSQL       │  │
│  │              │    │              │    │ + PostGIS        │  │
│  │ OpenLayers   │    │ Npgsql +     │    │                  │  │
│  │ (JS Interop) │    │ NTS          │    │ Geometrías       │  │
│  └──────────────┘    └──────────────┘    └──────────────────┘  │
│         │                   │                     │             │
│         ▼                   ▼                     ▼             │
│    GeoJSON            NetTopologySuite      PostGIS Types      │
│    (transferencia)    (C# geometries)       (storage)          │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Configuración de Paquetes

**Paquetes NuGet requeridos:**

```xml
<!-- PostgreSQL + Entity Framework Core -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.x" />

<!-- Soporte espacial NetTopologySuite -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite" Version="9.0.x" />

<!-- NetTopologySuite core (geometrías en C#) -->
<PackageReference Include="NetTopologySuite" Version="2.5.x" />

<!-- Conversión GeoJSON -->
<PackageReference Include="NetTopologySuite.IO.GeoJSON" Version="4.0.x" />
```

**Configuración DbContext:**

```csharp
// Program.cs o Startup.cs
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
    )
);
```

**Configuración de serialización GeoJSON:**

```csharp
// Para API controllers
services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new NetTopologySuite.IO.Converters.GeoJsonConverterFactory()
        );
    });
```

### Modelo de Datos Espaciales

**Ejemplo: Entidad BienPatrimonial**

```csharp
using NetTopologySuite.Geometries;

public class BienPatrimonial
{
    public int Id { get; set; }
    public string CodigoInventario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public CategoriaProteccion Categoria { get; set; }
    public EstadoConservacion Estado { get; set; }
    
    // Geometrías espaciales - NetTopologySuite
    public Point Ubicacion { get; set; }           // Punto central
    public Polygon? Perimetro { get; set; }        // Contorno del bien
    public MultiPolygon? ZonaProteccion { get; set; } // Área de amortiguamiento
    
    // Metadatos
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaUltimaInspeccion { get; set; }
    
    // Relaciones
    public int ZonaUrbanisticaId { get; set; }
    public ZonaUrbanistica ZonaUrbanistica { get; set; }
}

public class ZonaUrbanistica
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public TipoZona Tipo { get; set; }
    
    public MultiPolygon Geometria { get; set; }  // Límites de la zona
    
    public ICollection<BienPatrimonial> Bienes { get; set; }
}
```

**Configuración Fluent API:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.HasPostgresExtension("postgis");
    
    modelBuilder.Entity<BienPatrimonial>(entity =>
    {
        entity.Property(e => e.Ubicacion)
            .HasColumnType("geometry(Point, 25830)");  // ETRS89 / UTM zone 30N
            
        entity.Property(e => e.Perimetro)
            .HasColumnType("geometry(Polygon, 25830)");
            
        entity.HasIndex(e => e.Ubicacion)
            .HasMethod("GIST");  // Índice espacial
            
        entity.HasIndex(e => e.Perimetro)
            .HasMethod("GIST");
    });
}
```

### Consultas Espaciales

**Ejemplos de consultas espaciales comunes:**

```csharp
// 1. Bienes dentro de un radio (500m desde un punto)
public async Task<List<BienPatrimonial>> GetBienesEnRadio(
    double lon, double lat, double radioMetros)
{
    var punto = new Point(lon, lat) { SRID = 25830 };
    
    return await _context.BienesPatrimoniales
        .Where(b => b.Ubicacion.Distance(punto) <= radioMetros)
        .OrderBy(b => b.Ubicacion.Distance(punto))
        .ToListAsync();
}

// 2. Bienes dentro de una zona/polígono
public async Task<List<BienPatrimonial>> GetBienesEnZona(int zonaId)
{
    var zona = await _context.ZonasUrbanisticas.FindAsync(zonaId);
    
    return await _context.BienesPatrimoniales
        .Where(b => zona.Geometria.Contains(b.Ubicacion))
        .ToListAsync();
}

// 3. Bienes que intersectan con un área de búsqueda
public async Task<List<BienPatrimonial>> GetBienesIntersectan(Polygon areaBusqueda)
{
    return await _context.BienesPatrimoniales
        .Where(b => b.Perimetro != null && b.Perimetro.Intersects(areaBusqueda))
        .ToListAsync();
}

// 4. Buffer: Bienes a menos de 100m de una obra nueva
public async Task<List<BienPatrimonial>> GetBienesAfectadosPorObra(Point ubicacionObra)
{
    var bufferObra = ubicacionObra.Buffer(100); // 100m buffer
    
    return await _context.BienesPatrimoniales
        .Where(b => bufferObra.Intersects(b.Ubicacion) || 
                    (b.ZonaProteccion != null && bufferObra.Intersects(b.ZonaProteccion)))
        .ToListAsync();
}

// 5. Calcular área de un perímetro
public async Task<double> GetAreaPerimetro(int bienId)
{
    var bien = await _context.BienesPatrimoniales.FindAsync(bienId);
    return bien?.Perimetro?.Area ?? 0;
}
```

---

## 3. Rendimiento Blazor WASM con Mapas

### Arquitectura de Integración

**Realidad del ecosistema:**
- No existen wrappers Blazor maduros para OpenLayers
- La integración se realiza mediante **JavaScript Interop**
- El mapa "vive" en JavaScript, Blazor orquesta

```
┌────────────────────────────────────────────────────────────────┐
│                    Componente Blazor                            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  MapComponent.razor                                       │  │
│  │                                                           │  │
│  │  <div id="map-container" @ref="mapContainer"></div>      │  │
│  │                                                           │  │
│  │  @code {                                                  │  │
│  │      [Inject] IJSRuntime JS { get; set; }                │  │
│  │                                                           │  │
│  │      protected override async Task OnAfterRenderAsync()  │  │
│  │      {                                                    │  │
│  │          await JS.InvokeVoidAsync("MapInterop.init");    │  │
│  │      }                                                    │  │
│  │  }                                                        │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              ↕                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  wwwroot/js/map-interop.js                               │  │
│  │                                                           │  │
│  │  window.MapInterop = {                                   │  │
│  │      map: null,                                          │  │
│  │      init: function(config) { ... },                     │  │
│  │      addGeoJsonLayer: function(data) { ... },            │  │
│  │      setView: function(center, zoom) { ... },            │  │
│  │      onFeatureClick: function(callback) { ... }          │  │
│  │  }                                                        │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────────┘
```

### Patrón JS Interop

**Archivo: wwwroot/js/map-interop.js**

```javascript
window.MapInterop = {
    map: null,
    vectorSource: null,
    dotNetHelper: null,

    // Inicializar mapa
    init: function(containerId, config, dotNetRef) {
        this.dotNetHelper = dotNetRef;
        
        this.vectorSource = new ol.source.Vector();
        
        this.map = new ol.Map({
            target: containerId,
            layers: [
                new ol.layer.Tile({
                    source: new ol.source.OSM()
                }),
                new ol.layer.Vector({
                    source: this.vectorSource
                })
            ],
            view: new ol.View({
                center: ol.proj.fromLonLat(config.center),
                zoom: config.zoom
            })
        });

        // Click en features → callback a Blazor
        this.map.on('click', async (evt) => {
            const feature = this.map.forEachFeatureAtPixel(evt.pixel, f => f);
            if (feature && this.dotNetHelper) {
                await this.dotNetHelper.invokeMethodAsync(
                    'OnFeatureClicked', 
                    feature.getId()
                );
            }
        });
    },

    // Cargar GeoJSON desde URL (evita pasar datos por Blazor)
    loadGeoJsonFromUrl: async function(url, layerName) {
        const response = await fetch(url);
        const geojson = await response.json();
        
        const features = new ol.format.GeoJSON().readFeatures(geojson, {
            featureProjection: 'EPSG:3857'
        });
        
        this.vectorSource.addFeatures(features);
    },

    // Agregar WMS layer
    addWmsLayer: function(url, layerName) {
        const wmsLayer = new ol.layer.Tile({
            source: new ol.source.TileWMS({
                url: url,
                params: { 'LAYERS': layerName }
            })
        });
        this.map.addLayer(wmsLayer);
    },

    // Centrar en feature
    zoomToFeature: function(featureId) {
        const feature = this.vectorSource.getFeatureById(featureId);
        if (feature) {
            this.map.getView().fit(feature.getGeometry().getExtent(), {
                padding: [50, 50, 50, 50],
                duration: 500
            });
        }
    },

    // Limpiar
    dispose: function() {
        if (this.map) {
            this.map.setTarget(null);
            this.map = null;
        }
    }
};
```

**Componente Blazor: MapComponent.razor**

```razor
@inject IJSRuntime JS
@implements IAsyncDisposable

<div id="@containerId" style="width: 100%; height: @Height;"></div>

@code {
    [Parameter] public string Height { get; set; } = "500px";
    [Parameter] public double[] Center { get; set; } = new[] { -3.7, 40.4 }; // Madrid
    [Parameter] public int Zoom { get; set; } = 10;
    [Parameter] public EventCallback<int> OnFeatureSelected { get; set; }

    private string containerId = $"map-{Guid.NewGuid():N}";
    private DotNetObjectReference<MapComponent>? dotNetRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            dotNetRef = DotNetObjectReference.Create(this);
            
            await JS.InvokeVoidAsync("MapInterop.init", containerId, new
            {
                center = Center,
                zoom = Zoom
            }, dotNetRef);
        }
    }

    public async Task LoadBienesPatrimoniales()
    {
        // El mapa carga directamente del API, no pasa por Blazor
        await JS.InvokeVoidAsync(
            "MapInterop.loadGeoJsonFromUrl",
            "/api/bienes/geojson",
            "bienes"
        );
    }

    public async Task ZoomToBien(int bienId)
    {
        await JS.InvokeVoidAsync("MapInterop.zoomToFeature", bienId);
    }

    [JSInvokable]
    public async Task OnFeatureClicked(int featureId)
    {
        await OnFeatureSelected.InvokeAsync(featureId);
    }

    public async ValueTask DisposeAsync()
    {
        if (dotNetRef != null)
        {
            await JS.InvokeVoidAsync("MapInterop.dispose");
            dotNetRef.Dispose();
        }
    }
}
```

### Consideraciones de Rendimiento

| Aspecto | Impacto | Mitigación |
|---------|---------|------------|
| **Carga inicial WASM** | ⚠️ 2-5 segundos primera vez | AOT compilation, lazy loading de módulos |
| **JS Interop overhead** | ⚠️ ~0.1-1ms por llamada | Agrupar operaciones, minimizar llamadas |
| **Transferencia de datos** | ⚠️ Serialización costosa | Mapa carga GeoJSON directo del API |
| **Re-renders Blazor** | ⚠️ Pueden causar parpadeos | `@key`, `ShouldRender()`, `StateHasChanged()` controlado |
| **Memoria** | ⚠️ Acumulación en SPA | `IAsyncDisposable`, limpiar listeners |

### Mejores Prácticas

1. **El mapa vive en JavaScript**
   - Blazor solo orquesta y recibe eventos
   - No pasar geometrías grandes por JS Interop

2. **Datos directo al mapa**
   ```javascript
   // ✅ Correcto: mapa carga del API
   MapInterop.loadGeoJsonFromUrl('/api/bienes/geojson')
   
   // ❌ Evitar: pasar datos por Blazor
   // var data = await Http.GetAsync(...)
   // await JS.InvokeVoidAsync("addData", data)
   ```

3. **Minimizar JS Interop**
   ```csharp
   // ✅ Correcto: una llamada con múltiples operaciones
   await JS.InvokeVoidAsync("MapInterop.batchUpdate", new {
       center = newCenter,
       zoom = newZoom,
       layers = newLayers
   });
   
   // ❌ Evitar: múltiples llamadas
   // await JS.InvokeVoidAsync("setCenter", newCenter);
   // await JS.InvokeVoidAsync("setZoom", newZoom);
   // await JS.InvokeVoidAsync("setLayers", newLayers);
   ```

4. **Eventos inversos eficientes**
   - JS llama a Blazor solo en interacción del usuario
   - No polling ni updates constantes

5. **Lazy loading de capas**
   - Cargar capas bajo demanda
   - Viewport-based loading para grandes datasets

---

## 4. Conclusiones y Próximos Pasos

### Decisiones Técnicas Confirmadas

| Componente | Decisión | Confianza |
|------------|----------|-----------|
| Librería de mapas | **OpenLayers** | Alta ✅ |
| Base de datos espacial | PostgreSQL + PostGIS | Alta ✅ |
| ORM espacial | Npgsql + NetTopologySuite | Alta ✅ |
| Integración frontend | JS Interop custom | Media ⚠️ |

### Riesgos Identificados

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|------------|
| Curva aprendizaje OpenLayers | Media | Medio | Documentación, ejemplos progresivos |
| Performance JS Interop | Baja | Medio | Arquitectura correcta desde inicio |
| Complejidad debug Blazor+JS | Media | Bajo | Logging, DevTools, tests |

### Próximos Pasos

1. **Product Brief** - Formalizar visión del producto
2. **PRD** - Requisitos funcionales y no funcionales detallados
3. **Arquitectura** - Diseño técnico completo
4. **Spike técnico** - Prototipo OpenLayers + Blazor + PostGIS

---

## Referencias

- [Geoapify: Leaflet vs OpenLayers](https://www.geoapify.com/leaflet-vs-openlayers/)
- [psanxiao: Leaflet, OpenLayers y MapLibre en 2025](https://psanxiao.com/posts/2025-08-12-leaflet-openlayers-y-maplibre-en-2025.html)
- [MDPI: Performance Analysis Web Mapping Libraries 2024](https://www.mdpi.com/2220-9964/14/9/336)
- [Npgsql Documentation: Spatial Types](https://www.npgsql.org/efcore/mapping/nts.html)
- [NetTopologySuite Documentation](https://nettopologysuite.github.io/NetTopologySuite/)
- [OpenLayers Documentation](https://openlayers.org/doc/)

---

*Documento generado como parte del workflow de Research - BMAD Method*

