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
                name: "AspNetRoles",
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
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
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
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id");
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
                        name: "FK_ProductUser_AspNetUsers_WishListedByUsersId",
                        column: x => x.WishListedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUser_Products_WishListId",
                        column: x => x.WishListId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Administrator role", "Admin", "ADMIN" },
                    { 2, null, "User role", "User", "USER" }
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
                    { 1, "Pilot", new DateTime(2025, 3, 1, 15, 38, 13, 936, DateTimeKind.Local).AddTicks(3058), "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Generic Wooden Fish", 677.879170262702500m, 19, 75, new DateTime(2025, 3, 29, 0, 22, 57, 241, DateTimeKind.Local).AddTicks(9467) },
                    { 2, "Tombow", new DateTime(2025, 2, 1, 17, 38, 27, 1, DateTimeKind.Local).AddTicks(7888), "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Unbranded Fresh Shoes", 419.858610433636600m, 15, 47, new DateTime(2025, 3, 8, 8, 16, 42, 294, DateTimeKind.Local).AddTicks(7262) },
                    { 3, "Stabilo", new DateTime(2024, 6, 12, 7, 44, 23, 611, DateTimeKind.Local).AddTicks(8195), "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Fantastic Concrete Keyboard", 194.818832709100m, 39, 24, new DateTime(2025, 3, 25, 11, 35, 25, 905, DateTimeKind.Local).AddTicks(1926) },
                    { 4, "Marunman", new DateTime(2025, 1, 26, 4, 45, 42, 457, DateTimeKind.Local).AddTicks(6557), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Handmade Concrete Chips", 37.587966361759450m, 37, 2, new DateTime(2025, 3, 20, 7, 26, 10, 75, DateTimeKind.Local).AddTicks(8692) },
                    { 5, "Crayola", new DateTime(2024, 5, 13, 18, 55, 29, 210, DateTimeKind.Local).AddTicks(2517), "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Practical Metal Tuna", 430.148605992254800m, 8, 20, new DateTime(2025, 3, 1, 21, 5, 48, 583, DateTimeKind.Local).AddTicks(6344) },
                    { 6, "Zebra", new DateTime(2024, 6, 21, 3, 44, 8, 291, DateTimeKind.Local).AddTicks(9787), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Unbranded Plastic Sausages", 14.4329001851827360m, 22, 37, new DateTime(2025, 3, 1, 4, 39, 13, 952, DateTimeKind.Local).AddTicks(1829) },
                    { 7, "Stabilo", new DateTime(2025, 1, 17, 8, 20, 29, 279, DateTimeKind.Local).AddTicks(4350), "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Generic Plastic Chips", 290.272139333792800m, 22, 78, new DateTime(2025, 3, 1, 20, 45, 50, 578, DateTimeKind.Local).AddTicks(9830) },
                    { 8, "Plus", new DateTime(2024, 6, 30, 17, 41, 13, 245, DateTimeKind.Local).AddTicks(4500), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Tasty Frozen Pants", 120.986151073772600m, 50, 30, new DateTime(2025, 3, 25, 13, 36, 1, 785, DateTimeKind.Local).AddTicks(3909) },
                    { 9, "Zebra", new DateTime(2025, 1, 30, 19, 50, 0, 260, DateTimeKind.Local).AddTicks(4483), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Tasty Wooden Sausages", 171.438693553211100m, 31, 29, new DateTime(2025, 3, 14, 17, 26, 50, 153, DateTimeKind.Local).AddTicks(5150) },
                    { 10, "Kokuyo", new DateTime(2024, 10, 15, 1, 59, 2, 871, DateTimeKind.Local).AddTicks(3632), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Awesome Fresh Shirt", 273.266264993680700m, 16, 46, new DateTime(2025, 3, 10, 13, 40, 26, 613, DateTimeKind.Local).AddTicks(3770) }
                });

            migrationBuilder.InsertData(
                table: "CategoryProduct",
                columns: new[] { "CategoriesId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 1, 10 },
                    { 2, 1 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 8 },
                    { 3, 2 },
                    { 3, 7 },
                    { 4, 1 },
                    { 4, 3 },
                    { 4, 4 },
                    { 4, 5 },
                    { 5, 2 },
                    { 5, 6 },
                    { 5, 7 },
                    { 6, 8 },
                    { 7, 6 },
                    { 8, 9 },
                    { 8, 10 },
                    { 9, 1 },
                    { 9, 4 },
                    { 9, 7 }
                });

            migrationBuilder.InsertData(
                table: "ColorProduct",
                columns: new[] { "ColorsId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 6 },
                    { 2, 7 },
                    { 3, 10 },
                    { 4, 2 },
                    { 4, 3 },
                    { 4, 4 },
                    { 4, 5 },
                    { 5, 4 },
                    { 5, 8 },
                    { 7, 1 },
                    { 8, 8 },
                    { 9, 8 },
                    { 9, 9 },
                    { 9, 10 },
                    { 10, 1 },
                    { 10, 5 },
                    { 10, 7 },
                    { 11, 10 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ProductId", "Url" },
                values: new object[,]
                {
                    { 1, 1, "https://picsum.photos/500/500/?image=942" },
                    { 2, 1, "https://picsum.photos/500/500/?image=539" },
                    { 3, 1, "https://picsum.photos/500/500/?image=845" },
                    { 4, 1, "https://picsum.photos/500/500/?image=708" },
                    { 5, 1, "https://picsum.photos/500/500/?image=220" },
                    { 6, 1, "https://picsum.photos/500/500/?image=935" },
                    { 7, 1, "https://picsum.photos/500/500/?image=359" },
                    { 8, 1, "https://picsum.photos/500/500/?image=27" },
                    { 9, 1, "https://picsum.photos/500/500/?image=937" },
                    { 10, 1, "https://picsum.photos/500/500/?image=666" },
                    { 11, 2, "https://picsum.photos/500/500/?image=399" },
                    { 12, 2, "https://picsum.photos/500/500/?image=837" },
                    { 13, 2, "https://picsum.photos/500/500/?image=429" },
                    { 14, 2, "https://picsum.photos/500/500/?image=108" },
                    { 15, 2, "https://picsum.photos/500/500/?image=674" },
                    { 16, 2, "https://picsum.photos/500/500/?image=303" },
                    { 17, 2, "https://picsum.photos/500/500/?image=965" },
                    { 18, 2, "https://picsum.photos/500/500/?image=230" },
                    { 19, 2, "https://picsum.photos/500/500/?image=129" },
                    { 20, 2, "https://picsum.photos/500/500/?image=382" },
                    { 21, 3, "https://picsum.photos/500/500/?image=686" },
                    { 22, 3, "https://picsum.photos/500/500/?image=175" },
                    { 23, 3, "https://picsum.photos/500/500/?image=895" },
                    { 24, 3, "https://picsum.photos/500/500/?image=42" },
                    { 25, 3, "https://picsum.photos/500/500/?image=390" },
                    { 26, 3, "https://picsum.photos/500/500/?image=432" },
                    { 27, 3, "https://picsum.photos/500/500/?image=1016" },
                    { 28, 3, "https://picsum.photos/500/500/?image=362" },
                    { 29, 3, "https://picsum.photos/500/500/?image=706" },
                    { 30, 3, "https://picsum.photos/500/500/?image=493" },
                    { 31, 4, "https://picsum.photos/500/500/?image=485" },
                    { 32, 4, "https://picsum.photos/500/500/?image=916" },
                    { 33, 4, "https://picsum.photos/500/500/?image=969" },
                    { 34, 4, "https://picsum.photos/500/500/?image=370" },
                    { 35, 4, "https://picsum.photos/500/500/?image=201" },
                    { 36, 4, "https://picsum.photos/500/500/?image=96" },
                    { 37, 4, "https://picsum.photos/500/500/?image=40" },
                    { 38, 4, "https://picsum.photos/500/500/?image=447" },
                    { 39, 4, "https://picsum.photos/500/500/?image=311" },
                    { 40, 4, "https://picsum.photos/500/500/?image=122" },
                    { 41, 5, "https://picsum.photos/500/500/?image=1062" },
                    { 42, 5, "https://picsum.photos/500/500/?image=792" },
                    { 43, 5, "https://picsum.photos/500/500/?image=702" },
                    { 44, 5, "https://picsum.photos/500/500/?image=915" },
                    { 45, 5, "https://picsum.photos/500/500/?image=410" },
                    { 46, 5, "https://picsum.photos/500/500/?image=1060" },
                    { 47, 5, "https://picsum.photos/500/500/?image=27" },
                    { 48, 5, "https://picsum.photos/500/500/?image=97" },
                    { 49, 5, "https://picsum.photos/500/500/?image=453" },
                    { 50, 5, "https://picsum.photos/500/500/?image=915" },
                    { 51, 6, "https://picsum.photos/500/500/?image=904" },
                    { 52, 6, "https://picsum.photos/500/500/?image=514" },
                    { 53, 6, "https://picsum.photos/500/500/?image=579" },
                    { 54, 6, "https://picsum.photos/500/500/?image=494" },
                    { 55, 6, "https://picsum.photos/500/500/?image=761" },
                    { 56, 6, "https://picsum.photos/500/500/?image=266" },
                    { 57, 6, "https://picsum.photos/500/500/?image=253" },
                    { 58, 6, "https://picsum.photos/500/500/?image=288" },
                    { 59, 6, "https://picsum.photos/500/500/?image=880" },
                    { 60, 6, "https://picsum.photos/500/500/?image=599" },
                    { 61, 7, "https://picsum.photos/500/500/?image=623" },
                    { 62, 7, "https://picsum.photos/500/500/?image=861" },
                    { 63, 7, "https://picsum.photos/500/500/?image=524" },
                    { 64, 7, "https://picsum.photos/500/500/?image=368" },
                    { 65, 7, "https://picsum.photos/500/500/?image=1061" },
                    { 66, 7, "https://picsum.photos/500/500/?image=557" },
                    { 67, 7, "https://picsum.photos/500/500/?image=451" },
                    { 68, 7, "https://picsum.photos/500/500/?image=323" },
                    { 69, 7, "https://picsum.photos/500/500/?image=387" },
                    { 70, 7, "https://picsum.photos/500/500/?image=502" },
                    { 71, 8, "https://picsum.photos/500/500/?image=371" },
                    { 72, 8, "https://picsum.photos/500/500/?image=743" },
                    { 73, 8, "https://picsum.photos/500/500/?image=1004" },
                    { 74, 8, "https://picsum.photos/500/500/?image=671" },
                    { 75, 8, "https://picsum.photos/500/500/?image=479" },
                    { 76, 8, "https://picsum.photos/500/500/?image=139" },
                    { 77, 8, "https://picsum.photos/500/500/?image=522" },
                    { 78, 8, "https://picsum.photos/500/500/?image=547" },
                    { 79, 8, "https://picsum.photos/500/500/?image=710" },
                    { 80, 8, "https://picsum.photos/500/500/?image=112" },
                    { 81, 9, "https://picsum.photos/500/500/?image=807" },
                    { 82, 9, "https://picsum.photos/500/500/?image=366" },
                    { 83, 9, "https://picsum.photos/500/500/?image=66" },
                    { 84, 9, "https://picsum.photos/500/500/?image=392" },
                    { 85, 9, "https://picsum.photos/500/500/?image=178" },
                    { 86, 9, "https://picsum.photos/500/500/?image=787" },
                    { 87, 9, "https://picsum.photos/500/500/?image=601" },
                    { 88, 9, "https://picsum.photos/500/500/?image=887" },
                    { 89, 9, "https://picsum.photos/500/500/?image=848" },
                    { 90, 9, "https://picsum.photos/500/500/?image=931" },
                    { 91, 10, "https://picsum.photos/500/500/?image=128" },
                    { 92, 10, "https://picsum.photos/500/500/?image=765" },
                    { 93, 10, "https://picsum.photos/500/500/?image=582" },
                    { 94, 10, "https://picsum.photos/500/500/?image=1049" },
                    { 95, 10, "https://picsum.photos/500/500/?image=330" },
                    { 96, 10, "https://picsum.photos/500/500/?image=661" },
                    { 97, 10, "https://picsum.photos/500/500/?image=353" },
                    { 98, 10, "https://picsum.photos/500/500/?image=1077" },
                    { 99, 10, "https://picsum.photos/500/500/?image=38" },
                    { 100, 10, "https://picsum.photos/500/500/?image=882" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
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
                name: "UserRoles");

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
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
