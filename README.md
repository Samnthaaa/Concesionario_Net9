# 🚗 Sistema de Gestión de Concesionario

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Razor Pages](https://img.shields.io/badge/Razor_Pages-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

**Sistema integral para la gestión de autos, conductores y reservas de un concesionario**

[Características](#-características) • [Instalación](#-instalación) • [Uso](#-uso) • [Tecnologías](#-tecnologías) • [Capturas](#-capturas-de-pantalla)

</div>

---

## 📋 Descripción

CrudRazorApp es un sistema web completo desarrollado con ASP.NET Core Razor Pages que permite administrar de manera eficiente la flota de vehículos, el personal de conductores y las reservas de un concesionario. El sistema incluye funcionalidades de búsqueda en tiempo real, generación de reportes PDF y una interfaz moderna y responsiva.

## ✨ Características

### 🚙 Gestión de Autos
- ✅ Crear, editar, visualizar y eliminar vehículos
- 🔍 Búsqueda en tiempo real por marca, modelo, año o placa
- 📄 Generación de reportes PDF personalizados
- ✔️ Selección múltiple para exportación masiva

### 👨‍💼 Gestión de Conductores
- ✅ Administración completa de información de conductores
- 📝 Registro de datos de contacto y licencias
- 📅 Control de fechas de contratación
- 📄 Exportación de reportes en PDF

### 📅 Sistema de Reservas
- ✅ Gestión integral de reservas de vehículos
- ⚠️ Validación de solapamiento de fechas
- 🎨 Estados visuales (Pendiente, En Curso, Finalizada)
- 📊 Historial completo de actividad

### 🎨 Interfaz de Usuario
- 🌟 Diseño moderno y minimalista
- 📱 Totalmente responsiva (mobile-first)
- 🔍 Búsqueda en tiempo real con feedback visual
- 🎯 Navegación intuitiva y consistente
- 🖼️ Hero banner con efecto parallax
- ✨ Animaciones suaves y transiciones fluidas

### 📊 Dashboard
- 📈 Estadísticas en tiempo real
- 📋 Actividad reciente de reservas
- 🎯 Accesos rápidos a secciones principales
- 📊 Contadores visuales interactivos

## 🛠️ Tecnologías

### Backend
- **ASP.NET Core 9.0** - Framework web principal
- **Razor Pages** - Motor de vistas del lado del servidor
- **Entity Framework Core 9.0** - ORM para acceso a datos
- **SQL Server** - Base de datos relacional

### Frontend
- **HTML5 / CSS3** - Estructura y estilos
- **JavaScript (Vanilla)** - Interactividad del cliente
- **CSS Custom Properties** - Sistema de diseño consistente
- **SVG Icons** - Iconografía vectorial

### Librerías y Paquetes
- **iText7 (9.3.0)** - Generación de documentos PDF
- **Microsoft.Data.SqlClient** - Proveedor de datos SQL Server
- **jQuery Validation** - Validación de formularios

## 📦 Instalación

### Prerrequisitos

```bash
- .NET 9.0 SDK o superior
- SQL Server 2019 o superior (Express, Developer o Enterprise)
- Visual Studio 2022 o VS Code con extensión C#
```

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/tu-usuario/crud-razor-app.git
cd crud-razor-app
```

2. **Configurar la base de datos**

Edita el archivo `appsettings.json` con tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AUTOS;User Id=tu_usuario;Password=tu_contraseña;TrustServerCertificate=True;"
  }
}
```

3. **Crear la base de datos**

Ejecuta el siguiente script SQL en tu servidor:

```sql
CREATE DATABASE AUTOS;
GO

USE AUTOS;
GO

-- Tablas del sistema
CREATE TABLE Auto (
    id INT IDENTITY(1,1) PRIMARY KEY,
    marca NVARCHAR(50),
    modelo NVARCHAR(50),
    año INT,
    color NVARCHAR(50),
    placa NVARCHAR(20)
);

CREATE TABLE Conductor (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100),
    apellido NVARCHAR(100),
    fechanacimiento DATETIME,
    telefono INT,
    email NVARCHAR(150),
    licencia NVARCHAR(20),
    fechaContratacion DATETIME
);

CREATE TABLE ReservaAuto (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idAuto INT FOREIGN KEY REFERENCES Auto(id) ON DELETE CASCADE,
    idConductor INT FOREIGN KEY REFERENCES Conductor(id) ON DELETE CASCADE,
    fechaInicio DATETIME NOT NULL,
    fechaFin DATETIME NOT NULL
);
```

4. **Aplicar migraciones (opcional)**

```bash
dotnet ef database update
```

5. **Restaurar paquetes**

```bash
dotnet restore
```

6. **Ejecutar la aplicación**

```bash
dotnet run
```

La aplicación estará disponible en `https://localhost:5001`

## 🎯 Uso

### Panel de Control
- Accede al dashboard principal para ver estadísticas generales
- Visualiza las últimas reservas y su estado actual
- Navega rápidamente a cualquier módulo del sistema

### Gestión de Vehículos
1. Navega a **Autos** desde el menú principal
2. Haz clic en **Registrar Auto Nuevo** para agregar vehículos
3. Usa la barra de búsqueda para encontrar autos específicos
4. Selecciona múltiples registros y descarga reportes en PDF

### Sistema de Reservas
1. Ve a **Reservas** y selecciona **Registrar Nueva Reserva**
2. Elige un auto disponible y un conductor
3. Establece las fechas de inicio y fin
4. El sistema validará automáticamente conflictos de horario
5. Visualiza el estado de cada reserva (Pendiente/En Curso/Finalizada)

### Generación de Reportes
1. En cualquier listado, marca los registros deseados
2. Haz clic en el botón **Descargar PDF**
3. Elige entre descargar seleccionados o todos los registros
4. El reporte se generará automáticamente con diseño profesional

## 📁 Estructura del Proyecto

```
CrudRazorApp/
├── Data/
│   └── AppDbContext.cs          # Contexto de Entity Framework
├── Models/
│   ├── Auto.cs                  # Modelo de vehículo
│   ├── Conductor.cs             # Modelo de conductor
│   ├── ReservaAuto.cs           # Modelo de reserva
│   ├── Mantenimiento.cs         # Modelo de mantenimiento
│   └── ReporteVehiculo.cs       # Modelo de reporte
├── Pages/
│   ├── Autos/                   # CRUD de autos
│   ├── Conductores/             # CRUD de conductores
│   ├── Reservas/                # CRUD de reservas
│   ├── Shared/
│   │   └── _Layout.cshtml       # Layout principal
│   └── Index.cshtml             # Dashboard
├── Services/
│   └── PdfService.cs            # Servicio de generación PDF
├── wwwroot/
│   ├── css/
│   │   ├── site.css             # Estilos principales
│   │   └── messages.css         # Estilos de mensajes
│   ├── js/
│   │   └── shared.js            # JavaScript compartido
│   └── images/                  # Recursos gráficos
└── Program.cs                   # Punto de entrada
```

## 🎨 Capturas de Pantalla

### Dashboard Principal
*Panel de control con estadísticas en tiempo real y actividad reciente*

### Gestión de Autos
*Listado de vehículos con búsqueda y selección múltiple*

### Formulario de Reserva
*Interfaz intuitiva para crear nuevas reservas*

### Reporte PDF
*Documento generado automáticamente con diseño profesional*

## 🔧 Configuración Avanzada

### Personalizar Colores

Edita las variables CSS en `wwwroot/css/site.css`:

```css
:root {
    --primary: #2563EB;
    --primary-dark: #1E40AF;
    --secondary: #64748B;
    --bg: #FFFFFF;
    --bg-secondary: #F8FAFC;
}
```

### Modificar Cadena de Conexión

En `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "tu_cadena_de_conexion"
  }
}
```

## 🤝 Contribuir

Las contribuciones son bienvenidas. Para cambios importantes:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👨‍💻 Autor

**Tu Nombre**
- GitHub: [@tu-usuario](https://github.com/tu-usuario)
- Email: tu.email@ejemplo.com

## 🙏 Agradecimientos

- Diseño inspirado en sistemas modernos de gestión
- Iconografía basada en Feather Icons
- Generación de PDF con iText7

---

<div align="center">

**⭐ Si este proyecto te fue útil, considera darle una estrella ⭐**

Hecho con ❤️ y ☕

</div>
