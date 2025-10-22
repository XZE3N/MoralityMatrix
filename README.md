# 🧭 Morality Matrix

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0-blue?logo=dotnet)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap)
![MSSQL](https://img.shields.io/badge/Database-MSSQL-yellow?logo=microsoft-sql-server)
![License](https://img.shields.io/badge/License-MIT-green)
![Status](https://img.shields.io/badge/Status-Active-success)

<p align="center">
  <img src="https://i.imgur.com/vz7SIa2.png" alt="Morality Matrix Banner" width="100%"/>
</p>

---

## 📘 Overview

**Morality Matrix** is a full-stack ASP.NET Core web application designed to support philosophy-related educational activities for Romanian 12th-grade students.  
The platform provides a structured, interactive way for students to explore moral and philosophical concepts through quizzes, guided exercises, and user engagement features.

It leverages **ASP.NET Core (Razor Pages + MVC)** for the backend, **Bootstrap 5** for responsive UI, and **Microsoft SQL Server** for persistent data management.

---

## ✨ Key Features

- **🎨 Responsive Design**  
  Built with Bootstrap to ensure a seamless experience across desktops, tablets, and mobile devices.

- **🔐 Secure Authentication & Authorization**  
  Powered by ASP.NET Core Identity, supporting **email verification** and **Google OAuth** login.

- **📊 Knowledge Quiz System**  
  A dynamic quiz module enabling students to test their understanding of philosophical concepts.

- **🧠 Admin Role & Question Management**  
  Custom admin dashboard for managing quiz questions, answers, and student results.

- **📧 Email Integration via Mailjet**  
  Configurable email sender for user registration confirmations, password resets, and admin notifications.

- **⚙️ Custom Exception Handling**  
  Centralized and user-friendly error management to ensure system stability.

- **🔒 Identity-Based Security**  
  Implements best practices for user account security, password hashing, and data protection.

- **🧩 Extensible Architecture**  
  Clean, modular project structure for maintainability and future feature expansion.

---

## 🛠️ Technologies Used

| Layer | Technology |
|-------|-------------|
| **Frontend** | Razor Pages, Bootstrap 5, HTML5, CSS3 |
| **Backend** | ASP.NET Core MVC (C#), Identity |
| **Database** | Microsoft SQL Server (Entity Framework Core) |
| **Authentication** | Email verification, Google OAuth 2.0 |
| **Email Service** | Mailjet API |
| **Hosting/Runtime** | .NET 6 / .NET 7 |

---

## ⚙️ Installation & Setup

### 1️⃣ Prerequisites

Ensure you have the following installed:

- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- A [Mailjet account](https://www.mailjet.com/) for email services

---

### 2️⃣ Clone the Repository

```bash
git clone https://github.com/yourusername/MoralityMatrix.git
cd MoralityMatrix/MoralityMatrix
```

---

### 3️⃣ Configure the Application

Edit the `appsettings.json` and `appsettings.Development.json` files with your settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:SERVER_NAME,SERVER_PORT;Initial Catalog=DATABASE_NAME;Persist Security Info=False;User ID=USERNAME;Password=PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR-GOOGLE-OAUTH-CLIENT-ID",
      "ClientSecret": "YOUR-GOOGLE-OAUTH-CLIENT-SECRET"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MailJetPrivateKey": "YOUR-MAILJET-PRIVATE-KEY",
  "MailJetPublicKey": "YOUR-MAILJET-PUBLIC-KEY"
}
```

---

### 4️⃣ Apply Database Migrations

Use the Entity Framework CLI to create and update the database:

```bash
dotnet ef database update
```

If EF Tools are missing:

```bash
dotnet tool install --global dotnet-ef
```

---

### 5️⃣ Run the Application

```bash
dotnet run
```

Or launch it directly from Visual Studio (`F5`).

Then open:  
👉 [https://localhost:5001](https://localhost:5001)

---

### 🌱 Database Seeding (Optional)

To prepopulate the database with sample **quiz questions and answer options**, two SQL scripts are included in the repository under:

```
Database/
 └── Seeds/
     ├── Questions.sql
     └── Options.sql
```

#### 🧩 How to Use

1. Open **SQL Server Management Studio (SSMS)** and connect to your local SQL Server instance.  
2. Ensure that the database specified in your `appsettings.json` (usually `MoralityMatrixDB`) exists.  
3. Open the scripts in this order:
   - `Questions.sql`
   - `Options.sql`
4. Execute each script (F5 or the “Run” button) to insert initial quiz data.  
5. Verify the data by running:
   ```sql
   SELECT COUNT(*) FROM Questions;
   SELECT COUNT(*) FROM Options;
   ```
   You should see several rows of seed data successfully inserted.

---

## 🧑‍🏫 Roles Overview

| Role | Description |
|------|--------------|
| **User** | Can register, verify email, and take quizzes |
| **Admin** | Can manage questions, answers, and oversee quiz data |

---

## 🖼️ Screenshots / Demo

| Screenshot | Description |
|-------------|--------------|
| <img src="https://i.imgur.com/U1qjbkS.png" alt="Home" width="500px"> | Landing page overview |
| <img src="https://i.imgur.com/YEOwgIR.png" alt="Home" width="500px"> | Home page illustrations |
| <img src="https://i.imgur.com/cLwl2J5.png" alt="Guide" width="500px"> | Guide page overview |
| <img src="https://i.imgur.com/zbIGqYj.png" alt="Login" width="500px"> | Login page |

---

## 🧩 Project Structure

```
MoralityMatrix/
 ├── Areas/
 │    └── Identity/         # Identity and account management
 ├── Data/                  # EF Core context and migrations
 ├── Pages/                 # Razor Pages (frontend UI)
 ├── Services/              # Custom logic (Mailjet, Exception Handling)
 ├── wwwroot/               # Static assets (CSS, JS, images)
 ├── appsettings.json       # Main configuration
 ├── Program.cs             # App entry point
 └── MoralityMatrix.csproj  # Project definition
```

---

## 🧾 License

This project is licensed under the terms of the **MIT License**.  
See the [LICENSE.txt](LICENSE.txt) file for details.

---

## 🤝 Contributing

Contributions, feature suggestions, and pull requests are welcome!  
Please fork the repository and submit your improvements.

---

## 📫 Contact

For support or inquiries, reach out via:

📧 **xze3n.py@gmail.com**  

---

> **Morality Matrix** — Empowering philosophical learning through interactive digital experiences.
