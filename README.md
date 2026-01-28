# MovieDB Monorepo


## About

This repository contains a monorepo with multiple projects that together form a
Movie Database system. The solution includes a database, a REST API, shared models,
a WPF desktop client, a CLI client, and corresponding test projects.

The project was created at the beginning of my apprenticeship to demonstrate
WPF development, REST API design, and concepts such as authentication and
authorization.

> **Note:**  
> As this project was developed at the start of my apprenticeship, some parts may not follow best practices or reflect my current development style.

---

## Database

Database project containing schema and data-related configuration.

### Technologies & Tools
- Entity Framework Core
- Microsoft SQL Server (MSSQL)

### Skills & Concepts
- SQL
- Migrations

---

## MovieDB.REST-API

RESTful API providing access to movie data and business logic.

### Technologies & Tools
- ASP.NET Core
- Entity Framework Core
- RESTful HTTP endpoints

### Skills & Concepts
- REST API design
- Authentication and authorization with JSON Web Tokens (JWT)

---

## WPF-MovieDb

Desktop frontend built with WPF for interacting with the API.

### Technologies & Tools
- Windows Presentation Foundation (WPF)

### Skills & Concepts
- Model-View-ViewModel design pattern (MVVM)
- Desktop UI design
- API consumption

---

## YourMovieDB  

Command-line interface (CLI) frontend for accessing movie data.

### Technologies & Tools
- .NET Framework

### Skills & Concepts
- Simple CLI design
- API consumption

---

## Test Projects

Test projects for validating the functionality of the application components.

### Technologies & Tools
- FluentAssertions
- Moq
- Microsoft Test Tools (MSTest)

### Skills & Concepts
- Mocking dependencies
- Unit testing
