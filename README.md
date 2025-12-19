# Task Management Web Application

## Overview
This project is a Task Management Web Application developed as part of an interview assignment.
It allows users to create, update, view, filter, and delete tasks.

The application is built using **ASP.NET Core MVC (.NET 8)** with **SQL Server** as the database.
All database operations are performed using **Dapper ORM** and **System.Data.SqlClient** with
stored procedures (DB First approach).

---

## Database Design

### ER Diagram (Logical View)

This application uses a single entity to keep the design simple and focused
for task lifecycle management.


Soft delete is implemented using the `IsActive` flag.

---

### Data Dictionary

#### Table: Task

| Column Name        | Data Type          | Description |
|-------------------|-------------------|-------------|
| Id                | NVARCHAR(40) (PK) | Unique task identifier (GUID as string) |
| Title             | NVARCHAR(200)     | Task title |
| Description       | NVARCHAR(1000)    | Task description |
| DueDate           | DATETIME          | Task due date |
| Status            | NVARCHAR(50)      | Task status |
| Remarks           | NVARCHAR(500)     | Additional remarks |
| IsActive          | BIT               | Soft delete flag |
| CreatedOn         | DATETIME          | Creation timestamp |
| CreatedById       | NVARCHAR(50)      | Creator user ID |
| CreatedByName     | NVARCHAR(100)     | Creator name |
| LastUpdatedOn     | DATETIME          | Last update timestamp |
| LastUpdatedById   | NVARCHAR(50)      | Last updated by user ID |
| LastUpdatedByName | NVARCHAR(100)     | Last updated by user name |

---

### Indexes Used

| Index Name | Type | Column | Purpose |
|-----------|------|--------|--------|
| PK_Task   | Clustered Primary Key | Id | Fast lookup and uniqueness |

---
The primary key index is sufficient for the current scope.
Filtering and searching are handled through stored procedures.

### Database Approach

**Approach Used:** DB First

**Reason:**
- Stored procedures are explicitly defined
- Better performance control with Dapper
- Easy SQL review for assessment
- Common enterprise practice

---

## Application Structure

- Architecture: **ASP.NET Core MVC (MPA)**
- Rendering: **Server-side Razor Views**
- SPA / API-only approach is **not used**

**Reason:**  
Simple, clean architecture suitable for CRUD-based assessment.

---

## Frontend

**Technologies Used**
- Razor Views (.cshtml)
- HTML5
- CSS3
- Bootstrap

**Type:** Web Application

---

## Build and Run

### Environment & Dependencies
- OS: Windows
- IDE: Visual Studio 2022
- Framework: **.NET 8**
- Database: SQL Server 2022
- NuGet Packages:
  - Dapper (2.1.66)
  - System.Data.SqlClient (4.9.0)

---

### Build Instructions
1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Restore NuGet packages
4. Build the solution

---

### Run Instructions
1. Create a database named `AssignmentDb`
2. Execute SQL scripts from the `Database` folder:
   - `Task` table
   - Stored Procedures:
     - `sp_createTask`
     - `sp_updateTask`
     - `sp_deleteTask`
     - `sp_getTaskById`
     - `sp_getTasks`
3. Update the connection string in `appsettings.json`
4. Set the MVC project as startup project
5. Run the application

---

## Notes
- Database access is implemented using **Dapper with System.Data.SqlClient**
- Stored procedures are used for all CRUD operations
- Soft delete is implemented using `IsActive`
