
README - Sistema de Gestión Legal SGLPWEB
==========================================

📝 Descripción del Proyecto
----------------------------
SGLPWEB es una aplicación web de gestión legal diseñada para abogados y estudios jurídicos.
Permite gestionar clientes, casos, tareas, evidencias y plazos judiciales.
El sistema ha sido desarrollado con ASP.NET Core MVC, Entity Framework Core y SQL Server como motor de base de datos.

📌 Funcionalidades Principales
-------------------------------
- Gestión de Clientes (CRUD completo)
- Gestión de Casos legales (CRUD completo)
- Registro de Evidencias, Plazos y Tareas asociadas a cada caso
- Interfaz amigable basada en Razor Views
- Integración con SQL Server local
- Validaciones del lado servidor
- Arquitectura MVC organizada con carpetas por modelos, DTOs, controladores y vistas

🛠 Tecnologías Utilizadas
--------------------------
- ASP.NET Core MVC 8
- Entity Framework Core
- SQL Server (local y Docker)
- Bootstrap (diseño básico)
- Visual Studio 2022
- Postman (para pruebas API iniciales)

⚙️ Configuración del Entorno
-----------------------------
1. Clonar el repositorio o abrir el proyecto en Visual Studio.
2. Configurar la cadena de conexión en appsettings.json si es necesario.
3. Ejecutar las migraciones: Add-Migration Inicial, Update-Database.
4. Ejecutar la solución: se abrirá el navegador con el sistema.

📁 Estructura del Proyecto
---------------------------
- Controllers/
  - ClientesController.cs
  - CasosMVCController.cs
  - PlazosMVCController.cs
  - TareasMVCController.cs
  - EvidenciasMVCController.cs

- Models/
  - Cliente.cs
  - Caso.cs
  - Plazo.cs
  - Tarea.cs
  - Evidencia.cs

- DTO/
  - ClienteDTO.cs
  - CasoDTO.cs
  - ...

- Views/
  - ClientesMVC/
  - CasosMVC/
  - PlazosMVC/
  - TareasMVC/
  - EvidenciasMVC/

✅ Flujo Funcional y Pruebas
-----------------------------
- Se probó todo el CRUD (Create, Read, Update, Delete) de cada modelo desde la interfaz web.
- Se validó que los combos desplegables (relaciones) se cargan correctamente (por ejemplo, Cliente en Casos).
- Se corrigieron errores de validación de modelo.
- Se verificó que los datos se guardan en la base de datos local.

🔎 Observaciones
-----------------
- La propiedad de navegación Cliente en el modelo Caso generaba errores de validación.
- Se aplicó [JsonIgnore] y [BindNever] para evitar validaciones innecesarias.
- Se reorganizaron los DTOs en una carpeta separada para claridad estructural.

👩‍💻 Autora del Proyecto
-------------------------
Desarrollado por: destrada
