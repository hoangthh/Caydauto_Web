using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Sold = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryProduct",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProduct", x => new { x.CategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColorProduct",
                columns: table => new
                {
                    ColorsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorProduct", x => new { x.ColorsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ColorProduct_Colors_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPriceAfterDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    DiscountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProductUser",
                columns: table => new
                {
                    WishListId = table.Column<int>(type: "int", nullable: false),
                    WishListedByUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUser", x => new { x.WishListId, x.WishListedByUsersId });
                    table.ForeignKey(
                        name: "FK_ProductUser_Products_WishListId",
                        column: x => x.WishListId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUser_Users_WishListedByUsersId",
                        column: x => x.WishListedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentImages_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Danh mục Hộp bút - Túi bút chứa các sản phẩm liên quan.", "Hộp bút - Túi bút" },
                    { 2, "Danh mục Vở Học Sinh Campus chứa các sản phẩm liên quan.", "Vở Học Sinh Campus" },
                    { 3, "Danh mục Bút gel chứa các sản phẩm liên quan.", "Bút gel" },
                    { 4, "Danh mục Bút chì chứa các sản phẩm liên quan.", "Bút chì" },
                    { 5, "Danh mục Bút ghi nhớ chứa các sản phẩm liên quan.", "Bút ghi nhớ" },
                    { 6, "Danh mục Dụng cụ tẩy chứa các sản phẩm liên quan.", "Dụng cụ tẩy" },
                    { 7, "Danh mục Bút brush chứa các sản phẩm liên quan.", "Bút brush" },
                    { 8, "Danh mục Băng keo chứa các sản phẩm liên quan.", "Băng keo" },
                    { 9, "Danh mục Dụng cụ hỗ trợ chứa các sản phẩm liên quan.", "Dụng cụ hỗ trợ" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Black" },
                    { 2, "White" },
                    { 3, "Gray" },
                    { 4, "Orange" },
                    { 5, "Yellow" },
                    { 6, "Green" },
                    { 7, "Blue" },
                    { 8, "Purple" },
                    { 9, "Pink" },
                    { 10, "Red" },
                    { 11, "Brown" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CreatedDate", "Description", "Name", "Price", "Sold", "StockQuantity", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Marunman", new DateTime(2024, 10, 9, 15, 15, 28, 244, DateTimeKind.Local).AddTicks(5057), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Handcrafted Steel Tuna", 74027, 5, 86, new DateTime(2025, 3, 27, 18, 38, 39, 776, DateTimeKind.Local).AddTicks(9619) },
                    { 2, "Sakura", new DateTime(2025, 2, 21, 18, 28, 11, 192, DateTimeKind.Local).AddTicks(5941), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Fantastic Granite Mouse", 413582, 16, 68, new DateTime(2025, 3, 24, 3, 20, 28, 976, DateTimeKind.Local).AddTicks(1937) },
                    { 3, "Plus", new DateTime(2024, 9, 23, 15, 3, 40, 636, DateTimeKind.Local).AddTicks(7981), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Intelligent Soft Chips", 28021, 29, 14, new DateTime(2025, 3, 22, 1, 26, 20, 324, DateTimeKind.Local).AddTicks(6893) },
                    { 4, "Marunman", new DateTime(2025, 3, 2, 22, 54, 24, 689, DateTimeKind.Local).AddTicks(3541), "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Intelligent Plastic Fish", 208178, 16, 11, new DateTime(2025, 3, 21, 5, 20, 5, 36, DateTimeKind.Local).AddTicks(8721) },
                    { 5, "Marvy", new DateTime(2025, 2, 28, 11, 11, 48, 489, DateTimeKind.Local).AddTicks(8328), "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Unbranded Granite Cheese", 533196, 3, 30, new DateTime(2025, 4, 3, 14, 4, 6, 776, DateTimeKind.Local).AddTicks(6817) },
                    { 6, "Pilot", new DateTime(2024, 6, 27, 1, 16, 50, 689, DateTimeKind.Local).AddTicks(6606), "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Intelligent Soft Keyboard", 178682, 17, 82, new DateTime(2025, 3, 8, 14, 37, 25, 707, DateTimeKind.Local).AddTicks(9371) },
                    { 7, "Crayola", new DateTime(2024, 11, 4, 6, 49, 48, 706, DateTimeKind.Local).AddTicks(5921), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Licensed Rubber Shirt", 26544, 8, 35, new DateTime(2025, 4, 2, 1, 16, 57, 359, DateTimeKind.Local).AddTicks(7559) },
                    { 8, "Sakura", new DateTime(2025, 3, 17, 8, 17, 50, 331, DateTimeKind.Local).AddTicks(5301), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Generic Fresh Pants", 290349, 21, 48, new DateTime(2025, 3, 31, 12, 13, 19, 968, DateTimeKind.Local).AddTicks(1456) },
                    { 9, "Marunman", new DateTime(2024, 11, 11, 0, 27, 35, 51, DateTimeKind.Local).AddTicks(2668), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Sleek Wooden Soap", 251146, 30, 6, new DateTime(2025, 3, 16, 16, 53, 19, 515, DateTimeKind.Local).AddTicks(9500) },
                    { 10, "Stabilo", new DateTime(2024, 9, 28, 9, 37, 35, 669, DateTimeKind.Local).AddTicks(7031), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Gorgeous Steel Tuna", 229721, 32, 42, new DateTime(2025, 3, 24, 2, 53, 52, 450, DateTimeKind.Local).AddTicks(2326) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Administrator role", "Admin", "ADMIN" },
                    { 2, null, "User role", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "CategoryProduct",
                columns: new[] { "CategoriesId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 1, 8 },
                    { 1, 10 },
                    { 2, 1 },
                    { 2, 8 },
                    { 2, 10 },
                    { 3, 2 },
                    { 4, 1 },
                    { 4, 3 },
                    { 4, 9 },
                    { 5, 2 },
                    { 5, 5 },
                    { 5, 7 },
                    { 6, 9 },
                    { 7, 8 },
                    { 7, 10 },
                    { 8, 3 },
                    { 8, 4 },
                    { 8, 5 },
                    { 8, 6 },
                    { 9, 1 },
                    { 9, 6 }
                });

            migrationBuilder.InsertData(
                table: "ColorProduct",
                columns: new[] { "ColorsId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 10 },
                    { 2, 5 },
                    { 3, 2 },
                    { 3, 6 },
                    { 3, 8 },
                    { 3, 9 },
                    { 4, 1 },
                    { 4, 2 },
                    { 5, 5 },
                    { 6, 4 },
                    { 7, 1 },
                    { 9, 3 },
                    { 9, 8 },
                    { 9, 10 },
                    { 10, 1 },
                    { 10, 7 },
                    { 10, 9 },
                    { 11, 9 },
                    { 11, 10 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ProductId", "Url" },
                values: new object[,]
                {
                    { 1, 1, "https://picsum.photos/500/500/?image=542" },
                    { 2, 1, "https://picsum.photos/500/500/?image=526" },
                    { 3, 1, "https://picsum.photos/500/500/?image=60" },
                    { 4, 1, "https://picsum.photos/500/500/?image=284" },
                    { 5, 1, "https://picsum.photos/500/500/?image=311" },
                    { 6, 1, "https://picsum.photos/500/500/?image=261" },
                    { 7, 1, "https://picsum.photos/500/500/?image=866" },
                    { 8, 1, "https://picsum.photos/500/500/?image=758" },
                    { 9, 1, "https://picsum.photos/500/500/?image=904" },
                    { 10, 1, "https://picsum.photos/500/500/?image=1050" },
                    { 11, 2, "https://picsum.photos/500/500/?image=653" },
                    { 12, 2, "https://picsum.photos/500/500/?image=512" },
                    { 13, 2, "https://picsum.photos/500/500/?image=680" },
                    { 14, 2, "https://picsum.photos/500/500/?image=255" },
                    { 15, 2, "https://picsum.photos/500/500/?image=839" },
                    { 16, 2, "https://picsum.photos/500/500/?image=193" },
                    { 17, 2, "https://picsum.photos/500/500/?image=444" },
                    { 18, 2, "https://picsum.photos/500/500/?image=682" },
                    { 19, 2, "https://picsum.photos/500/500/?image=157" },
                    { 20, 2, "https://picsum.photos/500/500/?image=1013" },
                    { 21, 3, "https://picsum.photos/500/500/?image=793" },
                    { 22, 3, "https://picsum.photos/500/500/?image=132" },
                    { 23, 3, "https://picsum.photos/500/500/?image=1035" },
                    { 24, 3, "https://picsum.photos/500/500/?image=1000" },
                    { 25, 3, "https://picsum.photos/500/500/?image=594" },
                    { 26, 3, "https://picsum.photos/500/500/?image=420" },
                    { 27, 3, "https://picsum.photos/500/500/?image=207" },
                    { 28, 3, "https://picsum.photos/500/500/?image=401" },
                    { 29, 3, "https://picsum.photos/500/500/?image=41" },
                    { 30, 3, "https://picsum.photos/500/500/?image=37" },
                    { 31, 4, "https://picsum.photos/500/500/?image=129" },
                    { 32, 4, "https://picsum.photos/500/500/?image=931" },
                    { 33, 4, "https://picsum.photos/500/500/?image=322" },
                    { 34, 4, "https://picsum.photos/500/500/?image=699" },
                    { 35, 4, "https://picsum.photos/500/500/?image=674" },
                    { 36, 4, "https://picsum.photos/500/500/?image=738" },
                    { 37, 4, "https://picsum.photos/500/500/?image=243" },
                    { 38, 4, "https://picsum.photos/500/500/?image=943" },
                    { 39, 4, "https://picsum.photos/500/500/?image=616" },
                    { 40, 4, "https://picsum.photos/500/500/?image=689" },
                    { 41, 5, "https://picsum.photos/500/500/?image=339" },
                    { 42, 5, "https://picsum.photos/500/500/?image=61" },
                    { 43, 5, "https://picsum.photos/500/500/?image=393" },
                    { 44, 5, "https://picsum.photos/500/500/?image=396" },
                    { 45, 5, "https://picsum.photos/500/500/?image=181" },
                    { 46, 5, "https://picsum.photos/500/500/?image=938" },
                    { 47, 5, "https://picsum.photos/500/500/?image=189" },
                    { 48, 5, "https://picsum.photos/500/500/?image=753" },
                    { 49, 5, "https://picsum.photos/500/500/?image=652" },
                    { 50, 5, "https://picsum.photos/500/500/?image=100" },
                    { 51, 6, "https://picsum.photos/500/500/?image=608" },
                    { 52, 6, "https://picsum.photos/500/500/?image=45" },
                    { 53, 6, "https://picsum.photos/500/500/?image=467" },
                    { 54, 6, "https://picsum.photos/500/500/?image=58" },
                    { 55, 6, "https://picsum.photos/500/500/?image=407" },
                    { 56, 6, "https://picsum.photos/500/500/?image=1035" },
                    { 57, 6, "https://picsum.photos/500/500/?image=125" },
                    { 58, 6, "https://picsum.photos/500/500/?image=949" },
                    { 59, 6, "https://picsum.photos/500/500/?image=58" },
                    { 60, 6, "https://picsum.photos/500/500/?image=977" },
                    { 61, 7, "https://picsum.photos/500/500/?image=691" },
                    { 62, 7, "https://picsum.photos/500/500/?image=13" },
                    { 63, 7, "https://picsum.photos/500/500/?image=575" },
                    { 64, 7, "https://picsum.photos/500/500/?image=258" },
                    { 65, 7, "https://picsum.photos/500/500/?image=988" },
                    { 66, 7, "https://picsum.photos/500/500/?image=163" },
                    { 67, 7, "https://picsum.photos/500/500/?image=499" },
                    { 68, 7, "https://picsum.photos/500/500/?image=689" },
                    { 69, 7, "https://picsum.photos/500/500/?image=191" },
                    { 70, 7, "https://picsum.photos/500/500/?image=460" },
                    { 71, 8, "https://picsum.photos/500/500/?image=393" },
                    { 72, 8, "https://picsum.photos/500/500/?image=88" },
                    { 73, 8, "https://picsum.photos/500/500/?image=840" },
                    { 74, 8, "https://picsum.photos/500/500/?image=411" },
                    { 75, 8, "https://picsum.photos/500/500/?image=609" },
                    { 76, 8, "https://picsum.photos/500/500/?image=939" },
                    { 77, 8, "https://picsum.photos/500/500/?image=549" },
                    { 78, 8, "https://picsum.photos/500/500/?image=1009" },
                    { 79, 8, "https://picsum.photos/500/500/?image=702" },
                    { 80, 8, "https://picsum.photos/500/500/?image=967" },
                    { 81, 9, "https://picsum.photos/500/500/?image=928" },
                    { 82, 9, "https://picsum.photos/500/500/?image=840" },
                    { 83, 9, "https://picsum.photos/500/500/?image=133" },
                    { 84, 9, "https://picsum.photos/500/500/?image=1028" },
                    { 85, 9, "https://picsum.photos/500/500/?image=149" },
                    { 86, 9, "https://picsum.photos/500/500/?image=651" },
                    { 87, 9, "https://picsum.photos/500/500/?image=667" },
                    { 88, 9, "https://picsum.photos/500/500/?image=211" },
                    { 89, 9, "https://picsum.photos/500/500/?image=521" },
                    { 90, 9, "https://picsum.photos/500/500/?image=814" },
                    { 91, 10, "https://picsum.photos/500/500/?image=957" },
                    { 92, 10, "https://picsum.photos/500/500/?image=813" },
                    { 93, 10, "https://picsum.photos/500/500/?image=833" },
                    { 94, 10, "https://picsum.photos/500/500/?image=982" },
                    { 95, 10, "https://picsum.photos/500/500/?image=20" },
                    { 96, 10, "https://picsum.photos/500/500/?image=597" },
                    { 97, 10, "https://picsum.photos/500/500/?image=1045" },
                    { 98, 10, "https://picsum.photos/500/500/?image=670" },
                    { 99, 10, "https://picsum.photos/500/500/?image=626" },
                    { 100, 10, "https://picsum.photos/500/500/?image=461" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ColorId",
                table: "CartItems",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_ProductsId",
                table: "CategoryProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ColorProduct_ProductsId",
                table: "ColorProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentImages_CommentId",
                table: "CommentImages",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ColorId",
                table: "OrderItems",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DiscountId",
                table: "Orders",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUser_WishListedByUsersId",
                table: "ProductUser",
                column: "WishListedByUsersId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CategoryProduct");

            migrationBuilder.DropTable(
                name: "ColorProduct");

            migrationBuilder.DropTable(
                name: "CommentImages");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductUser");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
