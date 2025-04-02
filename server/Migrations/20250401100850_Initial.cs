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
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    { 1, "Zebra", new DateTime(2024, 6, 6, 14, 6, 19, 517, DateTimeKind.Local).AddTicks(59), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Generic Soft Car", 451.596219672537900m, 19, 14, new DateTime(2025, 3, 28, 2, 39, 22, 758, DateTimeKind.Local).AddTicks(1742) },
                    { 2, "Tombow", new DateTime(2024, 10, 23, 19, 13, 42, 629, DateTimeKind.Local).AddTicks(2169), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Incredible Metal Bike", 396.400895159891500m, 20, 71, new DateTime(2025, 3, 30, 9, 18, 34, 44, DateTimeKind.Local).AddTicks(4987) },
                    { 3, "Stabilo", new DateTime(2024, 11, 17, 2, 0, 9, 900, DateTimeKind.Local).AddTicks(821), "The Football Is Good For Training And Recreational Purposes", "Sleek Frozen Pants", 713.887338665028000m, 14, 18, new DateTime(2025, 3, 26, 13, 4, 6, 376, DateTimeKind.Local).AddTicks(5037) },
                    { 4, "Sakura", new DateTime(2025, 2, 22, 16, 49, 22, 938, DateTimeKind.Local).AddTicks(1859), "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Licensed Rubber Shoes", 473.48093813546500m, 15, 97, new DateTime(2025, 3, 30, 2, 2, 5, 721, DateTimeKind.Local).AddTicks(2879) },
                    { 5, "Zebra", new DateTime(2024, 12, 2, 6, 2, 46, 305, DateTimeKind.Local).AddTicks(105), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Awesome Granite Fish", 677.868806659363000m, 5, 0, new DateTime(2025, 3, 21, 3, 5, 50, 689, DateTimeKind.Local).AddTicks(2480) },
                    { 6, "Kokuyo", new DateTime(2024, 9, 13, 23, 11, 41, 30, DateTimeKind.Local).AddTicks(6443), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Small Steel Keyboard", 427.732963022223100m, 14, 42, new DateTime(2025, 3, 22, 10, 41, 58, 18, DateTimeKind.Local).AddTicks(7603) },
                    { 7, "Marvy", new DateTime(2024, 11, 25, 21, 15, 23, 167, DateTimeKind.Local).AddTicks(794), "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Handmade Granite Salad", 31.188464638099480m, 25, 60, new DateTime(2025, 3, 29, 20, 18, 27, 914, DateTimeKind.Local).AddTicks(7383) },
                    { 8, "Tombow", new DateTime(2024, 10, 24, 4, 26, 51, 895, DateTimeKind.Local).AddTicks(6561), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Unbranded Fresh Gloves", 479.270785582693800m, 14, 74, new DateTime(2025, 3, 21, 14, 33, 53, 513, DateTimeKind.Local).AddTicks(9297) },
                    { 9, "Zebra", new DateTime(2024, 5, 31, 10, 23, 8, 635, DateTimeKind.Local).AddTicks(6027), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Fantastic Rubber Bike", 406.9031865307050700m, 4, 40, new DateTime(2025, 3, 20, 7, 52, 9, 504, DateTimeKind.Local).AddTicks(788) },
                    { 10, "Crayola", new DateTime(2025, 2, 19, 18, 21, 30, 461, DateTimeKind.Local).AddTicks(7642), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Awesome Soft Tuna", 324.682047772196100m, 10, 32, new DateTime(2025, 3, 27, 18, 23, 40, 343, DateTimeKind.Local).AddTicks(5384) }
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
                    { 1, 6 },
                    { 1, 7 },
                    { 2, 8 },
                    { 3, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 4, 6 },
                    { 5, 2 },
                    { 5, 3 },
                    { 5, 10 },
                    { 6, 1 },
                    { 6, 6 },
                    { 6, 8 },
                    { 6, 9 },
                    { 6, 10 },
                    { 7, 8 },
                    { 8, 1 },
                    { 8, 10 },
                    { 9, 1 },
                    { 9, 2 },
                    { 9, 5 }
                });

            migrationBuilder.InsertData(
                table: "ColorProduct",
                columns: new[] { "ColorsId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 4 },
                    { 1, 7 },
                    { 2, 5 },
                    { 3, 6 },
                    { 3, 10 },
                    { 5, 4 },
                    { 5, 5 },
                    { 5, 7 },
                    { 5, 9 },
                    { 6, 1 },
                    { 6, 5 },
                    { 6, 6 },
                    { 7, 3 },
                    { 8, 3 },
                    { 9, 7 },
                    { 9, 8 },
                    { 10, 8 },
                    { 11, 4 },
                    { 11, 8 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ProductId", "Url" },
                values: new object[,]
                {
                    { 1, 1, "https://picsum.photos/500/500/?image=0" },
                    { 2, 1, "https://picsum.photos/500/500/?image=1000" },
                    { 3, 1, "https://picsum.photos/500/500/?image=126" },
                    { 4, 1, "https://picsum.photos/500/500/?image=60" },
                    { 5, 1, "https://picsum.photos/500/500/?image=559" },
                    { 6, 1, "https://picsum.photos/500/500/?image=225" },
                    { 7, 1, "https://picsum.photos/500/500/?image=171" },
                    { 8, 1, "https://picsum.photos/500/500/?image=27" },
                    { 9, 1, "https://picsum.photos/500/500/?image=239" },
                    { 10, 1, "https://picsum.photos/500/500/?image=292" },
                    { 11, 2, "https://picsum.photos/500/500/?image=417" },
                    { 12, 2, "https://picsum.photos/500/500/?image=188" },
                    { 13, 2, "https://picsum.photos/500/500/?image=315" },
                    { 14, 2, "https://picsum.photos/500/500/?image=247" },
                    { 15, 2, "https://picsum.photos/500/500/?image=858" },
                    { 16, 2, "https://picsum.photos/500/500/?image=104" },
                    { 17, 2, "https://picsum.photos/500/500/?image=284" },
                    { 18, 2, "https://picsum.photos/500/500/?image=463" },
                    { 19, 2, "https://picsum.photos/500/500/?image=1077" },
                    { 20, 2, "https://picsum.photos/500/500/?image=187" },
                    { 21, 3, "https://picsum.photos/500/500/?image=647" },
                    { 22, 3, "https://picsum.photos/500/500/?image=912" },
                    { 23, 3, "https://picsum.photos/500/500/?image=324" },
                    { 24, 3, "https://picsum.photos/500/500/?image=872" },
                    { 25, 3, "https://picsum.photos/500/500/?image=949" },
                    { 26, 3, "https://picsum.photos/500/500/?image=307" },
                    { 27, 3, "https://picsum.photos/500/500/?image=434" },
                    { 28, 3, "https://picsum.photos/500/500/?image=608" },
                    { 29, 3, "https://picsum.photos/500/500/?image=323" },
                    { 30, 3, "https://picsum.photos/500/500/?image=189" },
                    { 31, 4, "https://picsum.photos/500/500/?image=472" },
                    { 32, 4, "https://picsum.photos/500/500/?image=776" },
                    { 33, 4, "https://picsum.photos/500/500/?image=805" },
                    { 34, 4, "https://picsum.photos/500/500/?image=304" },
                    { 35, 4, "https://picsum.photos/500/500/?image=979" },
                    { 36, 4, "https://picsum.photos/500/500/?image=286" },
                    { 37, 4, "https://picsum.photos/500/500/?image=356" },
                    { 38, 4, "https://picsum.photos/500/500/?image=732" },
                    { 39, 4, "https://picsum.photos/500/500/?image=268" },
                    { 40, 4, "https://picsum.photos/500/500/?image=764" },
                    { 41, 5, "https://picsum.photos/500/500/?image=624" },
                    { 42, 5, "https://picsum.photos/500/500/?image=761" },
                    { 43, 5, "https://picsum.photos/500/500/?image=1019" },
                    { 44, 5, "https://picsum.photos/500/500/?image=655" },
                    { 45, 5, "https://picsum.photos/500/500/?image=303" },
                    { 46, 5, "https://picsum.photos/500/500/?image=54" },
                    { 47, 5, "https://picsum.photos/500/500/?image=843" },
                    { 48, 5, "https://picsum.photos/500/500/?image=674" },
                    { 49, 5, "https://picsum.photos/500/500/?image=679" },
                    { 50, 5, "https://picsum.photos/500/500/?image=771" },
                    { 51, 6, "https://picsum.photos/500/500/?image=857" },
                    { 52, 6, "https://picsum.photos/500/500/?image=339" },
                    { 53, 6, "https://picsum.photos/500/500/?image=613" },
                    { 54, 6, "https://picsum.photos/500/500/?image=254" },
                    { 55, 6, "https://picsum.photos/500/500/?image=439" },
                    { 56, 6, "https://picsum.photos/500/500/?image=171" },
                    { 57, 6, "https://picsum.photos/500/500/?image=15" },
                    { 58, 6, "https://picsum.photos/500/500/?image=485" },
                    { 59, 6, "https://picsum.photos/500/500/?image=572" },
                    { 60, 6, "https://picsum.photos/500/500/?image=982" },
                    { 61, 7, "https://picsum.photos/500/500/?image=1077" },
                    { 62, 7, "https://picsum.photos/500/500/?image=10" },
                    { 63, 7, "https://picsum.photos/500/500/?image=1061" },
                    { 64, 7, "https://picsum.photos/500/500/?image=437" },
                    { 65, 7, "https://picsum.photos/500/500/?image=2" },
                    { 66, 7, "https://picsum.photos/500/500/?image=87" },
                    { 67, 7, "https://picsum.photos/500/500/?image=184" },
                    { 68, 7, "https://picsum.photos/500/500/?image=462" },
                    { 69, 7, "https://picsum.photos/500/500/?image=285" },
                    { 70, 7, "https://picsum.photos/500/500/?image=581" },
                    { 71, 8, "https://picsum.photos/500/500/?image=992" },
                    { 72, 8, "https://picsum.photos/500/500/?image=176" },
                    { 73, 8, "https://picsum.photos/500/500/?image=459" },
                    { 74, 8, "https://picsum.photos/500/500/?image=512" },
                    { 75, 8, "https://picsum.photos/500/500/?image=16" },
                    { 76, 8, "https://picsum.photos/500/500/?image=168" },
                    { 77, 8, "https://picsum.photos/500/500/?image=305" },
                    { 78, 8, "https://picsum.photos/500/500/?image=453" },
                    { 79, 8, "https://picsum.photos/500/500/?image=1033" },
                    { 80, 8, "https://picsum.photos/500/500/?image=417" },
                    { 81, 9, "https://picsum.photos/500/500/?image=508" },
                    { 82, 9, "https://picsum.photos/500/500/?image=782" },
                    { 83, 9, "https://picsum.photos/500/500/?image=634" },
                    { 84, 9, "https://picsum.photos/500/500/?image=931" },
                    { 85, 9, "https://picsum.photos/500/500/?image=1036" },
                    { 86, 9, "https://picsum.photos/500/500/?image=355" },
                    { 87, 9, "https://picsum.photos/500/500/?image=868" },
                    { 88, 9, "https://picsum.photos/500/500/?image=956" },
                    { 89, 9, "https://picsum.photos/500/500/?image=399" },
                    { 90, 9, "https://picsum.photos/500/500/?image=361" },
                    { 91, 10, "https://picsum.photos/500/500/?image=628" },
                    { 92, 10, "https://picsum.photos/500/500/?image=861" },
                    { 93, 10, "https://picsum.photos/500/500/?image=410" },
                    { 94, 10, "https://picsum.photos/500/500/?image=417" },
                    { 95, 10, "https://picsum.photos/500/500/?image=668" },
                    { 96, 10, "https://picsum.photos/500/500/?image=438" },
                    { 97, 10, "https://picsum.photos/500/500/?image=875" },
                    { 98, 10, "https://picsum.photos/500/500/?image=160" },
                    { 99, 10, "https://picsum.photos/500/500/?image=853" },
                    { 100, 10, "https://picsum.photos/500/500/?image=601" }
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
