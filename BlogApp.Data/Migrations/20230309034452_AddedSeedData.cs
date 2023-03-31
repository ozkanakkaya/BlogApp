using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Data.Migrations
{
    public partial class AddedSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "About",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_About", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    About = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GitHubLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WebsiteLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogCategories_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogTags_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedByUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedByUsername", "CreatedDate", "Description", "IsActive", "IsDeleted", "Name", "UpdatedByUsername", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9612), "Dünyadan teknoloji haberleri", true, false, "Teknoloji", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9613) },
                    { 2, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9621), "Güncel yazılım gelişmeleri", true, false, "Yazılım", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9622) },
                    { 3, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9626), "Dünyadan bilim haberleri", true, false, "Bilim", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9629) },
                    { 4, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9634), "Eğitim hakkında en iyi bloglar", true, false, "Eğitim", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9635) },
                    { 5, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9641), "Yerli ve yabancı otomobil haberleri", true, false, "Otomobil", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9641) },
                    { 6, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9649), "Yerli ve yabancı oyun sektörü haberleri", true, false, "Oyun", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9650) },
                    { 7, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9656), "Mühendislik alanlarındaki gelişmeler", true, false, "Mühendislik", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9657) },
                    { 8, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9662), "Spor dünyasından haberler", true, false, "Spor", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9664) },
                    { 9, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9669), "Kişisel gelişim hakkında en iyi bloglar", true, false, "Kişisel Gelişim", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9670) },
                    { 10, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9676), "Sürdürülebilir yaşam", true, false, "Sürdürülebilir Yaşam", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9677) },
                    { 11, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9683), "Dünyadan sağlıklı yaşam haberleri", true, false, "Sağlıklı Yaşam", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9684) },
                    { 12, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9688), "Seyahat hakkında en iyi bloglar", true, false, "Seyahat", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9691) },
                    { 13, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9695), "Kariyerinize yön verecek içerikler", true, false, "Kariyer", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9696) }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Definition" },
                values: new object[,]
                {
                    { 1, "Man" },
                    { 2, "Woman" },
                    { 3, "Undefined" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Category.Create" },
                    { 2, "Category.Read" },
                    { 3, "Category.Update" },
                    { 4, "Category.Delete" },
                    { 5, "Blog.Create" },
                    { 6, "Blog.Read" },
                    { 7, "Blog.Update" },
                    { 8, "Blog.Delete" },
                    { 9, "User.Create" },
                    { 10, "User.Read" },
                    { 11, "User.Update" },
                    { 12, "User.Delete" },
                    { 13, "Role.Create" },
                    { 14, "Role.Read" },
                    { 15, "Role.Update" },
                    { 16, "Role.Delete" },
                    { 17, "Comment.Create" },
                    { 18, "Comment.Read" },
                    { 19, "Comment.Update" },
                    { 20, "Comment.Delete" },
                    { 21, "Member" },
                    { 22, "Admin" },
                    { 23, "SuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedByUsername", "CreatedDate", "Description", "IsActive", "IsDeleted", "Name", "UpdatedByUsername", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9965), "Son teknoloji haberleri ve yenilikler", true, false, "Teknoloji", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9966) },
                    { 2, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9973), "En son bilim keşifleri ve araştırmaları", true, false, "Bilim Keşifleri", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9974) },
                    { 3, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9981), "Sanat dünyasındaki son gelişmeler ve haberler", true, false, "Sanat Dünyası", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9982) }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedByUsername", "CreatedDate", "Description", "IsActive", "IsDeleted", "Name", "UpdatedByUsername", "UpdatedDate" },
                values: new object[,]
                {
                    { 4, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9988), "En son otomobil haberleri ve yenilikler", true, false, "Otomobil", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9989) },
                    { 5, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9994), "Sağlıklı yaşam, beslenme ve egzersiz tavsiyeleri", true, false, "Sağlıklı Yaşam", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9997) },
                    { 6, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(1), "Eğitim dünyasındaki son gelişmeler ve haberler", true, false, "Eğitim Gündemi", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(2) },
                    { 7, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(8), "Kariyer yolculuğunuza rehberlik edecek tavsiyeler", true, false, "Kariyer", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(10) },
                    { 8, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(16), "Keşfedilmesi gereken seyahat rotaları ve öneriler", true, false, "Seyahat", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(17) },
                    { 9, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(23), "Eğlendirirken bilgilendiren içerikler", true, false, "Bilgi Dolu Eğlence", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(24) },
                    { 10, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(29), "Yazılım hakkında içerikler", true, false, "Yazılım", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(31) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "About", "CreatedByUsername", "CreatedDate", "Email", "Firstname", "GenderId", "GitHubLink", "ImageUrl", "IsActive", "IsDeleted", "Lastname", "PasswordHash", "PhoneNumber", "UpdatedByUsername", "UpdatedDate", "Username", "WebsiteLink" },
                values: new object[,]
                {
                    { 1, "Blog sitesinin süper admini", "ozkky", new DateTime(2023, 3, 9, 6, 44, 50, 70, DateTimeKind.Local).AddTicks(3473), "ozkky@gmail.com", "Özkan", 1, "ozkanakkaya", "userImages/defaultUser.png", true, false, "Akkaya", "AQAAAAIAAYagAAAAEHbRchXWaoVo3Oa8zhHC5opaH3eqIuZrf11cOqnd2RLZO9p+66/WgQF2PzQsMmQwwQ==", "5555555555", "ozkky", new DateTime(2023, 3, 9, 6, 44, 50, 70, DateTimeKind.Local).AddTicks(3474), "ozkky", "ozkanakkaya.com" },
                    { 2, "Blog sitesinin admini", "adminuser", new DateTime(2023, 3, 9, 6, 44, 50, 430, DateTimeKind.Local).AddTicks(7474), "adminuser@gmail.com", "Admin", 2, "adminuser", "userImages/defaultUser.png", true, false, "Admin", "AQAAAAIAAYagAAAAEJwfbn15YLPyGFtBMtevmUpLvJIZzRw/F8jwHrSbyIF0eGDRu1B/vDbS13zZu/GSgQ==", "5555555555", "adminuser", new DateTime(2023, 3, 9, 6, 44, 50, 430, DateTimeKind.Local).AddTicks(7475), "adminuser", "adminuser.com" },
                    { 3, "Blog sitesinde üye kullanıcı", "editoruser", new DateTime(2023, 3, 9, 6, 44, 50, 791, DateTimeKind.Local).AddTicks(2452), "memberuser@gmail.com", "Member", 1, "memberuser", "userImages/defaultUser.png", true, false, "Member", "AQAAAAIAAYagAAAAEJb5P6Voy4b8BXbClpPbgalGhH/S58RGzQrK6qQtiqjoHf6e/6j7fVIh/2iFgrzumw==", "5555555555", "editoruser", new DateTime(2023, 3, 9, 6, 44, 50, 791, DateTimeKind.Local).AddTicks(2453), "memberuser", "memberuser.com" },
                    { 4, "Blog sitesinde üye editör", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 120, DateTimeKind.Local).AddTicks(7444), "editoruser@gmail.com", "Editor", 1, "editoruser", "userImages/defaultUser.png", true, false, "Editor", "AQAAAAIAAYagAAAAECfXo6EBaqD8oLAx50p/rzaJWt9iiFmTGXqe5E1WfLejnZI9q1LgXA86Se5q6Y4G8A==", "5555555555", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 120, DateTimeKind.Local).AddTicks(7445), "editoruser", "editoruser.com" }
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "CommentCount", "Content", "CreatedByUsername", "CreatedDate", "ImageUrl", "IsActive", "IsDeleted", "LikeCount", "Title", "UpdatedByUsername", "UpdatedDate", "UserId", "ViewCount" },
                values: new object[,]
                {
                    { 1, 0, "Son yıllarda teknoloji hızla gelişiyor ve bu gelişmelerle birlikte veri bilimi ve makine öğrenmesi alanları da hızla büyüyor. Ancak, bu terimlerin ne anlama geldiği konusunda hala birçok insan kafa karışıklığı yaşayabilir. Bu yazıda, veri bilimi ve makine öğrenmesi hakkında genel bir bilgi vereceğim ve bunların neden önemli olduğunu anlatacağım.\r\n\r\nVeri bilimi, verilerin analiz edilmesi, yorumlanması ve anlamlı sonuçlar elde edilmesi için istatistik, matematik ve bilgisayar bilimlerinin kullanıldığı bir alandır. Veri bilimciler, verileri toplar, temizler, işler ve model oluştururlar. Bu modeller, öngörülebilir sonuçlar elde etmek için kullanılabilir.\r\n\r\nMakine öğrenmesi ise, bilgisayarların belirli bir görevi yerine getirmek için verileri kullanarak kendi kendine öğrenmesi ve geliştirmesi anlamına gelir. Bu yöntem, birçok alanda kullanılabilir. Örneğin, e-posta spam filtrelemesi, resim tanıma, doğal dil işleme ve daha birçok şey.\r\n\r\nNeden Veri Bilimi ve Makine Öğrenmesi Önemlidir?\r\n\r\nVeri bilimi ve makine öğrenmesi, işletmeler, hükümetler ve toplumlar için önemli bir rol oynamaktadır. Bu alanlar, daha iyi kararlar almak ve verimliliklerini artırmak için büyük miktarda veriyi analiz etmek zorundadır. Veri bilimi ve makine öğrenmesi, bu verilerin anlamlı hale getirilmesine yardımcı olabilir.\r\n\r\nÖrneğin, bir perakende şirketi, müşteri davranışları hakkında daha iyi bir anlayışa sahip olduğunda, daha iyi bir müşteri deneyimi sunabilir ve daha etkili bir pazarlama stratejisi oluşturabilir. Bir sağlık hizmetleri şirketi, büyük veri analizi kullanarak hastalıkların yayılmasını tahmin edebilir ve sağlık hizmetlerinin daha iyi bir şekilde yönetilmesine yardımcı olabilir.\r\n\r\nSonuç olarak, veri bilimi ve makine öğrenmesi, birçok alanda büyük önem taşıyan güçlü araçlardır. Bu alanların hızla gelişmesi, daha iyi kararlar almak ve verimliliği artırmak için daha fazla veriyi analiz etmemizi sağlayacaktır.", "ozkky", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9182), "postImages/defaultImage.png", true, false, 100, "Veri Bilimi ve Makine Öğrenmesi: Geleceğin Teknolojisi", "ozkky", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9183), 1, 100 },
                    { 2, 0, "Günümüzde teknolojinin hızlı gelişimiyle birlikte siber güvenlik tehditleri de artış göstermiştir. Siber suçlular, bireysel ve kurumsal kullanıcıların bilgi güvenliğini tehdit ederek, zararlı yazılımlar, kimlik avı saldırıları, DDoS saldırıları ve daha birçok yöntemle hedeflerine ulaşmaya çalışmaktadırlar. Bu nedenle siber güvenlik, günümüzde hayati bir öneme sahip bir konudur.\r\n\r\nSiber güvenlik tehditleri, bireysel kullanıcıların yanı sıra, işletmeler ve kurumlar için de büyük bir risk oluşturur. Siber saldırılara maruz kalan şirketler, müşteri güvenini kaybedebilir, maddi zararlarla karşılaşabilir ve hatta itibar kaybına uğrayabilirler. Bu nedenle, kurumlar siber güvenliği ciddiye almalı ve koruma önlemleri almalıdırlar.\r\n\r\nSiber güvenliği sağlamak için izlenebilecek birkaç yöntem şöyle sıralanabilir:\r\n\r\nGüçlü Parolalar Kullanın: Parolalar, çevrimiçi güvenliğiniz için ilk savunma hattıdır. Güçlü bir parola, saldırganların şifrenizi tahmin etmelerini önleyerek hesabınızın güvenliğini artırır. Şifrenizde büyük/küçük harfler, sayılar ve semboller kullanarak daha güçlü bir parola oluşturabilirsiniz.\r\n\r\nGüncel Yazılımlar Kullanın: Yazılım güncellemeleri, bilgisayarınızdaki güvenlik açıklarını kapatır. Bu nedenle, bilgisayarınızda kullandığınız yazılımları ve işletim sistemini güncel tutmanız, siber güvenliğiniz için çok önemlidir.\r\n\r\nKimlik Doğrulama Yöntemleri Kullanın: İki faktörlü kimlik doğrulama, parola ile birlikte bir başka doğrulama yöntemi daha kullanarak hesabınızın güvenliğini artırır. Bu sayede, saldırganların hesabınıza erişmelerini zorlaştırarak güvenliğinizi artırabilirsiniz.\r\n\r\nVeri Yedeklemesi Yapın: Yedekleme, olası bir saldırı veya sistem hatası durumunda verilerinizin korunmasını sağlar. Verilerinizi yedekleyerek, saldırı sonrası verilerinizi geri yükleyerek kayıplarınızı en aza indirebilirsiniz.", "ozkky", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9192), "postImages/defaultImage.png", true, false, 123, "Siber Güvenlik Tehditleri ve Önleme Yöntemleri", "ozkky", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9193), 1, 243 },
                    { 3, 0, "Günümüzde teknolojinin hızlı gelişimiyle birlikte siber güvenlik tehditleri de artış göstermiştir. Siber suçlular, bireysel ve kurumsal kullanıcıların bilgi güvenliğini tehdit ederek, zararlı yazılımlar, kimlik avı saldırıları, DDoS saldırıları ve daha birçok yöntemle hedeflerine ulaşmaya çalışmaktadırlar. Bu nedenle siber güvenlik, günümüzde hayati bir öneme sahip bir konudur.\r\n\r\nSiber güvenlik tehditleri, bireysel kullanıcıların yanı sıra, işletmeler ve kurumlar için de büyük bir risk oluşturur. Siber saldırılara maruz kalan şirketler, müşteri güvenini kaybedebilir, maddi zararlarla karşılaşabilir ve hatta itibar kaybına uğrayabilirler. Bu nedenle, kurumlar siber güvenliği ciddiye almalı ve koruma önlemleri almalıdırlar.\r\n\r\nSiber güvenliği sağlamak için izlenebilecek birkaç yöntem şöyle sıralanabilir:\r\n\r\nGüçlü Parolalar Kullanın: Parolalar, çevrimiçi güvenliğiniz için ilk savunma hattıdır. Güçlü bir parola, saldırganların şifrenizi tahmin etmelerini önleyerek hesabınızın güvenliğini artırır. Şifrenizde büyük/küçük harfler, sayılar ve semboller kullanarak daha güçlü bir parola oluşturabilirsiniz.\r\n\r\nGüncel Yazılımlar Kullanın: Yazılım güncellemeleri, bilgisayarınızdaki güvenlik açıklarını kapatır. Bu nedenle, bilgisayarınızda kullandığınız yazılımları ve işletim sistemini güncel tutmanız, siber güvenliğiniz için çok önemlidir.\r\n\r\nKimlik Doğrulama Yöntemleri Kullanın: İki faktörlü kimlik doğrulama, parola ile birlikte bir başka doğrulama yöntemi daha kullanarak hesabınızın güvenliğini artırır. Bu sayede, saldırganların hesabınıza erişmelerini zorlaştırarak güvenliğinizi artırabilirsiniz.\r\n\r\nVeri Yedeklemesi Yapın: Yedekleme, olası bir saldırı veya sistem hatası durumunda verilerinizin korunmasını sağlar. Verilerinizi yedekleyerek, saldırı sonrası verilerinizi geri yükleyerek kayıplarınızı en aza indirebilirsiniz.", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9200), "postImages/defaultImage.png", true, false, 134, "Yapay Zeka ve Otomasyon", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9201), 2, 222 },
                    { 4, 0, "Günümüzde teknolojinin hızlı gelişimiyle birlikte siber güvenlik tehditleri de artış göstermiştir. Siber suçlular, bireysel ve kurumsal kullanıcıların bilgi güvenliğini tehdit ederek, zararlı yazılımlar, kimlik avı saldırıları, DDoS saldırıları ve daha birçok yöntemle hedeflerine ulaşmaya çalışmaktadırlar. Bu nedenle siber güvenlik, günümüzde hayati bir öneme sahip bir konudur.\r\n\r\nSiber güvenlik tehditleri, bireysel kullanıcıların yanı sıra, işletmeler ve kurumlar için de büyük bir risk oluşturur. Siber saldırılara maruz kalan şirketler, müşteri güvenini kaybedebilir, maddi zararlarla karşılaşabilir ve hatta itibar kaybına uğrayabilirler. Bu nedenle, kurumlar siber güvenliği ciddiye almalı ve koruma önlemleri almalıdırlar.\r\n\r\nSiber güvenliği sağlamak için izlenebilecek birkaç yöntem şöyle sıralanabilir:\r\n\r\nGüçlü Parolalar Kullanın: Parolalar, çevrimiçi güvenliğiniz için ilk savunma hattıdır. Güçlü bir parola, saldırganların şifrenizi tahmin etmelerini önleyerek hesabınızın güvenliğini artırır. Şifrenizde büyük/küçük harfler, sayılar ve semboller kullanarak daha güçlü bir parola oluşturabilirsiniz.\r\n\r\nGüncel Yazılımlar Kullanın: Yazılım güncellemeleri, bilgisayarınızdaki güvenlik açıklarını kapatır. Bu nedenle, bilgisayarınızda kullandığınız yazılımları ve işletim sistemini güncel tutmanız, siber güvenliğiniz için çok önemlidir.\r\n\r\nKimlik Doğrulama Yöntemleri Kullanın: İki faktörlü kimlik doğrulama, parola ile birlikte bir başka doğrulama yöntemi daha kullanarak hesabınızın güvenliğini artırır. Bu sayede, saldırganların hesabınıza erişmelerini zorlaştırarak güvenliğinizi artırabilirsiniz.\r\n\r\nVeri Yedeklemesi Yapın: Yedekleme, olası bir saldırı veya sistem hatası durumunda verilerinizin korunmasını sağlar. Verilerinizi yedekleyerek, saldırı sonrası verilerinizi geri yükleyerek kayıplarınızı en aza indirebilirsiniz.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9208), "postImages/defaultImage.png", true, false, 233, "İnovasyon ve Yenilik", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9209), 4, 2323 },
                    { 5, 0, "Günümüzde evde geçirdiğimiz zamanın artması, iç mekan dekorasyonuna olan ilgiyi arttırdı. İç mekan dekorasyonu sadece evin güzel görünmesini sağlamakla kalmaz, aynı zamanda ruh halimizi ve genel sağlığımızı da etkileyebilir. Doğru renkler, aydınlatma, mobilyalar ve aksesuarlar, evinizi sıcak, rahat ve davetkar bir yer haline getirebilir.\r\n\r\nEvde kalmanın keyfini çıkarmak için, iç mekan dekorasyonunda kişisel tarzınıza uygun seçimler yapabilirsiniz. Minimalist bir tarz mı tercih ediyorsunuz yoksa renkli ve canlı bir dekor mu istiyorsunuz? Evinizdeki doğal ışığı artırmak için hangi renkler ve perdeleri tercih etmelisiniz? Tüm bu soruların yanıtlarını, evde kalmak süresince kendinize daha uygun bir yaşam alanı yaratmak için kullanabilirsiniz.\r\n\r\nUnutmayın, eviniz sizin kişisel alanınızdır ve onun size iyi hissettirmesi önemlidir. Bu nedenle, iç mekan dekorasyonunda kendi stilinizi ve kişiliğinizi yansıtan seçimler yaparak evde kalmanın keyfini çıkarabilirsiniz.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9215), "postImages/defaultImage.png", true, false, 2113, "Evde Kalmanın Keyfini Çıkarma: İç Mekan Dekorasyon Fikirleri", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9216), 4, 2333 },
                    { 6, 0, "Sağlıklı beslenmek, vücudumuzun ihtiyacı olan besinleri alarak sağlıklı bir yaşam sürdürmek için önemlidir. Günümüzde fast food ve işlenmiş gıdaların tüketimi arttıkça, vitamin ve mineral eksikliği gibi sağlık sorunları da artmaktadır. Vitaminler ve mineraller, vücudumuzun doğru bir şekilde çalışması için gerekli olan besin öğeleridir. Bu besinler, kemiklerin güçlendirilmesi, bağışıklık sisteminin korunması ve enerji üretimi gibi birçok vücut fonksiyonu için gereklidir.\r\n\r\nSağlıklı bir beslenme planı, yeterli miktarda vitamin ve mineral alımını içerir. C vitamini, demir, kalsiyum, magnezyum ve potasyum gibi vitamin ve mineraller, günlük diyetimizde yer alması gereken önemli besin öğeleridir. Meyve, sebze, tam tahıllı gıdalar, kuruyemişler ve tohumlar, bu besinlerin en iyi kaynaklarıdır.\r\n\r\nSağlıklı bir beslenme tarzını benimsemek, sadece fiziksel sağlığımız için değil, aynı zamanda mental sağlığımız ve yaşam kalitemiz için de önemlidir. Düzenli egzersizle birlikte sağlıklı beslenme, stres azaltmaya, zihinsel netliği artırmaya ve genel olarak daha mutlu bir yaşam sürdürmeye yardımcı olabilir.\r\n\r\nSonuç olarak, sağlıklı beslenme alışkanlıkları benimsemek, vücudumuzun ihtiyacı olan vitamin ve mineralleri alarak sağlıklı bir yaşam sürdürmek için kritik bir adımdır. Sağlıklı bir diyet, düzenli egzersiz ve yaşam tarzı değişiklikleri, sağlıklı yaşamın anahtarıdır.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9221), "postImages/defaultImage.png", true, false, 1223, "Sağlıklı Beslenmenin Önemi: Vitamin ve Minerallerin Günlük Hayatta Rolü", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9222), 4, 3323 },
                    { 7, 0, "Teknolojinin gelişmesiyle birlikte, dijital dünya ve sosyal medya hayatımızda önemli bir rol oynamaya başladı. İnternet ve sosyal medya platformları, insanlar arasındaki iletişim ve etkileşimi kolaylaştırırken, aynı zamanda yeni sorunlar ve endişeler de yaratıyor.\r\n\r\nSosyal medya, insanların arkadaşlarıyla ve aileleriyle bağlantıda kalmalarına ve dünyadaki olaylar hakkında bilgi edinmelerine yardımcı olurken, zaman zaman da zararlı etkileri olabilir. Sosyal medya kullanımı, özellikle gençler arasında, düşük özgüven, yalnızlık, kaygı ve depresyon gibi ruh sağlığı sorunlarına neden olabilir.\r\n\r\nAyrıca, sosyal medya ve dijital dünya, özellikle çocuklar ve gençler arasında, aşırı tüketim, sosyal medya bağımlılığı, internet güvenliği konularında ciddi endişelere neden oluyor. Dolayısıyla, sosyal medya ve dijital dünya ile ilgili farkındalık yaratmak, bilinçli bir şekilde kullanımını sağlamak için önemlidir.\r\n\r\nSonuç olarak, dijital dünya ve sosyal medya, hayatımızda önemli bir rol oynamaya devam edecek. Bu nedenle, insanlar olarak bu platformları nasıl kullanacağımızı öğrenmeliyiz ve özellikle gençler arasında, sorumlu ve bilinçli bir şekilde kullanımını sağlamalıyız. Sosyal medya ve dijital dünya, hayatımızın bir parçası olsa da, sağlıklı bir denge oluşturmak önemlidir.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9227), "postImages/defaultImage.png", true, false, 123, "Dijital Dünya ve Sosyal Medyanın Hayatımıza Etkisi", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9228), 4, 1223 },
                    { 8, 0, "Düzenli egzersiz yapmak, sağlıklı bir yaşam tarzının temelidir. Egzersiz yapmak, fiziksel ve zihinsel sağlığı geliştirir, kilo kontrolüne yardımcı olur, stresi azaltır, enerji seviyelerini artırır ve kalp hastalığı, diyabet ve kanser gibi birçok kronik hastalığı önlemeye yardımcı olur.\r\n\r\nAyrıca, düzenli egzersiz yapmak, kas ve kemik sağlığını korumaya yardımcı olur, vücutta yağ oranını azaltır, kan dolaşımını artırır ve bağışıklık sistemini güçlendirir. Düzenli egzersiz yapmak aynı zamanda zihinsel sağlığı da etkiler ve stres ve kaygı gibi duygusal durumları kontrol altına almaya yardımcı olur.\r\n\r\nAncak, düzenli egzersiz yapmak için zaman ve motivasyon bulmak her zaman kolay değildir. Başlangıçta, yavaş ve istikrarlı bir programla başlamak ve egzersiz hedeflerini yavaş yavaş artırmak önemlidir. Ayrıca, egzersiz programını eğlenceli hale getirmek, bir egzersiz ortağı bulmak ve farklı egzersiz türlerini denemek, motivasyonu artırabilir.\r\n\r\nSonuç olarak, düzenli egzersiz yapmak, sağlıklı bir yaşam tarzının vazgeçilmez bir parçasıdır. Egzersiz programını kişiselleştirerek, hedeflerimize uygun bir şekilde tasarlamalıyız. Egzersiz yapmak zaman ve motivasyon gerektirse de, sağlığımız için yaptığımız en iyi yatırım olduğunu unutmamalıyız.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9234), "postImages/defaultImage.png", true, false, 222, "Düzenli Egzersizin Önemi", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9235), 4, 2324 },
                    { 9, 0, "Dünya üzerindeki çevre sorunları giderek artıyor ve bu sorunların çözümü için sürdürülebilirlik kavramı hayati önem taşıyor. Sürdürülebilirlik, insanların ihtiyaçlarını karşılamak için doğal kaynakları kullanırken, gelecek nesillerin ihtiyaçlarını da göz önünde bulunduran bir yaklaşımı ifade eder.\r\n\r\nGünümüzde, iklim değişikliği, su kaynaklarının azalması, deniz kirliliği, ormansızlaşma, doğal habitatların yok olması ve atık yönetimi gibi birçok çevre sorunu var. Bu sorunların büyük bir kısmı, insan faaliyetlerinden kaynaklanmaktadır.\r\n\r\nSürdürülebilirlik, bu sorunları önlemek için birçok çözüm sunar. Örneğin, yenilenebilir enerji kaynaklarına yatırım yaparak, fosil yakıt tüketimini azaltabiliriz. Atık yönetimi programları oluşturarak, atık miktarını azaltabilir ve geri dönüşüm yaparak kaynakları yeniden kullanabiliriz. Sürdürülebilirlik aynı zamanda, tarım yöntemlerini değiştirerek, toprak erozyonunu önleyebilir ve su kaynaklarını daha etkin bir şekilde kullanabiliriz.\r\n\r\nSürdürülebilirlik, gelecek nesillerin ihtiyaçlarını da göz önünde bulundurduğu için, uzun vadeli bir yaklaşımı ifade eder. Bu nedenle, sürdürülebilirliğin önemi giderek artıyor. Herkesin bu konuda sorumluluk alması ve sürdürülebilir bir gelecek için çalışması gerekiyor.\r\n\r\nSonuç olarak, çevre sorunları ve sürdürülebilirlik, hayatımızın önemli bir parçasıdır. Sürdürülebilir bir gelecek için, herkesin sorumluluk alması ve doğal kaynakları verimli bir şekilde kullanması gerekiyor. Gelecek nesillerin de ihtiyaçlarını göz önünde bulundurarak, sürdürülebilir bir dünya için çalışmalıyız.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9240), "postImages/defaultImage.png", true, false, 23, "Çevre Sorunları ve Sürdürülebilirlik", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9241), 4, 223 },
                    { 10, 0, "Dijital dönüşüm, işletmelerin dijital teknolojileri kullanarak iş süreçlerini yenilemesi ve iyileştirmesi anlamına gelir. Günümüzde işletmelerin büyük bir kısmı, dijital dönüşümü benimseyerek rekabet avantajı elde etmeye çalışıyor.\r\n\r\nDijital dönüşüm, işletmelerin müşteri deneyimini geliştirmesi, verimliliği artırması ve maliyetleri düşürmesi için birçok fırsat sunar. Örneğin, bulut tabanlı yazılımlar sayesinde işletmeler, verilerini güvenli bir şekilde depolayabilir ve erişebilir hale getirebilir. Bu da işletmelerin daha hızlı ve etkili kararlar almasına yardımcı olur.\r\n\r\nDijital dönüşüm aynı zamanda, işletmelerin müşterileriyle daha iyi etkileşim kurmasını sağlar. Örneğin, sosyal medya platformları sayesinde işletmeler, müşterileriyle daha kolay ve etkili bir şekilde iletişim kurabilir ve geri bildirimleri hızlı bir şekilde alabilir. Bu da müşteri deneyimini geliştirmeye yardımcı olur ve müşteri sadakatini artırır.\r\n\r\nDijital dönüşümün bir diğer avantajı da, işletmelerin daha verimli çalışmasını sağlamasıdır. Otomasyon teknolojileri sayesinde, işletmeler manuel işlemleri otomatikleştirebilir ve çalışanların zamanını daha verimli kullanabilir. Bu da işletmelerin maliyetleri düşürmesine ve üretkenliği artırmasına yardımcı olur.\r\n\r\nSonuç olarak, dijital dönüşüm işletmeler için önemli bir fırsat sunar. İşletmeler, dijital teknolojileri benimseyerek müşteri deneyimini geliştirebilir, verimliliği artırabilir ve maliyetleri düşürebilir. Dijital dönüşümü benimseyen işletmeler, rekabet avantajı elde edebilir ve başarıya daha kolay ulaşabilir.", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9248), "postImages/defaultImage.png", true, false, 1233, "Dijital Dönüşüm ve İşletmeler", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9249), 2, 2223 },
                    { 11, 0, "Yoga, bedenin fiziksel ve zihinsel sağlığı için birçok faydası olan bir uygulamadır. Yoga yapmak, insanların stresi azaltmasına, zihinsel sağlığı iyileştirmesine ve genel refahı arttırmasına yardımcı olur.\r\n\r\nYoga, vücuttaki kasları ve eklemleri güçlendirir, esnekliği arttırır ve duruşu düzeltir. Bu, aynı zamanda yaralanma riskini de azaltır. Ancak yoga sadece fiziksel sağlığa fayda sağlamaz, aynı zamanda zihinsel sağlığı da destekler.\r\n\r\nYoga yapmak, kişilerin zihinsel durumunu düzeltir ve stresi azaltır. Yoga uygulayıcıları, derin nefes alarak stresi azaltır ve zihinsel berraklığı arttırır. Bu da, zihinsel sağlığı iyileştirir ve depresyon, anksiyete ve diğer zihinsel sağlık sorunlarının semptomlarını hafifletir.\r\n\r\nYoga aynı zamanda, kişilerin farkındalığını arttırır ve meditatif bir etki yaratır. Bu, düşünceleri sakinleştirir ve kişilerin daha yüksek bir bilince ulaşmasına yardımcı olur. Yoga ayrıca, uyku kalitesini de arttırır ve yorgunluğu azaltır.\r\n\r\nSonuç olarak, yoga insanların hem fiziksel hem de zihinsel sağlıklarını iyileştirmelerine yardımcı olan bir uygulamadır. Yoga yapmak, stresi azaltır, zihinsel sağlığı iyileştirir, farkındalığı arttırır ve genel refahı arttırır. Yoga, sağlıklı bir yaşam tarzı için harika bir ek olarak da kullanılabilir.", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9254), "postImages/defaultImage.png", true, false, 211, "Yoga ve Zihinsel Sağlık", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9255), 2, 2123 },
                    { 12, 0, "Dijital yorgunluk, sürekli olarak dijital teknolojilerle etkileşim halinde olan insanların maruz kaldığı bir durumdur. Bu, sürekli olarak telefonlara, bilgisayarlara, tablet'lere ve diğer dijital cihazlara bakarak uzun saatler geçirerek gerçekleşir.\r\n\r\nDijital yorgunluk, yorgunluk, halsizlik, baş ağrısı, gözlerde yanma ve kuru his, boyun ağrısı, uykusuzluk, konsantrasyon güçlüğü ve hatta depresyon gibi çeşitli semptomlara neden olabilir. Bu semptomlar, dijital teknolojilere aşırı maruz kalmanın bir sonucudur.\r\n\r\nDijital yorgunluğu önlemek için, bazı öneriler şunlar olabilir:\r\n\r\nDijital cihaz kullanımını azaltın. Çok fazla zaman harcamaktan kaçının.\r\nTelefonunuza veya diğer cihazlarınıza bakmak yerine, gerçek hayattaki etkinliklere katılın.\r\nİnternet kullanımını sınırlandırın. Sosyal medya ve diğer dijital platformlarda harcadığınız süreyi azaltın.\r\nKendinize ara verin. Dijital cihazlarla aranızda zaman zaman uzaklaşın ve kendi kendinizle baş başa kalın.\r\nGözlerinizi dinlendirmek için her 20-30 dakikada bir göz egzersizleri yapın.\r\nSonuç olarak, dijital yorgunluk birçok kişi için yaygın bir sorundur. Bu semptomları önlemek için, dijital cihazların kullanımını sınırlandırmak ve kendi kendinizle zaman geçirmek önemlidir. Dijital yorgunluğun önlenmesi, daha sağlıklı bir yaşam tarzını sürdürmek için önemlidir.", "ozkky", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9520), "postImages/defaultImage.png", true, false, 233, "Dijital Yorgunluk: Belirtileri ve Nasıl Önlenir?", "ozkky", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9523), 1, 2323 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 23, 1 },
                    { 2, 22, 2 },
                    { 3, 21, 3 },
                    { 4, 6, 3 },
                    { 5, 17, 3 },
                    { 6, 21, 4 },
                    { 7, 6, 4 },
                    { 8, 7, 4 },
                    { 9, 17, 4 }
                });

            migrationBuilder.InsertData(
                table: "BlogCategories",
                columns: new[] { "Id", "BlogId", "CategoryId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 5 },
                    { 3, 2, 1 },
                    { 4, 3, 4 },
                    { 5, 3, 6 },
                    { 6, 3, 9 },
                    { 7, 4, 8 },
                    { 8, 5, 4 },
                    { 9, 6, 7 },
                    { 10, 6, 10 },
                    { 11, 7, 12 },
                    { 12, 8, 13 },
                    { 13, 9, 2 },
                    { 14, 9, 3 },
                    { 15, 10, 1 },
                    { 16, 10, 9 },
                    { 17, 11, 4 },
                    { 18, 11, 5 },
                    { 19, 12, 11 }
                });

            migrationBuilder.InsertData(
                table: "BlogTags",
                columns: new[] { "Id", "BlogId", "TagId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 2, 4 },
                    { 4, 2, 6 },
                    { 5, 3, 2 },
                    { 6, 3, 3 },
                    { 7, 4, 1 },
                    { 8, 4, 9 },
                    { 9, 5, 7 },
                    { 10, 5, 10 },
                    { 11, 6, 2 },
                    { 12, 7, 3 },
                    { 13, 7, 5 },
                    { 14, 8, 4 },
                    { 15, 8, 6 },
                    { 16, 9, 7 },
                    { 17, 9, 8 },
                    { 18, 10, 9 },
                    { 19, 10, 10 },
                    { 20, 11, 2 },
                    { 21, 11, 3 },
                    { 22, 12, 3 },
                    { 23, 12, 4 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "BlogId", "Content", "CreatedByUsername", "CreatedDate", "Email", "IsActive", "IsDeleted", "UpdatedByUsername", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, "Teknoloji hayatımızın ayrılmaz bir parçası haline geldi. Gelişen teknoloji ile birlikte insan hayatı daha da kolaylaşıyor ve dijital dünya daha da genişliyor.", "memberuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9831), "memberuser@gmail.com", true, false, "memberuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9832) },
                    { 2, 2, "Bilim, insanlık için son derece önemlidir. Bilim sayesinde dünya hakkında daha fazla şey öğreniyor ve hayatımızı daha iyi hale getiriyoruz. Bilimin gelişmesiyle birlikte, tıp, çevre, enerji ve diğer alanlarda önemli ilerlemeler kaydedildi.", "memberuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9840), "memberuser@gmail.com", true, false, "memberuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9841) },
                    { 3, 3, "Sanat, insanlığın ruhunu besleyen bir kaynaktır. Sanat, farklı kültürler ve farklı dönemlerden gelen insanların duygularını, hayallerini ve düşüncelerini yansıtır.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9848), "editoruser@gmail.com", true, false, "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9849) },
                    { 4, 4, "Otomobil, modern yaşamın önemli bir parçasıdır. Otomobiller, günlük hayatımızda işe, okula veya seyahat etmek için kullandığımız en yaygın taşıtlardan biridir. Ancak, otomobillerin çevresel etkileri de düşünülmesi gereken önemli bir konudur.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9856), "editoruser@gmail.com", true, false, "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9857) },
                    { 5, 5, "Sağlıklı yaşam, insan hayatı için son derece önemlidir. Sağlıklı beslenme, düzenli egzersiz ve uyku, stres yönetimi gibi faktörler, sağlıklı yaşam için önemlidir. Sağlıklı yaşam, uzun ve mutlu bir hayat için temel gereksinimdir.", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9863), "editoruser@gmail.com", true, false, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9864) },
                    { 6, 5, "Eğitim, toplumun gelişmesi için en önemli araçlardan biridir. Eğitim sayesinde insanlar, dünya hakkında daha fazla şey öğrenirler ve kendilerini geliştirirler. Eğitim, insan hayatında bir dönüm noktasıdır ve hayatımız boyunca öğrenmeye devam etmek önemlidir.", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9868), "editoruser@gmail.com", true, false, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9871) },
                    { 7, 4, "Kariyer, iş hayatında başarıya ulaşmak için önemlidir. İyi bir kariyer yapmak, finansal bağımsızlık ve kişisel tatmin açısından önemlidir. Ancak, kariyer hedefleri belirlerken, kişinin yeteneklerini ve değerlerini dikkate alması gereklidir.", "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9877), "editoruser@gmail.com", true, false, "editoruser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9880) },
                    { 8, 5, "Seyahat, yeni yerler keşfetmek, farklı kültürleri tanımak ve yeni deneyimler edinmek için harika bir yoldur. Seyahat etmek, kişinin bakış açısını genişletir ve yaşam deneyimini zenginleştirir. Ancak, seyahat ederken, diğer kültürlerin saygı göstermek ve doğal kaynaklara zarar vermemek gibi sorumlulukları da unutmamak gereklidir.", "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9891), "editoruser@gmail.com", true, false, "adminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9892) },
                    { 9, 6, "Eğitimde yeni metotlar, öğrenme teknikleri, öğrenci motivasyonu ve sınavlara hazırlık gibi konular hakkında paylaşılan bilgiler, öğrencilerin başarılarını arttırabilir.", "memberuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9900), "memberuser@gmail.com", true, false, "memberuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9901) },
                    { 10, 7, "Yeni kültürler keşfetmek, farklı yemekleri tatmak, yeni insanlarla tanışmak gibi seyahat etmenin birçok faydası var. Bu başlık altında yer alan yazılar, insanların seyahatlerini planlamalarına ve daha keyifli bir deneyim yaşamalarına yardımcı olabilir.", "superadminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9907), "ozkky@gmail.com", true, false, "superadminuser", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9908) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_BlogId_CategoryId",
                table: "BlogCategories",
                columns: new[] { "BlogId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_CategoryId",
                table: "BlogCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_BlogId",
                table: "BlogTags",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_TagId_BlogId",
                table: "BlogTags",
                columns: new[] { "TagId", "BlogId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId_UserId",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GenderId",
                table: "Users",
                column: "GenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "About");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "BlogTags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}
