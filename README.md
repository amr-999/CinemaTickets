# 🎬 CinemaTickets – ASP.NET Core MVC (.NET 10)

Single-project cinema booking platform with Admin dashboard + Customer storefront.

---

## 📁 Full Structure

```
CinemaTickets/
├── CinemaTickets.slnx / .csproj   ← Open in Visual Studio
│
├── Models/          Category · Cinema · Hall · Actor · Movie · MovieActor · MovieSubImage
├── Data/            AppDbContext (EF Core, seed data)
├── Repository/      IGenericRepository · GenericRepository · IUnitOfWork · UnitOfWork
├── Helpers/         PaginatedList<T>
├── Services/        Image · Category · Cinema · Hall · Actor · Movie
├── ViewModels/      MovieVM · CinemaVM · ActorVM · HallVM · DashboardVM · ActorCheckboxVM
├── Extensions/      MovieStatusExtensions (ToBadge / ToDisplay)
│
├── Areas/
│   ├── Admin/       6 Controllers + 16 Views (full CRUD with filter + pagination)
│   └── Customer/    1 Controller + 2 Views (movie listing + details)
│
├── Views/Shared/    _AdminLayout · _CustomerLayout · _Pagination · _ValidationScripts
└── wwwroot/         admin.css · customer.css · admin.js · uploads/
```

---

## ✅ Features

### Admin Area  `/Admin/...`
| Feature | Detail |
|---|---|
| Dashboard | Stat cards · Status chart · Recent movies |
| Movies | Full CRUD + **Filter** (name/category/cinema/status) + **Pagination** |
| Categories | CRUD + **Search** + **Pagination** |
| Cinemas | CRUD + **Search** + **Pagination** |
| **Halls** | CRUD – screen/hall management per cinema |
| Actors | CRUD + **Search** + **Pagination** |
| Image Upload | Poster · Sub-gallery · Actor/Cinema photos |
| Many-to-Many | Movie ↔ Actor |
| Delete Modal | Shared Bootstrap confirm modal |

### Customer Area  `/Customer/...`
| Feature | Detail |
|---|---|
| Home Page | Movie card grid · Hero banner |
| **Filters** | Search · Genre · Cinema · Status (Now Showing / Coming Soon) |
| **Pagination** | 6 cards/page |
| Movie Card | Poster · Cinema · Hall · Date & Time · Available seats · Price |
| Details Page | Hero banner · Synopsis · Cast · Gallery · Booking info card |
| Seat Availability | Shows available/total with colour-coded progress bar |

---

## 🚀 Getting Started

### 1 — Set Connection String (`appsettings.json`)
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CinemaTicketsDB;Trusted_Connection=True;TrustServerCertificate=True"
```

### 2 — Package Manager Console

> **Default Project = `CinemaTickets`**   |   **Startup Project = `CinemaTickets`**

**First time:**
```
Add-Migration InitialCreate
Update-Database
```

**After pulling this update** (Hall model + Movie changes):
```
Add-Migration AddHallAndSeats
Update-Database
```

> The app also calls `db.Database.Migrate()` on startup – so the DB is created automatically.

### 3 — Run

Press **F5**.
- `/` → redirects to **Customer home** (movie listing)
- `/Admin` → Admin dashboard

---

## 🗃️ Database Schema

```
Category ──────────────────┐
                           ├──< Movie >──┬──< MovieActor >── Actor
Cinema ──< Hall >──────────┘             └──< MovieSubImage
```

---

## 🛠️ Tech Stack

- **ASP.NET Core MVC** (.NET 10)
- **Entity Framework Core 10** + SQL Server
- **Generic Repository + Unit of Work + Service Layer**
- **Bootstrap 5.3** · Font Awesome 6 · jQuery Validation
- **Two Areas**: Admin (dark sidebar) + Customer (cinema dark theme)

---

## 📦 Packages

| Package | Version |
|---|---|
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.0 |
| Microsoft.EntityFrameworkCore.Tools | 10.0.0 |
| Microsoft.EntityFrameworkCore.Design | 10.0.0 |

> **.NET 9** → change versions to `9.0.0` and `<TargetFramework>net9.0</TargetFramework>`  
> **.NET 8** → change versions to `8.0.0` and `<TargetFramework>net8.0</TargetFramework>`
