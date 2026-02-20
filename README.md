# 🛒 Frontek Full Web E-Commerce

![ASP.NET](https://img.shields.io/badge/ASP.NET-MVC-blue)
![C#](https://img.shields.io/badge/C%23-.NET-purple) ![SQL
Server](https://img.shields.io/badge/Database-SQL_Server-red)
![Bootstrap](https://img.shields.io/badge/UI-Bootstrap-7952B3)
![Architecture](https://img.shields.io/badge/Architecture-MVC%20%7C%20Service%20%7C%20Repository-green)

------------------------------------------------------------------------

## 📚 Información Académica

**Curso:** SC-601 Programación Avanzada\
**Profesor:** RODRIGUEZ GUZMAN JORGE PABLO\
**Cuatrimestre:** I Cuatrimestre

**Integrantes:** - Barrantes Jiménez Jonathan Steven - José Alejandro
González Agüero

------------------------------------------------------------------------

## 📑 Tabla de Contenidos

1.  [Resumen Ejecutivo](#-resumen-ejecutivo)
2.  [Objetivo del Proyecto](#-objetivo-del-proyecto)
3.  [Arquitectura del Sistema](#-arquitectura-del-sistema)
4.  [Requerimientos Funcionales](#-requerimientos-funcionales)
5.  [Requerimientos No Funcionales](#-requerimientos-no-funcionales)
6.  [Modelo de Base de Datos](#-modelo-de-base-de-datos)
7.  [Tecnologías Utilizadas](#-tecnologías-utilizadas)
8.  [Buenas Prácticas Implementadas](#-buenas-prácticas-implementadas)
9.  [Conclusión](#-conclusión)

------------------------------------------------------------------------

## 📌 Resumen Ejecutivo

Frontek Full Web E-Commerce es una plataforma de comercio electrónico
desarrollada bajo arquitectura en capas utilizando ASP.NET MVC.

El sistema permite:

-   Gestión completa de productos
-   Administración de usuarios y roles
-   Carrito de compras
-   Validación de inventario en tiempo real
-   Registro de órdenes y detalles
-   Historial de pedidos
-   Panel administrativo con estadísticas
-   Web API interna para operaciones asincrónicas

**Palabras clave:** E-Commerce, MVC, Web API, Arquitectura en Capas.

------------------------------------------------------------------------

## 🎯 Objetivo del Proyecto

Desarrollar una plataforma web orientada a la venta de componentes de PC
que permita:

-   Navegación pública del catálogo
-   Compra segura en línea
-   Gestión administrativa robusta
-   Escalabilidad mediante separación de responsabilidades

------------------------------------------------------------------------

## 🏗 Arquitectura del Sistema

El proyecto implementa una arquitectura basada en capas:

    Frontek
    │
    ├── Controllers
    ├── Models
    ├── DTOs
    ├── Services
    ├── Repositories
    ├── Data
    └── wwwroot

### Patrones Aplicados

-   MVC (Model-View-Controller)
-   Repository Pattern
-   Service Layer Pattern
-   Separación de responsabilidades (SRP)

------------------------------------------------------------------------

## ⚙ Requerimientos Funcionales

### Gestión del Catálogo

-   Administración de productos
-   Hasta 3 imágenes por producto
-   Control de estado activo/inactivo
-   Gestión de stock

### Gestión de Usuarios y Roles

-   Registro y autenticación
-   Contraseñas encriptadas
-   Roles:
    -   Cliente
    -   Administrador

### Carrito y Proceso de Compra

-   Agregar / modificar / eliminar productos
-   Validación de inventario al confirmar compra
-   Registro de orden y detalle

### Reseñas

-   Usuarios pueden publicar reseñas
-   Moderación por administrador

### Panel Administrativo

-   Total de usuarios
-   Total de productos
-   Productos con bajo inventario
-   Estadísticas de ventas

### Operaciones Asincrónicas

-   Web API interna
-   Carga dinámica de estadísticas
-   Gestión del carrito y reseñas

------------------------------------------------------------------------

## 🔐 Requerimientos No Funcionales

-   Acceso desde navegadores modernos
-   Autenticación y autorización segura
-   Arquitectura en capas
-   Operaciones asincrónicas (async/await)
-   Interfaz responsiva con Bootstrap

------------------------------------------------------------------------

## 🗄 Modelo de Base de Datos

### Entidades Principales

-   Usuario
-   Rol
-   Producto
-   Reseña
-   Orden
-   DetalleOrden
-   Tarjeta

### Relaciones Clave

-   Un Usuario pertenece a un Rol.
-   Un Usuario puede tener múltiples Órdenes.
-   Una Orden contiene múltiples Detalles.
-   Cada Detalle corresponde a un Producto.
-   Un Producto puede tener múltiples Reseñas.

Se implementan claves primarias y foráneas para garantizar integridad
referencial.

------------------------------------------------------------------------

## 💻 Tecnologías Utilizadas

-   ASP.NET MVC
-   C#
-   Entity Framework
-   SQL Server
-   Bootstrap
-   Web API
-   Async/Await

------------------------------------------------------------------------

## 🧠 Buenas Prácticas Implementadas

-   Separación clara de capas
-   Uso de DTOs para transferencia de datos
-   Validación de inventario en capa de servicio
-   Encriptación de contraseñas
-   Consultas asincrónicas para mejorar rendimiento
-   Diseño relacional normalizado

------------------------------------------------------------------------

## 🚀 Escalabilidad Futura

-   Implementación de pasarela de pagos real
-   Integración con servicios externos
-   Implementación de JWT
-   Implementación de microservicios
-   Dashboard avanzado con Power BI

------------------------------------------------------------------------

## 📌 Conclusión

Frontek Full Web E-Commerce representa una solución académica sólida que
integra arquitectura profesional, seguridad, separación de
responsabilidades y buenas prácticas de desarrollo.

El proyecto demuestra comprensión de:

-   Diseño relacional
-   Arquitectura en capas
-   Patrones de diseño
-   Control de inventario
-   Desarrollo web moderno

------------------------------------------------------------------------

© 2026 - Proyecto Académico SC-601 Programación Avanzada
