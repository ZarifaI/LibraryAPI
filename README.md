# 📚 Library Management API

![C#](https://img.shields.io/badge/Backend-C%23%20ASP.NET-blueviolet)
![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-blue)
![Status](https://img.shields.io/badge/Status-Working-green)


A full-stack backend built with **C# (ASP.NET Core)** and **PostgreSQL**, exposing API routes to manage books in a digital library system.

## 🚀 Features

- `GET /books` – Fetch all books from the PostgreSQL database
- Clean RESTful API structure
- Returns JSON data
- PostgreSQL-powered with live data from `library_db`

## 🧰 Tech Stack

- ✅ ASP.NET Core Web API (C#)
- ✅ PostgreSQL
- ✅ Npgsql driver
- ✅ VS Code + pgAdmin

## 🛠️ How to Run

1. Clone the repository
2. Set your PostgreSQL connection in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Username=postgres;Password=yourpassword;Database=library_db"
   }
