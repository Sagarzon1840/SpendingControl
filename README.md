# SpendingControl

Aplicación y API REST en .NET 8 para gestionar gastos, depósitos y presupuestos mensuales por tipo de gasto.

## Características principales
- Registro y autenticación de usuarios con JWT.
- Tipos de gasto (Spend Types) y presupuestos mensuales (Budgets).
- Fondos monetarios (Monetary Funds) con saldo inicial y saldo actual calculado en memoria.
- Registro de gastos (Expenses) y depósitos (Deposits).
- Documentación interactiva con Swagger (ruta `/swagger`).
- Arquitectura por capas (Domain, Application, Infrastructure, Api).
- Persistencia con Entity Framework Core y SQL Server (pool de DbContext + reintentos).

## Arquitectura (vista general)
```
SpendingControl.Domain          -> Entidades y contratos (interfaces repositorios)
SpendingControl.Application     -> Casos de uso / servicios de aplicación
SpendingControl.Infrastructure  -> EF Core, repositorios concretos y DI
SpendingControl.Api             -> Controladores, DTOs, filtros, Startup/Program
```

## Especificaciones de la arquitectura
### Principios
- Separación de responsabilidades y dependencia unidireccional hacia el dominio.
- El dominio NO depende de capas externas (clean architecture / onion style).
- La capa Application no conoce detalles de persistencia (trabaja con interfaces de repositorios).
- La infraestructura implementa detalles (EF Core, SQL Server, configuración de reintentos).
- La API actúa como composición final y punto de entrada HTTP.

### Dependencias permitidas
```
Domain <- Application <- Infrastructure <- Api (composición)
Domain <- Infrastructure (solo para implementar interfaces) ?
Api -> Application + Infrastructure (registro de servicios) ?
Api NO debe llamar directamente a EF Core ni a DbContext. ?
```

### Flujo de una petición
1. Cliente invoca endpoint en `SpendingControl.Api`.
2. Filtros (`ModelValidationFilter`, `ValidationExceptionFilter`) procesan validaciones iniciales.
3. Controlador traduce DTO -> Entidad (o parámetros simples).
4. Invoca servicio de aplicación (caso de uso) que orquesta reglas de negocio y repositorios.
5. Repositorio concreto (Infrastructure) usa `AppDbContext` para acceder a la base de datos.
6. Resultado vuelve al servicio, se transforma a DTO de respuesta y se serializa (enum como string). 

### Capas en detalle
- Domain: Entidades (`User`, `MonetaryFund`, etc.), enums (`FundType`), interfaces (`IUserRepository`, etc.). No contiene dependencias externas ni frameworks.
- Application: Servicios (`UserService`, ...). Implementa lógica de negocio: validación de contraseñas, reglas de unicidad, cálculos temporales (saldo actual in?memory). Puede incluir futuros patrones (mapeadores, manejadores de comandos). 
- Infrastructure: `AppDbContext`, repositorios concretos, configuración de resiliencia (retries en SQL Server), registro de servicios mediante `DependencyInjection.AddInfrastructure`. 
- Api: `Program` y `Startup` configuran el pipeline: CORS, autenticación JWT, Swagger, filtros globales, routing y endpoints raíz. Define DTOs para separación de modelos de dominio.

### Persistencia
- EF Core con SQL Server y `AddDbContextPool` para eficiencia.
- Retries configurados (`EnableRetryOnFailure`) contra transitorios.
- Repositorios encapsulan el acceso a datos evitando exposición del DbContext.

### Autenticación y seguridad
- JWT Bearer con validación estricta (`ClockSkew = 0`).
- Hash de contraseñas con `HMACSHA512` + salt por usuario.
- Futuros roles/autorización se integrarán vía `AddAuthorization` y políticas.

### Serialización
- Enums como string gracias a `JsonStringEnumConverter` (evita números mágicos en clientes).

### Validación
- Atributos DataAnnotations en DTOs.
- Filtros globales encapsulan respuesta de errores de modelo y excepciones de validación.

### Configuración
- `appsettings.Development.json` ignorado en control de versiones (secrets locales).
- Sección `Jwt` obligatoria para emitir/validar tokens.

### Extensibilidad prevista
- Añadir capa de Mapping (por ejemplo AutoMapper) para reducir código repetitivo.
- Integrar migraciones automatizadas controladas (bandera de entorno).
- Introducir capa de Application para comandos/queries (CQRS ligero) si escala.

### Diagrama ASCII simplificado
```
[HTTP Request]
     |
 [Api Controllers]
     |
 [Application Services]
     |
 [Repository Interfaces] <- Domain
     |
 [Repository Implementations]
     |
 [EF Core / SQL Server]
```

## Requisitos
- .NET 8 SDK
- SQL Server (local o remoto)

## Configuración
1. Crear cadena de conexión en `appsettings.Development.json` (no se versiona, ignorado en .gitignore). Clave: `ConnectionStrings:Default`.
2. Configurar sección JWT:
```json
"Jwt": {
  "Key": "clave-super-secreta",  
  "Issuer": "spendingcontrol",
  "Audience": "spendingcontrol"
}
```

## Ejecución local
```bash
# Restaurar
dotnet restore
# Ejecutar API
dotnet run --project SpendingControl.Api
# Abrir Swagger
http://localhost:5078/swagger
```
Puertos reales según `launchSettings.json`.

## Publicación (ejemplo)
```bash
dotnet publish SpendingControl.Api -c Release -o ./publish
```
El archivo `buildspec.yml` define pasos para AWS CodeBuild (restore + publish) generando artefactos de salida en `output`.

## Seguridad
- Autenticación mediante JWT Bearer (`Authorization: Bearer <token>`).
- Validación estricta de tiempo de vida (`ClockSkew = 0`);

## DTOs y entidades
Ejemplo de `MonetaryFund` expuesto mediante `MonetaryFundResponseDto` con conversión de enums a string via `JsonStringEnumConverter`.

## Buenas prácticas aplicadas
- Validaciones centralizadas vía filtros (`ModelValidationFilter`, `ValidationExceptionFilter`).
- Separación de responsabilidades clara.
- Hash de contraseñas con HMACSHA512 y salt por usuario.

## Próximas mejoras sugeridas
- Migraciones automáticas (dotnet ef) en arranque controlado.
- Tests unitarios y de integración.
- Paginación y filtros avanzados en listados.
- Roles/autorización por políticas.

## Licencia
Pendiente de definir.