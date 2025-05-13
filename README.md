📘 MVCApp Project Description
MVCApp is a .NET Core MVC application that implements a complete Employee Management
System following a clean architecture and 3-tier structure. The system includes fully 
functional modules for managing employees, departments, and user authentication 
using ASP.NET Core Identity.
===================================
🏗️ Project Structure
The solution is organized into three main projects for a clean separation of concerns:

Demo.PL (Presentation Layer)
Contains the MVC controllers, views, and UI logic.

Demo.BLL (Business Logic Layer)
Includes the application’s core business logic, services, and DTOs.

Demo.DAL (Data Access Layer)
Handles data persistence, database context, repositories, and entity configurations.

🚀 Main Features
1. 👨‍💼 Employee Management
The system supports full CRUD operations:

List employees with search functionality

Create new employees

View employee details

Update employee records

Delete employees

Related files: EmployeeController.cs

2. 🏢 Department Management
Fully functional department management:

View all departments

Create new departments

View department details

Update department information

Delete departments

Related files: DepartmentController.cs

3. 🔐 User Authentication & Identity
Integrated ASP.NET Core Identity system with:

User Registration

User Login

Logout functionality

Password Reset with email notifications

Related files: AccountController.cs

📦 Data Models & Architecture
Employee Model: Tracks full employee data including personal and job details.

File: Employee.cs

ApplicationDbContext: Configured with Identity and EF Core for data access.

File: ApplicationDbContext.cs

Architecture Pattern:
Uses Repository Pattern with Unit of Work to abstract and decouple data access logic.

Example: DepartmentService.cs

🔐 Security
All employee and department operations are protected and require user authentication.

See: [Authorize] attributes in EmployeeController and DepartmentController.

🧰 Technologies Used
ASP.NET Core MVC

Entity Framework Core

ASP.NET Core Identity

SQL Server

Bootstrap 4

📝 Notes
Implements clean 3-layered architecture (PL, BLL, DAL).

Includes comprehensive error handling and logging.

Follows best practices in code structure and separation of concerns.

Suitable for learning clean architecture and enterprise-level .NET Core development.

