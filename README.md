# 🎓 Student Management System API

## 📌 Overview

This project is a **Student Management System API** built using **ASP.NET Core 8 Web API** following clean architecture principles.

It supports full CRUD operations with secure authentication and logging.

---

## 🚀 Features

* ✅ CRUD Operations (Create, Read, Update, Delete)
* 🔐 JWT Authentication
* 📦 Layered Architecture (Controller, Service, Repository)
* 🗄️ SQL Server (ADO.NET)
* 🛡️ Global Exception Handling (Middleware)
* 📊 Logging using Serilog
* 📘 Swagger API Documentation

---

## 🛠️ Technologies Used

* .NET 8 Web API
* SQL Server
* ADO.NET
* JWT Authentication
* Serilog
* Swagger (OpenAPI)

---

## ⚙️ Setup Instructions

### 1️⃣ Clone Repository

```bash
git clone https://github.com/Vishalchauhan7490/StudentManagementSystem.git
cd StudentManagementSystem
```

---

### 2️⃣ Configure Database

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

### 3️⃣ Create Database

Run this SQL:

```sql
CREATE DATABASE StudentDB;

USE StudentDB;

CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Age INT,
    Course NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

---

### 4️⃣ Run Project

```bash
dotnet run
```

---

### 5️⃣ Swagger URL

```
https://localhost:xxxx/swagger
```

---

## 🔐 Authentication

### Login API:

```
POST /api/auth/login
```

### Sample Request:

```json
{
  "username": "admin",
  "password": "1234"
}
```

👉 Copy token and use in Swagger:

```
Bearer YOUR_TOKEN
```

---

## 📂 Project Structure

```
Controllers/
Services/
Repositories/
Models/
Middleware/
Helpers/
```

---

## 🧠 Key Highlights

* Clean architecture implementation
* Async ADO.NET operations
* Structured logging with Serilog
* Secure API using JWT
* Centralized exception handling

---

## 👨‍💻 Author

Vishal Chauhan
