# 🔧 Sistema de Mantención - Flota de Camionetas

Sistema web de gestión de flota y mantenciones de camionetas. Proporciona servicios gRPC al Sistema Comercial.

## 📋 Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

## 🔧 Instalación

### 1. Clonar el repositorio
```bash
git clone https://github.com/JJAroca/Sistema_Mantencion.git
cd Sistema_Mantencion
```

### 2. Restaurar paquetes
```bash
dotnet restore
```

### 3. Crear la base de datos
```bash
dotnet ef database update
```

### 4. Configurar certificado HTTPS
```bash
dotnet dev-certs https --trust
```

## 🚀 Ejecutar
```bash
dotnet run
```

Abre en navegador: **https://localhost:7002**

⚠️ **IMPORTANTE:** Este sistema debe ejecutarse ANTES que el Sistema Comercial.

## 📊 Características

- ✅ **Gestión de Camionetas** - CRUD completo con Soft Delete
- ✅ **Control de Estados** - Disponible, En Arriendo, En Mantención
- ✅ **Registro de Mantenciones** - Preventiva, Correctiva, Revisión
- ✅ **Historial Completo** - Tracking de todos los cambios de estado
- ✅ **Servidor gRPC** - Proporciona información al Sistema Comercial
- ✅ **Validaciones** - No eliminar camionetas en uso

## 🔗 Sistema Relacionado

**Sistema Comercial:** https://github.com/JJAroca/Sistema_Comercial.git

## 🗂️ Estructura del Proyecto
```
Sistema_Mantencion/
├── Controllers/          # Controladores MVC
│   ├── CamionetasController.cs
│   └── MantencionesController.cs
├── Models/              # Modelos de datos
│   └── Camioneta.cs
├── Views/               # Vistas Razor
│   ├── Camionetas/
│   └── Mantenciones/
├── Data/                # Contexto de base de datos
│   └── MantencionDbContext.cs
├── Services/            # Servicios gRPC
│   └── MantencionServiceImpl.cs
├── Protos/              # Definición gRPC
│   └── arriendos.proto
└── mantencion.db        # Base de datos SQLite
```

## 🛠️ Tecnologías

- ASP.NET Core 8.0 MVC
- Entity Framework Core (Code First)
- SQLite
- gRPC (Servidor)
- Bootstrap 5
- Razor Pages

## 📡 Servicios gRPC Expuestos

Este sistema expone los siguientes servicios para el Sistema Comercial:

| Método | Descripción |
|--------|-------------|
| `ObtenerCamionetasDisponibles()` | Devuelve lista de camionetas con estado "Disponible" |
| `ObtenerCamioneta(id)` | Devuelve información detallada de una camioneta |
| `SolicitarRetiroCamioneta(id, motivo)` | Cambia estado a "En Arriendo" |
| `ReintegrarCamioneta(id, km)` | Cambia estado a "Disponible" y actualiza kilometraje |
| `ActualizarKilometraje(id, km)` | Actualiza el kilometraje de una camioneta |

## 📝 Base de Datos

**Tablas principales:**
- `Camionetas` - Flota de vehículos (con Soft Delete)
- `Mantenciones` - Registros de mantenciones realizadas
- `HistorialCamioneta` - Log completo de todos los cambios

## 📦 Datos Iniciales

Al ejecutar `dotnet ef database update`, se crean automáticamente 5 camionetas de ejemplo:

| Patente | Marca | Modelo | Año | Kilometraje |
|---------|-------|--------|-----|-------------|
| ABCD12 | Toyota | Hilux | 2022 | 15,000 km |
| EFGH34 | Ford | Ranger | 2023 | 8,000 km |
| IJKL56 | Chevrolet | Colorado | 2021 | 25,000 km |
| MNOP78 | Nissan | Frontier | 2022 | 18,000 km |
| QRST90 | Mitsubishi | L200 | 2023 | 5,000 km |

## ⚠️ Solución de Problemas

### Error: "No such table"
```bash
del mantencion.db
dotnet ef database update
```

### Puerto 7002 ocupado
Edita `appsettings.json`:
```json
"Https": {
  "Url": "https://localhost:NUEVO_PUERTO"
}
```

### gRPC no responde
Verifica que el certificado HTTPS esté configurado:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

## 👥 Autores

Juan José Aroca - Universidad Católica del Norte

## 📅 Proyecto

Evaluación 2 - Desarrollo e Integración de Soluciones  
Fecha de entrega: 27-11-2024
