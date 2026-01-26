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


## Core Projects

- **Database**  
  Database project containing schema and data-related configuration.

- **MovieDB.REST-API**  
  RESTful API providing access to movie data and business logic.

- **MovieDB.SharedModels**  
  Shared data models used across the API and frontend applications.


## Frontend Clients

- **WPF-MovieDb**  
  Desktop frontend built with WPF for interacting with the API.

- **YourMovieDB**  
  Command-line interface (CLI) frontend for accessing movie data.


## Test Projects

- **MovieDB.REST-API.Test**  
  Tests for the REST API.

- **WPF-MovieDb.Test**  
  Tests for the WPF frontend.

- **YourMovieDB.Test**  
  Tests for the CLI frontend.
