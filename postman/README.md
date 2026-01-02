# Postman Collections - UrbaGIStory API

Este directorio contiene todas las colecciones de Postman para la API de UrbaGIStory.

## Colecciones Disponibles

### 1. **UrbaGIStory-API-Auth.postman_collection.json**
- **Propósito:** Autenticación
- **Endpoints:**
  - `POST /api/Auth/login` - Iniciar sesión y obtener token JWT
- **Características:** Guarda el token automáticamente en la variable de entorno

### 2. **UrbaGIStory-API-Health.postman_collection.json**
- **Propósito:** Health checks
- **Endpoints:**
  - `GET /api/Health/database` - Verificar conexión a base de datos y PostGIS
- **Características:** No requiere autenticación

### 3. **UrbaGIStory-API-SystemAdmin.postman_collection.json**
- **Propósito:** Administración del sistema
- **Endpoints:**
  - `GET /api/SystemAdmin/qgis-config` - Obtener configuración QGIS
  - `PUT /api/SystemAdmin/qgis-config` - Actualizar configuración QGIS
  - `GET /api/SystemAdmin/system-config` - Obtener configuración del sistema
  - `PUT /api/SystemAdmin/system-config` - Actualizar configuración del sistema
  - `POST /api/SystemAdmin/categories/load` - Cargar categorías predefinidas
  - `GET /api/SystemAdmin/status` - Obtener estado del sistema
- **Características:** Requiere rol `TechnicalAdministrator`

### 4. **UrbaGIStory-API-Backup.postman_collection.json**
- **Propósito:** Backup y restauración de base de datos
- **Endpoints:**
  - `POST /api/Backup` - Crear backup
  - `GET /api/Backup` - Listar backups disponibles
  - `POST /api/Backup/restore` - Restaurar desde backup
- **Características:** Requiere rol `TechnicalAdministrator`

### 5. **UrbaGIStory-API-Logs-Monitoring.postman_collection.json**
- **Propósito:** Logs y monitoreo del sistema
- **Endpoints:**
  - `GET /api/Logs` - Listar logs con filtros y paginación
  - `GET /api/Logs/export` - Exportar logs como JSON
  - `GET /api/Monitoring/metrics` - Obtener métricas de rendimiento
- **Características:** Requiere rol `TechnicalAdministrator`

### 6. **UrbaGIStory-API-Users.postman_collection.json**
- **Propósito:** Gestión de usuarios y roles
- **Endpoints:**
  - `POST /api/Users` - Crear nuevo usuario
  - `GET /api/Users` - Listar usuarios con filtros y paginación
  - `GET /api/Users/{id}` - Obtener usuario por ID
  - `PUT /api/Users/{id}` - Actualizar usuario
  - `GET /api/Users/{id}/roles` - Obtener roles del usuario
  - `POST /api/Users/{id}/roles` - Agregar rol a usuario (permite múltiples roles)
  - `PUT /api/Users/{id}/roles` - Reemplazar todos los roles con uno nuevo
  - `DELETE /api/Users/{id}/roles/{roleName}` - Eliminar un rol específico del usuario
  - `DELETE /api/Users/{id}` - Desactivar usuario (soft delete)
  - `POST /api/Users/{id}/activate` - Reactivar usuario desactivado
- **Características:** Requiere rol `TechnicalAdministrator`

## Environment

### **UrbaGIStory-API.postman_environment.json**
Archivo de environment con las variables:
- `baseUrl`: http://localhost:5064
- `token`: (se establece automáticamente después del login)

## Instrucciones de Uso

### 1. Importar en Postman

1. Abre Postman
2. Importa todas las colecciones desde este directorio
3. Importa el environment: `UrbaGIStory-API.postman_environment.json`
4. Selecciona el environment importado

### 2. Flujo de Trabajo Recomendado

1. **Primero:** Ejecuta `Login` de la colección **Auth** para obtener el token
2. **Luego:** Usa cualquier otra colección según necesites:
   - **Health** para verificar el estado
   - **SystemAdmin** para administración
   - **Backup** para operaciones de backup/restore
   - **Logs-Monitoring** para ver logs y métricas
   - **Users** para gestión de usuarios

### 3. Autenticación Automática

Todas las colecciones tienen un script pre-request que automáticamente:
- Lee el token de la variable de entorno
- Lo agrega al header `Authorization: Bearer {token}`

No necesitas configurar el token manualmente en cada request.

## Notas Importantes

- **Token compartido:** Todas las colecciones usan el mismo token del environment
- **Orden de ejecución:** Siempre ejecuta `Login` primero para establecer el token
- **Roles requeridos:** 
  - SystemAdmin, Backup, Logs-Monitoring y Users requieren rol `TechnicalAdministrator`
  - Health no requiere autenticación
- **Base URL:** Por defecto es `http://localhost:5064`, cámbiala en el environment si es necesario

## Actualización de Colecciones

Cuando se agreguen nuevos endpoints a la API:
1. Actualiza la colección correspondiente manualmente
2. O usa el swagger.json para generar/actualizar: `http://localhost:5064/swagger/v1/swagger.json`

