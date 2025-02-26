# Gamatrain Backend

Gamatrain Backend is an ASP.NET Core-based RESTful API designed to support the Gamatrain platform. This backend handles user management, course management, governance tokens, and other core functionalities of the Gamatrain ecosystem.

---

## Table of Contents
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

---

## Features
- User Authentication and Management
- Course Management (CRUD Operations)
- Database Integration using Entity Framework Core
- Lightweight and Scalable RESTful API
- Unit Testing for Key Endpoints

---

## Technology Stack
- **Backend Framework:** ASP.NET Core
- **Database:** SQL Server
- **Authentication:** JWT (JSON Web Token)
- **Development Tools:** 
  - Entity Framework Core
  - Dapper
  - Fluent Validation
  - XUnit
  - Fluent Assertion
  - Swagger (API Documentation)

---

## Getting Started

### Prerequisites
Before you begin, ensure you have the following installed:
- [.NET SDK 9.0 or later](https://dotnet.microsoft.com/download)
- [Visual Studio Community 2022 - v17.12.3 or later](https://visualstudio.microsoft.com/vs/community/)
- [SQL Server 2022 or later](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Microsoft SQL Server management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16#download-ssms)

### Installation
1. **Clone the Repository:**
   ```bash
   git https://github.com/GamaEdtech/back.git
   cd back
   ``` 

### Run
1. **Development**

To open solution in visual studio run the following command

  ```bash
   start back.sln 
   ``` 


To create database and tables, go to tools menu and follow the below path:

(Tools >> NuGet Package Manager >> Package Manager Console)

![image](https://github.com/user-attachments/assets/097dccba-be05-45ce-862b-e210c8b5263e)

Then Select the GamaEdTech.back.DataSource as Default Project:

![image](https://github.com/user-attachments/assets/35346f7c-bf02-4277-b139-6d406dd56d13)

Then run the following command:

   ```bash
   update-database  
   ```

![image](https://github.com/user-attachments/assets/3752b597-1c2c-4cea-bec7-bbd61772fd63)

Then press ctrl + F5 to run the project.

![image](https://github.com/user-attachments/assets/a61ab89a-5c3f-49ef-88ad-ce000781b2a4)



