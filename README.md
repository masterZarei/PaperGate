# 🧠 PaperGate  
### A Web-Based Platform for Managing Academic Papers & Theses  
#### Developed by Mohammad Mahdi Zarei | Sajjad University of Technology  

---

## 🌍 Overview

**PaperGate** is a web-based platform designed to manage, categorize, and display academic papers and theses.  
It provides a centralized and bilingual (Farsi-English) environment for universities and research institutions to organize and present their academic content efficiently.  
The system focuses on scalability, data integrity, and modern web design using **ASP.NET Core**, **SQL Server**, and **Tailwind CSS**.

---

## ⚙️ Technologies Used

| Layer / Purpose    | Technology / Tool                            |
|---------------------|----------------------------------------------|
| Backend             | ASP.NET Core 8                               |
| Frontend            | Razor Pages, Tailwind CSS                    |
| Database            | SQL Server                                   |
| ORM                 | Entity Framework Core                        |
| Architecture        | Clean Architecture                           |
| Authentication      | Microsoft Identity                           |
| Design Patterns     | Repository Pattern, Unit of Work             |

---

## 🏗 System Architecture

PaperGate follows **Clean Architecture**, ensuring that core business logic is independent from infrastructure and UI.  
It consists of four main layers:

- **Core:** Contains entities, interfaces, and domain logic.  
- **Infrastructure:** Handles data access, repositories, and external services.  
- **Shared:** Includes utility classes, enums, and shared models.  
- **Web:** Implements UI and user interactions via Razor Pages.

This structure enhances maintainability, testability, and scalability of the application.

---

## 🗄 Database Design

The database is normalized (3NF) and optimized for relational data.  
Key tables include:

- **Posts:** Stores articles and theses with attributes such as `Title`, `Content`, `IsActive`, and `ShowOnSlider`.  
  Each post belongs to a category and may contain multiple keywords through a many-to-many relation.  
- **Categories:** Manages hierarchical classification.
- **Keywords / PostKeywordInfo:** Defines a many-to-many relationship for tagging.  
- **Messages:** Captures user messages from the “Contact Us” page.  
- **AboutUs, UsefulLinks, ContactWays:** Manage static informational content of the site.

Foreign keys enforce referential integrity, and indexes are applied to frequent queries (e.g., `Title`, `CategoryId`) for performance.

---

## 🌐 Key Features

- Admin-only content management (no registration required).  
- Full CRUD operations for articles, categories, keywords, and messages.  
- Responsive design using Tailwind CSS.  
- Bilingual interface (Farsi–English) with dynamic language switching.  
- Featured post slider via `ShowOnSlider`.  
- Clean Architecture & SOLID principles.  
- Secure login and error logging using Microsoft Identity.  
- Contact form available for guests (no login needed).  

---

## 🧠 Repository & Unit of Work Pattern

PaperGate implements the **Repository Pattern** for abstracting data access and **Unit of Work Pattern** to ensure all database operations are handled in a transactional and consistent manner.  
This design improves modularity, testability, and reduces code duplication across repositories.

---

## 🚀 How to Run

```bash
git clone https://github.com/masterZarei/PaperGate.git  
cd PaperGate  
dotnet ef database update  
dotnet run  
````

1. Clone the repository.
2. Configure your database connection in `appsettings.json`.
3. Run migrations and update the database.
4. Launch the project and log in using the seeded admin account.

---

## 📚 Future Enhancements

* Add user roles such as *Student* and *Reviewer*.
* Implement full-text search and smart filters.
* Enable thesis file uploads.
* Develop RESTful APIs for integration with university systems.
* Introduce AI-based keyword and topic suggestions.

---

## 🙏 Acknowledgments

Special thanks to **Dr. Amir Bavafaa Toosi** for his guidance.

---


🧾 Developed by **Mohammad Mahdi Zarei**
🎓 B.Sc. Software Engineering – Sajjad University of Technology
📅 Academic Year: 2024–2025



---

<div dir=”rtl”>
## 🌍 معرفی کلی


 یک سامانه تحت وب است که برای مدیریت، دسته‌بندی و نمایش مقالات و پایان‌نامه‌های دانشگاهی طراحی شده است**PaperGate**.
این پروژه بستری متمرکز و دو‌زبانه (فارسی و انگلیسی) برای ساماندهی آثار پژوهشی در محیطی کاربرپسند، مقیاس‌پذیر و ایمن فراهم می‌سازد.
توسعه این پروژه با استفاده از **ASP.NET Core**، **SQL Server** و **Tailwind CSS** انجام شده است.

---

## ⚙️ فناوری‌های مورد استفاده

| بخش / لایه    | فناوری / ابزار                   |
| ------------- | -------------------------------- |
| بک‌اند        | ASP.NET Core 8                   |
| فرانت‌اند     | Razor Pages، Tailwind CSS        |
| پایگاه داده   | SQL Server                       |
| ORM           | Entity Framework Core            |
| معماری        | Clean Architecture               |
| احراز هویت    | Microsoft Identity               |
| الگوهای طراحی | Repository Pattern، Unit of Work |

---

## 🏗 معماری سیستم

پروژه از معماری تمیز (Clean Architecture) پیروی می‌کند تا جداسازی مناسبی میان منطق کسب‌وکار، داده و رابط کاربری برقرار باشد.
لایه‌ها شامل موارد زیر هستند:

* **Core:** شامل موجودیت‌ها و منطق اصلی برنامه.
* **Infrastructure:** پیاده‌سازی دسترسی به داده و سرویس‌های خارجی.
* **Shared:** شامل ابزارها و مدل‌های مشترک میان لایه‌ها.
* **Web:** مدیریت رابط کاربری و تعامل با کاربر.

این ساختار موجب افزایش پایداری، نگه‌داری‌پذیری و تست‌پذیری پروژه می‌شود.

---

## 🗄 طراحی پایگاه داده

پایگاه داده پروژه PaperGate به صورت نرمال‌سازی‌شده (3NF) طراحی شده است تا از تکرار داده جلوگیری و سرعت پردازش افزایش یابد.
جداول اصلی شامل:

* **Posts:** نگهداری مقالات و پایان‌نامه‌ها با فیلدهایی مانند عنوان، چکیده، تاریخ، وضعیت و قابلیت نمایش در اسلایدر.
* **Categories:** مدیریت دسته‌بندی‌های مقالات.
* **Keywords:** برقراری ارتباط چند‌به‌چند بین مقالات و کلیدواژه‌ها.
* **Messages:** ذخیره پیام‌های کاربران از فرم تماس با ما.
* **AboutUs / ContactWays / UsefulLinks:** مدیریت محتوای ثابت سایت.

---

## 🌐 ویژگی‌های کلیدی

* مدیریت کامل محتوای سایت تنها توسط ادمین
* پشتیبانی از رابط کاربری دو‌زبانه (فارسی و انگلیسی)
* طراحی واکنش‌گرا با Tailwind CSS
* پیاده‌سازی معماری تمیز و اصول SOLID
* قابلیت نمایش پست‌های منتخب در اسلایدر صفحه اصلی
* ثبت پیام توسط کاربران بدون نیاز به ورود
* ثبت لاگ خطاها و رویدادها جهت افزایش امنیت

---

## 🚀 نحوه اجرا

```bash
git clone https://github.com/masterZarei/PaperGate.git  
cd PaperGate  
dotnet ef database update  
dotnet run  
```

۱. مخزن پروژه را کلون کنید.
۲. اتصال پایگاه داده را در فایل `appsettings.json` تنظیم نمایید.
۳. مایگریشن‌ها را اجرا کرده و پایگاه داده را بروزرسانی کنید.
۴. پروژه را اجرا کرده و با حساب ادمین وارد شوید.

---

## 📚 کارهای آتی

* افزودن نقش‌های جدید مانند دانشجو و داور
* افزودن قابلیت جست‌وجوی پیشرفته
* توسعه API جهت ارتباط با سایر سامانه‌های دانشگاهی
* افزودن سیستم هوش مصنوعی برای پیشنهاد خودکار کلیدواژه‌ها
* امکان بارگذاری فایل پایان‌نامه‌ها

---

## 🙏 قدردانی

از راهنمایی‌های استاد محترم **جناب آقای دکتر امیر باوفا طوسی صمیمانه سپاسگزارم.

---


🧾 توسعه‌دهنده: **محمدمهدی زارعی**
🎓 کارشناسی مهندسی نرم‌افزار – دانشگاه سجاد
📅 سال تحصیلی: 1403–1404
</div>
```

