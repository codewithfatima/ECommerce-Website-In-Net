### ECommerce

A full-stack e-commerce web application built with **ASP.NET Core**, **Entity Framework Core**, and **SQL Server**.  
Includes authentication, product browsing, shopping cart, order management, and an admin panel.

---

## 🚀 Features

### Customer
- Register / Login with JWT authentication
- Browse products with search, filter, sort, and pagination
- View product details and reviews
- Add products to cart, update quantities, remove items
- Place orders
- View order history
- Cancel orders (only when Pending or Confirmed)
- Wishlist (add/remove products)
- Profile management (edit name and image)

### Admin
- Role-based access (`Admin` role)
- View all orders
- Change order status (Pending → Confirmed → Shipped → Delivered → Cancelled)
- Customer name shown per order
- Colored status badges

---

## 🏗️ Architecture

The solution uses a **layered architecture**:
