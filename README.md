# Sistema_Comercial
# 🏢 Sistema Comercial - Arriendo de Camionetas

Sistema web de gestión comercial para arriendos de camionetas. Se comunica con el Sistema de Mantención mediante gRPC.

## 📋 Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git
- **Sistema de Mantención** ejecutándose en `https://localhost:7002`

## 🔧 Instalación

### 1. Clonar el repositorio
```bash
git clone https://github.com/JJAroca/Sistema_Comercial.git
cd Sistema_Comercial
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

Abre en navegador: **https://localhost:7001**

## 📊 Características

- ✅ **Gestión de Clientes** - CRUD completo con Soft Delete
- ✅ **Registro de Arriendos** - Creación y finalización
- ✅ **Facturación Automática** - Generación con IVA 19%
- ✅ **Integración gRPC** - Consulta camionetas disponibles del Sistema de Mantención
- ✅ **Validaciones de Negocio** - No eliminar clientes con arriendos activos

## 🔗 Sistema Relacionado

**Sistema de Mantención:** https://github.com/JJAroca/Sistema_Mantencion.git

⚠️ **IMPORTANTE:** El Sistema de Mantención debe estar ejecutándose antes de usar este sistema.

## 🗂️ Estructura del Proyecto
```
Sistema_Comercial/
├── Controllers/          # Controladores MVC
│   ├── ClientesController.cs
│   ├── ArriendosController.cs
│   └── FacturasController.cs
├── Models/              # Modelos de datos
│   └── Cliente.cs
├── Views/               # Vistas Razor
│   ├── Clientes/
│   ├── Arriendos/
│   └── Facturas/
├── Data/                # Contexto de base de datos
│   └── ComercialDbContext.cs
├── Services/            # Servicios gRPC
│   └── ArriendoServiceImpl.cs
├── Protos/              # Definición gRPC
│   └── arriendos.proto
└── comercial.db         # Base de datos SQLite
```

## 🛠️ Tecnologías

- ASP.NET Core 8.0 MVC
- Entity Framework Core (Code First)
- SQLite
- gRPC (Cliente y Servidor)
- Bootstrap 5
- Razor Pages

## 📡 Servicios gRPC Consumidos

Este sistema consume los siguientes servicios del Sistema de Mantención:

- `ObtenerCamionetasDisponibles()` - Obtener lista de camionetas disponibles
- `ObtenerCamioneta(id)` - Obtener información de una camioneta
- `SolicitarRetiroCamioneta(id, motivo)` - Notificar inicio de arriendo
- `ReintegrarCamioneta(id, kilometraje)` - Notificar fin de arriendo

## 📝 Base de Datos

**Tablas principales:**
- `Clientes` - Información de clientes (con Soft Delete)
- `Arriendos` - Registros de arriendos
- `Facturas` - Facturas generadas automáticamente
- `PreciosArriendo` - Configuración de tarifas

## ⚠️ Solución de Problemas

### No se puede conectar con Sistema de Mantención
```bash
# Asegúrate de que el Sistema de Mantención esté corriendo en:
# https://localhost:7002
```

### Error: "No such table"
```bash
del comercial.db
dotnet ef database update
```

### Puerto 7001 ocupado
Edita `appsettings.json` y cambia el puerto:
```json
"Https": {
  "Url": "https://localhost:NUEVO_PUERTO"
}
```

## 👥 Autores

JuanJosé Aroca- Diego Beluzaran - Universidad Católica del Norte

## 📅 Proyecto

Evaluación 2 - Desarrollo e Integración de Soluciones  
Fecha de entrega: 27-11-2024
