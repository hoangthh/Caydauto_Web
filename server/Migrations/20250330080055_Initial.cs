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
                    { 1, "Marvy", new DateTime(2024, 12, 12, 14, 33, 33, 663, DateTimeKind.Local).AddTicks(6074), "The Football Is Good For Training And Recreational Purposes", "Rustic Soft Soap", 723.504384713329500m, 7, 19, new DateTime(2025, 3, 29, 10, 44, 57, 37, DateTimeKind.Local).AddTicks(8819) },
                    { 2, "Pilot", new DateTime(2024, 5, 26, 4, 33, 50, 544, DateTimeKind.Local).AddTicks(6862), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Awesome Cotton Shirt", 276.013261607743900m, 4, 70, new DateTime(2025, 3, 5, 3, 20, 21, 653, DateTimeKind.Local).AddTicks(9822) },
                    { 3, "Marunman", new DateTime(2024, 8, 3, 7, 40, 49, 753, DateTimeKind.Local).AddTicks(1261), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Ergonomic Granite Table", 128.644830928727600m, 21, 2, new DateTime(2025, 3, 18, 3, 52, 50, 58, DateTimeKind.Local).AddTicks(1158) },
                    { 4, "Tombow", new DateTime(2024, 9, 21, 20, 24, 33, 657, DateTimeKind.Local).AddTicks(2693), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Rustic Metal Soap", 452.952143492489800m, 42, 20, new DateTime(2025, 3, 3, 18, 32, 44, 103, DateTimeKind.Local).AddTicks(4926) },
                    { 5, "Crayola", new DateTime(2024, 8, 18, 17, 59, 56, 856, DateTimeKind.Local).AddTicks(9039), "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Refined Soft Chair", 24.176629853292340m, 10, 69, new DateTime(2025, 3, 12, 20, 8, 55, 529, DateTimeKind.Local).AddTicks(2929) },
                    { 6, "Marunman", new DateTime(2024, 7, 20, 17, 25, 26, 93, DateTimeKind.Local).AddTicks(1771), "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Gorgeous Metal Chips", 48.154511512219420m, 43, 46, new DateTime(2025, 3, 17, 4, 9, 12, 144, DateTimeKind.Local).AddTicks(6502) },
                    { 7, "Sakura", new DateTime(2024, 9, 26, 5, 57, 48, 192, DateTimeKind.Local).AddTicks(7638), "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Tasty Concrete Soap", 100.9885504201169400m, 29, 9, new DateTime(2025, 3, 12, 15, 18, 9, 371, DateTimeKind.Local).AddTicks(102) },
                    { 8, "Plus", new DateTime(2024, 10, 7, 20, 47, 40, 841, DateTimeKind.Local).AddTicks(7433), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Licensed Frozen Shoes", 778.725843341654000m, 31, 88, new DateTime(2025, 3, 18, 2, 53, 26, 952, DateTimeKind.Local).AddTicks(4723) },
                    { 9, "Plus", new DateTime(2025, 1, 16, 10, 46, 31, 444, DateTimeKind.Local).AddTicks(3095), "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Handcrafted Steel Pants", 652.895354914541500m, 24, 47, new DateTime(2025, 3, 27, 8, 5, 30, 909, DateTimeKind.Local).AddTicks(6486) },
                    { 10, "Tombow", new DateTime(2024, 5, 22, 14, 18, 1, 26, DateTimeKind.Local).AddTicks(6156), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Handmade Cotton Salad", 331.119815747339400m, 17, 69, new DateTime(2025, 3, 29, 20, 11, 53, 598, DateTimeKind.Local).AddTicks(5749) }
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
                    { 2, 1 },
                    { 2, 6 },
                    { 2, 7 },
                    { 2, 8 },
                    { 3, 2 },
                    { 3, 10 },
                    { 4, 1 },
                    { 4, 4 },
                    { 4, 9 },
                    { 5, 6 },
                    { 6, 3 },
                    { 6, 6 },
                    { 7, 5 },
                    { 7, 8 },
                    { 7, 10 },
                    { 8, 4 },
                    { 9, 5 },
                    { 9, 10 }
                });

            migrationBuilder.InsertData(
                table: "ColorProduct",
                columns: new[] { "ColorsId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 2, 7 },
                    { 3, 3 },
                    { 3, 5 },
                    { 3, 7 },
                    { 3, 10 },
                    { 5, 2 },
                    { 5, 5 },
                    { 6, 8 },
                    { 7, 1 },
                    { 7, 7 },
                    { 8, 4 },
                    { 8, 10 },
                    { 9, 4 },
                    { 9, 6 },
                    { 9, 9 },
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 },
                    { 10, 9 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 6 },
                    { 11, 8 },
                    { 11, 10 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ProductId", "Url" },
                values: new object[,]
                {
                    { 1, 1, "https://picsum.photos/500/500/?image=203" },
                    { 2, 1, "https://picsum.photos/500/500/?image=224" },
                    { 3, 1, "https://picsum.photos/500/500/?image=355" },
                    { 4, 1, "https://picsum.photos/500/500/?image=536" },
                    { 5, 1, "https://picsum.photos/500/500/?image=971" },
                    { 6, 1, "https://picsum.photos/500/500/?image=769" },
                    { 7, 1, "https://picsum.photos/500/500/?image=928" },
                    { 8, 1, "https://picsum.photos/500/500/?image=770" },
                    { 9, 1, "https://picsum.photos/500/500/?image=202" },
                    { 10, 1, "https://picsum.photos/500/500/?image=1059" },
                    { 11, 2, "https://picsum.photos/500/500/?image=703" },
                    { 12, 2, "https://picsum.photos/500/500/?image=874" },
                    { 13, 2, "https://picsum.photos/500/500/?image=500" },
                    { 14, 2, "https://picsum.photos/500/500/?image=1078" },
                    { 15, 2, "https://picsum.photos/500/500/?image=1055" },
                    { 16, 2, "https://picsum.photos/500/500/?image=1015" },
                    { 17, 2, "https://picsum.photos/500/500/?image=708" },
                    { 18, 2, "https://picsum.photos/500/500/?image=31" },
                    { 19, 2, "https://picsum.photos/500/500/?image=875" },
                    { 20, 2, "https://picsum.photos/500/500/?image=148" },
                    { 21, 3, "https://picsum.photos/500/500/?image=305" },
                    { 22, 3, "https://picsum.photos/500/500/?image=632" },
                    { 23, 3, "https://picsum.photos/500/500/?image=723" },
                    { 24, 3, "https://picsum.photos/500/500/?image=30" },
                    { 25, 3, "https://picsum.photos/500/500/?image=282" },
                    { 26, 3, "https://picsum.photos/500/500/?image=745" },
                    { 27, 3, "https://picsum.photos/500/500/?image=45" },
                    { 28, 3, "https://picsum.photos/500/500/?image=312" },
                    { 29, 3, "https://picsum.photos/500/500/?image=713" },
                    { 30, 3, "https://picsum.photos/500/500/?image=1049" },
                    { 31, 4, "https://picsum.photos/500/500/?image=986" },
                    { 32, 4, "https://picsum.photos/500/500/?image=384" },
                    { 33, 4, "https://picsum.photos/500/500/?image=440" },
                    { 34, 4, "https://picsum.photos/500/500/?image=364" },
                    { 35, 4, "https://picsum.photos/500/500/?image=308" },
                    { 36, 4, "https://picsum.photos/500/500/?image=216" },
                    { 37, 4, "https://picsum.photos/500/500/?image=335" },
                    { 38, 4, "https://picsum.photos/500/500/?image=155" },
                    { 39, 4, "https://picsum.photos/500/500/?image=574" },
                    { 40, 4, "https://picsum.photos/500/500/?image=890" },
                    { 41, 5, "https://picsum.photos/500/500/?image=45" },
                    { 42, 5, "https://picsum.photos/500/500/?image=796" },
                    { 43, 5, "https://picsum.photos/500/500/?image=683" },
                    { 44, 5, "https://picsum.photos/500/500/?image=1052" },
                    { 45, 5, "https://picsum.photos/500/500/?image=292" },
                    { 46, 5, "https://picsum.photos/500/500/?image=792" },
                    { 47, 5, "https://picsum.photos/500/500/?image=488" },
                    { 48, 5, "https://picsum.photos/500/500/?image=664" },
                    { 49, 5, "https://picsum.photos/500/500/?image=50" },
                    { 50, 5, "https://picsum.photos/500/500/?image=274" },
                    { 51, 6, "https://picsum.photos/500/500/?image=402" },
                    { 52, 6, "https://picsum.photos/500/500/?image=578" },
                    { 53, 6, "https://picsum.photos/500/500/?image=387" },
                    { 54, 6, "https://picsum.photos/500/500/?image=378" },
                    { 55, 6, "https://picsum.photos/500/500/?image=266" },
                    { 56, 6, "https://picsum.photos/500/500/?image=386" },
                    { 57, 6, "https://picsum.photos/500/500/?image=421" },
                    { 58, 6, "https://picsum.photos/500/500/?image=963" },
                    { 59, 6, "https://picsum.photos/500/500/?image=656" },
                    { 60, 6, "https://picsum.photos/500/500/?image=1027" },
                    { 61, 7, "https://picsum.photos/500/500/?image=486" },
                    { 62, 7, "https://picsum.photos/500/500/?image=284" },
                    { 63, 7, "https://picsum.photos/500/500/?image=578" },
                    { 64, 7, "https://picsum.photos/500/500/?image=776" },
                    { 65, 7, "https://picsum.photos/500/500/?image=1018" },
                    { 66, 7, "https://picsum.photos/500/500/?image=146" },
                    { 67, 7, "https://picsum.photos/500/500/?image=368" },
                    { 68, 7, "https://picsum.photos/500/500/?image=185" },
                    { 69, 7, "https://picsum.photos/500/500/?image=1013" },
                    { 70, 7, "https://picsum.photos/500/500/?image=11" },
                    { 71, 8, "https://picsum.photos/500/500/?image=321" },
                    { 72, 8, "https://picsum.photos/500/500/?image=362" },
                    { 73, 8, "https://picsum.photos/500/500/?image=88" },
                    { 74, 8, "https://picsum.photos/500/500/?image=460" },
                    { 75, 8, "https://picsum.photos/500/500/?image=768" },
                    { 76, 8, "https://picsum.photos/500/500/?image=532" },
                    { 77, 8, "https://picsum.photos/500/500/?image=1011" },
                    { 78, 8, "https://picsum.photos/500/500/?image=359" },
                    { 79, 8, "https://picsum.photos/500/500/?image=598" },
                    { 80, 8, "https://picsum.photos/500/500/?image=431" },
                    { 81, 9, "https://picsum.photos/500/500/?image=150" },
                    { 82, 9, "https://picsum.photos/500/500/?image=137" },
                    { 83, 9, "https://picsum.photos/500/500/?image=0" },
                    { 84, 9, "https://picsum.photos/500/500/?image=970" },
                    { 85, 9, "https://picsum.photos/500/500/?image=639" },
                    { 86, 9, "https://picsum.photos/500/500/?image=1043" },
                    { 87, 9, "https://picsum.photos/500/500/?image=863" },
                    { 88, 9, "https://picsum.photos/500/500/?image=486" },
                    { 89, 9, "https://picsum.photos/500/500/?image=583" },
                    { 90, 9, "https://picsum.photos/500/500/?image=535" },
                    { 91, 10, "https://picsum.photos/500/500/?image=910" },
                    { 92, 10, "https://picsum.photos/500/500/?image=254" },
                    { 93, 10, "https://picsum.photos/500/500/?image=13" },
                    { 94, 10, "https://picsum.photos/500/500/?image=733" },
                    { 95, 10, "https://picsum.photos/500/500/?image=317" },
                    { 96, 10, "https://picsum.photos/500/500/?image=180" },
                    { 97, 10, "https://picsum.photos/500/500/?image=148" },
                    { 98, 10, "https://picsum.photos/500/500/?image=438" },
                    { 99, 10, "https://picsum.photos/500/500/?image=811" },
                    { 100, 10, "https://picsum.photos/500/500/?image=928" }
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
