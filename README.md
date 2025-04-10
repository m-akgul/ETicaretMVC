
# ETicaret

This repository contains a full-featured e-commerce application built using **.NET 6** and **ASP.NET Core MVC**. It demonstrates a layered architecture with modern web technologies and best practices in software design.

---

## 📦 Features

- **User Authentication & Authorization**: Implemented with ASP.NET Core Identity, supporting roles like Admin, Personnel, and Customer.
- **Product Management**: Create, update, delete, and list products with category filters and sorting.
- **Shopping Cart & Checkout**: Interactive cart with quantity controls and order summary. Checkout includes discount code functionality.
- **User Profile & Order Tracking**: Users can edit profiles, view orders, and cancel unapproved orders.
- **Admin Panel**: Restricted access for product, category, and user management.
- **Validation**: Client-side and server-side validations on forms.
- **Responsive UI**: MVC Views with reusable components and ViewComponents like `CartSummary`.
- **Discount System**: Example discount code "ExampleCode" applied during checkout.
- **Role-Based Navigation**: UI adapts based on the user's role after login.

---

## 🧰 Technologies Used

- **.NET 6 SDK**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **MSSQL**
- **SQL Server Management Studio (SSMS)**

---

## 🚀 Getting Started

### Prerequisites

Ensure the following tools are installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-ssms)

### Installation Steps

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/m-akgul/ETicaretMVC.git
   cd ETicaretMVC
   ```

2. **Open Solution:**
   - Open `ETicaret.sln` in Visual Studio 2022.
   - Right-click `ETicaretUI` → Set as Startup Project.

3. **Configure Database:**
   - In `ETicaretData/Context/ETicaretContext.cs`, update the `UseSqlServer` method with your own server name:
     ```csharp
     options.UseSqlServer("Server=YOUR_SERVER_NAME;Database=ETicaret;Trusted_Connection=True;");
     ```

4. **Restore NuGet Packages:**
   If not automatically restored, use:
   ```bash
   dotnet restore
   ```

5. **Import the Database:**
   - Open SSMS and connect to your server.
   - Right-click on **Databases** → **Import Data-Tier Application**.
   - Select the `.bacpac` file found in the project directory to create the database.

6. **Run the Project:**
   - Press `F5` or click the run button in Visual Studio.
   - Alternatively, from the terminal:
     ```bash
     dotnet run
     ```

---

## 📂 Project Structure

- `ETicaretUI`: UI layer (Controllers, Views, ViewComponents, Program.cs)
- `ETicaretBusiness`: Business logic, services, generic repositories.
- `ETicaretDal`: Data access layer with interfaces and concrete repositories.
- `ETicaretData`: Entity models, EF Core `DbContext`, and ViewModels.

---

## 🔐 Sample Users

| Role      | Username  | Password  |
|-----------|-----------|-----------|
| Admin     | m-akgul   | 123456    |
| Personnel | kerem12   | asdfasdf  |
| Customer  | aliveli12 | asdasd    |

---

## 🖼️ Screenshots

### Home Page
![Home Page](images/home-page.png)

### Product Listing
![Product Listing](images/product-listing.png)

### Shopping Cart
![Shopping Cart](images/shopping-cart.png)

### Admin Panel
![Admin Panel](images/admin-panel.png)

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).
