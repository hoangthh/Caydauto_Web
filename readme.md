# CayDauTo - Stationery Website

CayDauTo is an online stationery website built with the goal of providing an easy and enjoyable shopping experience for stationery tools. Users can browse product samples, view product details, and add items to their cart for online shopping.

## 🚀 Key Features

- **Beautiful User Interface**: Modern design with a friendly and easy-to-use interface.
- **Product Catalog**: View, search, and filter products based on various criteria.
- **Product Details**: Display images, prices, and detailed information for each product.
- **Shopping Cart**: Add products to the cart and track selected items.
- **Mobile Optimization**: Ensures a great user experience on both desktop and mobile devices.

## 💻 Installation

### Prerequisites
- **NodeJS**: Install from https://nodejs.org/en
- **.NET**: Install .NET 8.0 or 9.0 from https://dotnet.microsoft.com/en-us/download/dotnet/8.0

### Steps

1. Clone the repository to your machine by running ```git clone https://github.com/hoangthh/Caydauto_Web.git```.

2. Install frontend dependencies: Navigate to the client folder with 
```cd client```
, then install the required packages using 
```npm install```.

3. Run the frontend application: Start the app with ```npm start```. It will be available at http://localhost:3000.

4. Install and start the Backend (ASP.NET): Open the server folder in Visual Studio or an IDE that supports ASP.NET. Restore the .NET dependencies with ```dotnet restore```. 
Update the database connection string in appsettings.json, for example: 
```"ConnectionStrings": { "DefaultConnection": "Data Source=<Your Database Name>; Initial Catalog=CayDauToDB; MultipleActiveResultSets=True; TrustServerCertificate=True; Trusted_Connection=True;" }```. 
Run migrations to create the database using 
```dotnet ef database update```. 
Start the Backend project with 
```dotnet run```. 
Finally, seed the database (e.g., Admin account) by running 
```dotnet run seed```.

## Notes
- Ensure both the frontend (client) and backend (server) are running simultaneously for full functionality.
- The frontend is built using ReactJS with Vite, while the backend uses ASP.NET Core.
- Adjust the database connection string in appsettings.json to match your local setup before running migrations.
## Directory Structure

**C:\SOAPROJECT\CAYDAUTO_WEB**  
│   **Caydauto_Web.sln** - The main solution file for the project.  
│  
├───**.config**  
│   │   **dotnet-tools.json** - Configuration file for .NET tools.  
│   Contains configuration files for .NET development tools.  
├───**client**  
│   │   **.env** - File containing environment variables for the client.  
│   │   **.gitignore** - List of files/folders ignored by Git.  
│   │   **eslint.config.js** - ESLint configuration for JavaScript linting.  
│   │   **index.html** - Main HTML file for the client application.  
│   │   **package-lock.json** - Lock file for npm package versions.  
│   │   **package.json** - Configuration file for the client project (dependencies, scripts).  
│   │   **README.md** - Documentation for the client section.  
│   │   **vite.config.js** - Configuration for Vite (frontend build tool).  
│   Main directory for the user interface (frontend).  
│   └───**src**  
│       │   **App.jsx** - Main React component of the application.  
│       │   **index.css** - Global CSS file for the application.  
│       │   **main.jsx** - Entry point for the React application.  
│       │   **mockData.js** - Mock data for development and testing.  
│       ├───**apis**  
│       │   │   **admin.js** - API for admin functionalities.  
│       │   │   **auth.js** - API for authentication (login, register).  
│       │   │   **axiosInstance.js** - Axios configuration for API calls.  
│       │   │   **cart.js** - API for cart operations.  
│       │   │   **category.js** - API for product categories.  
│       │   │   **color.js** - API for product colors.  
│       │   │   **delivery.js** - API for delivery operations.  
│       │   │   **favor.js** - API for favorites/wishlist.  
│       │   │   **filter.js** - API for filtering data.  
│       │   │   **location.js** - API for location-related operations.  
│       │   │   **order.js** - API for order management.  
│       │   │   **product.js** - API for product management.  
│       │   │   **profile.js** - API for user profile management.  
│       │   Contains API definition files for server communication.  
│       ├───**assets**  
│       │   │   **banner.svg**, **logo.svg**, ... - Static resources like images and icons.  
│       │   Stores static assets such as images and icons.  
│       ├───**components**  
│       │   ├───**Add**  
│       │   │   │   **Add.jsx** - Component for adding new data.  
│       │   │   Component for adding data (e.g., products).  
│       │   ├───**CartProduct**  
│       │   │   │   **CartProduct.jsx** - Logic for displaying cart products.  
│       │   │   │   **CartProduct.scss** - Styles for CartProduct.  
│       │   │   Displays products in the cart.  
│       │   ├───**DataTable**  
│       │   │   │   **DataTable.jsx** - Generic data table component.  
│       │   │   Data table for displaying information.  
│       │   ├───**Favor**  
│       │   │   │   **Favor.jsx** - Logic for managing favorites.  
│       │   │   Manages the favorites/wishlist.  
│       │   ├───**Filter**  
│       │   │   │   **Filter.jsx** - Logic for filtering data.  
│       │   │   │   **Filter.scss** - Styles for Filter.  
│       │   │   Data filtering component (e.g., product filters).  
│       │   ├───**Footer**  
│       │   │   │   **Footer.jsx** - Logic for the footer.  
│       │   │   Website footer component.  
│       │   ├───**LocationSelect**  
│       │   │   │   **LocationSelect.jsx** - Logic for location selection.  
│       │   │   │   **LocationSelect.scss** - Styles for LocationSelect.  
│       │   │   Component for selecting locations.  
│       │   ├───**Navbar**  
│       │   │   │   **Navbar.jsx** - Logic for the navigation bar.  
│       │   │   │   **Navbar.scss** - Styles for Navbar.  
│       │   │   Website navigation bar (menu).  
│       │   ├───**Order**  
│       │   │   │   **Order.jsx** - Logic for displaying orders.  
│       │   │   Displays order details.  
│       │   ├───**OrderList**  
│       │   │   │   **OrderList.jsx** - Logic for listing orders.  
│       │   │   │   **OrderList.scss** - Styles for OrderList.  
│       │   │   Lists all orders.  
│       │   ├───**Product**  
│       │   │   │   **Product.jsx** - Logic for displaying a product.  
│       │   │   │   **Product.scss** - Styles for Product.  
│       │   │   Displays a single product’s information.  
│       │   ├───**ProductList**  
│       │   │   │   **ProductList.jsx** - Logic for listing products.  
│       │   │   │   **ProductList.scss** - Styles for ProductList.  
│       │   │   Lists multiple products.  
│       │   ├───**Profile**  
│       │   │   │   **Profile.jsx** - Logic for user profile.  
│       │   │   │   **Profile.scss** - Styles for Profile.  
│       │   │   Displays user profile information.  
│       │   └───**Update**  
│       │   │   │   **Update.jsx** - Logic for updating data.  
│       │   │   Component for updating data (e.g., editing products).  
│       │   Contains reusable UI components.  
│       ├───**contexts**  
│       │   │   **AlertContext.jsx** - Manages global alerts/notifications.  
│       │   │   **AuthContext.jsx** - Manages authentication state.  
│       │   Manages global application state.  
│       ├───**helpers**  
│       │   │   **string.js** - Utility functions for string manipulation.  
│       │   Contains helper/utility functions.  
│       ├───**hooks**  
│       │   │   **useFetchProducts.js** - Custom hook for fetching products.  
│       │   Custom React hooks for the application.  
│       ├───**layout**  
│       │   │   **EmptyLayout.jsx** - Empty layout template.  
│       │   │   **MainLayout.jsx** - Main layout template for the app.  
│       │   Defines UI layouts.  
│       ├───**pages**  
│       │   ├───**AdminPage**  
│       │   │   │   **AdminPage.jsx** - Logic for the admin page.  
│       │   │   │   **AdminPage.scss** - Styles for AdminPage.  
│       │   │   Admin dashboard page.  
│       │   ├───**CartPage**  
│       │   │   │   **CartPage.jsx** - Logic for the cart page.  
│       │   │   │   **CartPage.scss** - Styles for CartPage.  
│       │   │   Shopping cart page.  
│       │   ├───**DetailPage**  
│       │   │   │   **DetailPage.jsx** - Logic for the detail page.  
│       │   │   │   **DetailPage.scss** - Styles for DetailPage.  
│       │   │   Detail page (e.g., product details).  
│       │   ├───**HomePage**  
│       │   │   │   **HomePage.jsx** - Logic for the homepage.  
│       │   │   │   **HomePage.scss** - Styles for HomePage.  
│       │   │   Website homepage.  
│       │   ├───**LoginPage**  
│       │   │   │   **LoginPage.jsx** - Logic for the login page.  
│       │   │   │   **LoginPage.scss** - Styles for LoginPage.  
│       │   │   User login page.  
│       │   ├───**NewsPage**  
│       │   │   │   **NewsPage.jsx** - Logic for the news page.  
│       │   │   News page.  
│       │   ├───**PaymentPage**  
│       │   │   │   **PaymentPage.jsx** - Logic for the payment page.  
│       │   │   │   **PaymentPage.scss** - Styles for PaymentPage.  
│       │   │   Payment processing page.  
│       │   ├───**PaymentSuccessPage**  
│       │   │   │   **PaymentSuccessPage.jsx** - Logic for payment success page.  
│       │   │   │   **PaymentSuccessPage.scss** - Styles for PaymentSuccessPage.  
│       │   │   Payment success confirmation page.  
│       │   ├───**ProductPage**  
│       │   │   │   **ProductPage.jsx** - Logic for the product listing page.  
│       │   │   │   **ProductPage.scss** - Styles for ProductPage.  
│       │   │   Product listing page.  
│       │   ├───**RegisterPage**  
│       │   │   │   **RegisterPage.jsx** - Logic for the registration page.  
│       │   │   │   **RegisterPage.scss** - Styles for RegisterPage.  
│       │   │   User registration page.  
│       │   ├───**SupportPage**  
│       │   │   │   **SupportPage.jsx** - Logic for the support page.  
│       │   │   Customer support page.  
│       │   └───**UserPage**  
│       │   │   │   **UserPage.jsx** - Logic for the user page.  
│       │   │   │   **UserPage.scss** - Styles for UserPage.  
│       │   │   User profile/information page.  
│       │   Contains the main pages of the application.  
│       └───**routes**  
│       │   │   **PrivateRoutes.jsx** - Defines protected routes.  
│       │   Defines application routes.  
└───**server**  
    │   **.gitignore** - List of files/folders ignored by Git.  
    │   **appsettings.Development.json** - Development environment configuration.  
    │   **appsettings.json** - General server configuration.  
    │   **Program.cs** - Entry point for the server application.  
    │   **server.csproj** - .NET project file for the server.  
    │   **server.http** - HTTP request configuration file.  
    Main directory for the backend of the application.  
    ├───**bin**  
    │   └───**Debug**  
    │       └───**net9.0** - Directory for compiled binaries targeting .NET 9.0.  
    │   Contains compiled binary files.  
    ├───**Controllers**  
    │   │   **AccountController.cs** - API controller for account operations.  
    │   │   **CartController.cs** - API controller for cart operations.  
    │   │   **CategoryController.cs** - API controller for categories.  
    │   │   **ColorController.cs** - API controller for colors.  
    │   │   **DeliveryController.cs** - API controller for delivery.  
    │   │   **DiscountController.cs** - API controller for discounts.  
    │   │   **OrderController.cs** - API controller for orders.  
    │   │   **ProductController.cs** - API controller for products.  
    │   │   **WishListController.cs** - API controller for wishlist.  
    │   Contains API controllers.  
    ├───**DAL**  
    │   └───**Repositories**  
    │       │   **CartRepository.cs** - Repository for cart data.  
    │       │   **CategoryRepository.cs** - Repository for category data.  
    │       │   **ColorRepository.cs** - Repository for color data.  
    │       │   **DiscountRepository.cs** - Repository for discount data.  
    │       │   **OrderRepository.cs** - Repository for order data.  
    │       │   **ProductRepository.cs** - Repository for product data.  
    │       │   **Repository.cs** - Base repository class.  
    │       │   **RoleRepository.cs** - Repository for role data.  
    │       │   **UserRepository.cs** - Repository for user data.  
    │       └───**Interfaces**  
    │       │   │   **ICartRepository.cs** - Interface for cart repository.  
    │       │   │   **ICategoryRepository.cs** - Interface for category repository.  
    │       │   │   **IColorRepository.cs** - Interface for color repository.  
    │       │   │   **IDiscountRepository.cs** - Interface for discount repository.  
    │       │   │   **IOrderRepository.cs** - Interface for order repository.  
    │       │   │   **IProductRepository.cs** - Interface for product repository.  
    │       │   │   **IRepository.cs** - Base repository interface.  
    │       │   │   **IRoleRepository.cs** - Interface for role repository.  
    │       │   │   **IUserRepository.cs** - Interface for user repository.  
    │       │   Data Access Layer (DAL) for database operations.  
    ├───**Data**  
    │   │   **SeedData.cs** - Logic for seeding initial data.  
    │   Contains data initialization logic.  
    ├───**Middlewares**  
    │   │   **UserIdMiddleware.cs** - Middleware for handling user IDs.  
    │   Contains custom middleware.  
    ├───**Migrations**  
    │   │   **20250404080148_Initial.cs** - Initial database migration.  
    │   │   **20250404080148_Initial.Designer.cs** - Designer file for initial migration.  
    │   │   **AppDbContextModelSnapshot.cs** - Snapshot of the database model.  
    │   Contains database migration files.  
    ├───**Models**  
    │   │   **AppDbContext.cs** - Main database context.  
    │   ├───**DTO**  
    │   │   │   **CartDTO.cs**, **ProductDTO.cs**, ... - Data Transfer Objects.  
    │   │   Data Transfer Objects (DTOs) for API communication.  
    │   ├───**Entities**  
    │   │   │   **Cart.cs**, **Product.cs**, ... - Database entities.  
    │   │   Database entity models.  
    │   ├───**Interface**  
    │   │   │   **IDateTracking.cs** - Interface for date tracking.  
    │   │   Interfaces for models.  
    │   └───**Pagination**  
    │   │   │   **PageResult.cs** - Pagination result structure.  
    │   │   Pagination logic.  
    │   Defines data models.  
    ├───**obj**  
    │   │   **project.assets.json**, ... - Temporary files generated during compilation.  
    │   └───**Debug**  
    │       └───**net9.0** - Compiled files for .NET 9.0.  
    │   Contains temporary compilation files.  
    ├───**Properties**  
    │   │   **launchSettings.json** - Server launch configuration.  
    │   Server project configuration.  
    ├───**Services**  
    │   │   **AccountService.cs**, **ProductService.cs**, ... - Business logic services.  
    │   ├───**Interfaces**  
    │   │   │   **IAccountService.cs**, **IProductService.cs**, ... - Service interfaces.  
    │   │   Interfaces for services.  
    │   └───**Mapping**  
    │   │   │   **MappingProfile.cs** - Data mapping configuration (e.g., AutoMapper).  
    │   │   Data mapping logic.  
    │   Contains business logic services.  
    └───**Utilities**  
    │   **Constraint.cs** - Utility for constraints/rules.  
    │   **Enum.cs** - Enum definitions.  
    │   **Extension.cs** - Extension methods.  
    │   **VnPayLibrary.cs** - Utility for VnPay payment integration.  
    Contains utility classes and helpers.  

## Notes
- The **client** is built using ReactJS with Vite as the build tool.
- The **server** is developed using .NET 9.0 as the backend framework.
- The project is divided into two main parts: the frontend (client) and backend (server).
## 🛠️ Công nghệ sử dụng

![React](https://img.shields.io/badge/react-%2320232a.svg?style=for-the-badge&logo=react&logoColor=%2361DAFB)
![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)
![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
![Vercel](https://img.shields.io/badge/vercel-%23000000.svg?style=for-the-badge&logo=vercel&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)

## 🛠️ Technologies Used

### FrontEnd

- **React.js**:  
  React.js is a powerful JavaScript library used to build interactive and efficient user interfaces (UI). React simplifies application state management through components. These components are independent, reusable UI building blocks that are well-organized, making it easy to maintain and scale the application. React utilizes a Virtual DOM mechanism, which speeds up UI updates, delivering a smooth user experience.

- **SCSS (Sassy CSS)**:  
  SCSS is an extension of CSS that provides advanced features like variables, inheritance, and loops, making CSS management and optimization more straightforward. SCSS helps structure CSS code in a project in a clear, maintainable way compared to regular CSS. This is especially useful for large and complex applications like CayDauTo, enabling a consistent and scalable interface.

### BackEnd

- **ASP.NET Core**:  
  ASP.NET Core is an open-source, cross-platform framework for developing web applications. In this project, ASP.NET Core serves as the backend, providing APIs and handling server-side logic. With ASP.NET Core, we can build RESTful APIs, connect to and process data from a database, and perform complex tasks such as user authentication and authorization management. ASP.NET Core offers excellent scalability, high security, and optimized performance, making it easy to develop and integrate with other services.

- **Entity Framework Core (EF Core)**:  
  Entity Framework Core is an open-source ORM (Object-Relational Mapper) for .NET, enabling efficient and secure database interactions. EF Core converts C# objects into database records and vice versa, simplifying data handling without requiring complex SQL queries. It also provides migration features to manage and update the database structure when the codebase changes, ensuring consistency between code and data. This is crucial for web application development as it efficiently manages and maintains data records while minimizing errors.

### Database

- **SQL Server**:  
  SQL Server is used as the database management system in this project. With its security features, high performance, and scalability, SQL Server is well-suited for managing data in an e-commerce web application. Paired with Entity Framework Core, SQL Server enables automated and efficient data management, supporting features like transactions, data backup, and recovery to safeguard user data.
