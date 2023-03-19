using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Data.Migrations
{
    public partial class UpdatedComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6547), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6555) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6563), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6564) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6569), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6570) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6575), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6576) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6581), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6582) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6587), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6588) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6593), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6594) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6600), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6601) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6607), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6608) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6612), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6613) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6618), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6619) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6624), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6625) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6722), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6723) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6729), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6730) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6734), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6736) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6740), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6740) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6746), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6747) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6751), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6752) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6757), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6758) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6762), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6763) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6767), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6768) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6772), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6773) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6778), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6779) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6784), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6785) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6790), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(6791) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7186), "Member", "userImages/defaultUser.png", "Member", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7187), 3 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7194), "Member", "userImages/defaultUser.png", "Member", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7195), 3 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7200), "Editor", "userImages/defaultUser.png", "Editor", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7201), 4 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7206), "Editor", "userImages/defaultUser.png", "Editor", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7207), 4 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Email", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7213), "adminuser@gmail.com", "Admin", "userImages/defaultUser.png", "Admin", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7214), 2 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Email", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7219), "adminuser@gmail.com", "Admin", "userImages/defaultUser.png", "Admin", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7220), 2 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7226), "Editor", "userImages/defaultUser.png", "Editor", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7227), 4 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "Email", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7232), "adminuser@gmail.com", "Admin", "userImages/defaultUser.png", "Admin", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7233), 2 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7239), "Member", "userImages/defaultUser.png", "Member", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7240), 3 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "Firstname", "ImageUrl", "Lastname", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7245), "Özkan", "userImages/defaultUser.png", "Akkaya", new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7246), 1 });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7301), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7303) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7309), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7310) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7315), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7316) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7322), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7323) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7328), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7328) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7333), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7334) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7338), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7339) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7343), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7344) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7349), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7350) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7354), new DateTime(2023, 3, 17, 15, 41, 10, 17, DateTimeKind.Local).AddTicks(7355) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 9, 94, DateTimeKind.Local).AddTicks(5827), "AQAAAAIAAYagAAAAECMT+LfFaspuqH3rPFcY1cdJNl9KDnNIAUyA8PRj285xzgTTII9KTJ9We6gArcsjCg==", new DateTime(2023, 3, 17, 15, 41, 9, 94, DateTimeKind.Local).AddTicks(5829) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 9, 316, DateTimeKind.Local).AddTicks(1181), "AQAAAAIAAYagAAAAEGwQEUuOvHt1f6fni8zjLcXIpw/+zhM676D5jc6Icq7JYVeohcJgJ3Fdnv+P8S7pCQ==", new DateTime(2023, 3, 17, 15, 41, 9, 316, DateTimeKind.Local).AddTicks(1183) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 9, 574, DateTimeKind.Local).AddTicks(539), "AQAAAAIAAYagAAAAENqVZfuCj1lTdGD70L7sv5cmPyG65iq4mZKOlHAarOccsBX4bhpkpT2jwFUxDA0wng==", new DateTime(2023, 3, 17, 15, 41, 9, 574, DateTimeKind.Local).AddTicks(541) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 17, 15, 41, 9, 768, DateTimeKind.Local).AddTicks(4974), "AQAAAAIAAYagAAAAEJqEXWKJMh/sr3WEgXNaqUZrkqCJL/PzOhggckw9c+NiZNQAjhF8Ap0UafM5stTqGA==", new DateTime(2023, 3, 17, 15, 41, 9, 768, DateTimeKind.Local).AddTicks(4976) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9182), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9183) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9192), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9193) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9200), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9201) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9208), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9209) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9215), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9216) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9221), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9222) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9227), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9228) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9234), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9240), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9241) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9248), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9249) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9254), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9255) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9520), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9523) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9612), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9613) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9621), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9622) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9626), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9629) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9634), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9635) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9641), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9641) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9649), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9650) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9656), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9657) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9662), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9664) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9669), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9670) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9676), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9677) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9683), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9684) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9688), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9691) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9695), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9696) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9831), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9832) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9840), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9841) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9848), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9849) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9856), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9857) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Email", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9863), "editoruser@gmail.com", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9864) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Email", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9868), "editoruser@gmail.com", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9871) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9877), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9880) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "Email", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9891), "editoruser@gmail.com", new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9892) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9900), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9901) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9907), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9908) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9965), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9966) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9973), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9974) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9981), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9982) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9988), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9989) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9994), new DateTime(2023, 3, 9, 6, 44, 51, 478, DateTimeKind.Local).AddTicks(9997) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(1), new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(2) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(8), new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(10) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(16), new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(17) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(23), new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(24) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(29), new DateTime(2023, 3, 9, 6, 44, 51, 479, DateTimeKind.Local).AddTicks(31) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 50, 70, DateTimeKind.Local).AddTicks(3473), "AQAAAAIAAYagAAAAEHbRchXWaoVo3Oa8zhHC5opaH3eqIuZrf11cOqnd2RLZO9p+66/WgQF2PzQsMmQwwQ==", new DateTime(2023, 3, 9, 6, 44, 50, 70, DateTimeKind.Local).AddTicks(3474) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 50, 430, DateTimeKind.Local).AddTicks(7474), "AQAAAAIAAYagAAAAEJwfbn15YLPyGFtBMtevmUpLvJIZzRw/F8jwHrSbyIF0eGDRu1B/vDbS13zZu/GSgQ==", new DateTime(2023, 3, 9, 6, 44, 50, 430, DateTimeKind.Local).AddTicks(7475) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 50, 791, DateTimeKind.Local).AddTicks(2452), "AQAAAAIAAYagAAAAEJb5P6Voy4b8BXbClpPbgalGhH/S58RGzQrK6qQtiqjoHf6e/6j7fVIh/2iFgrzumw==", new DateTime(2023, 3, 9, 6, 44, 50, 791, DateTimeKind.Local).AddTicks(2453) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 3, 9, 6, 44, 51, 120, DateTimeKind.Local).AddTicks(7444), "AQAAAAIAAYagAAAAECfXo6EBaqD8oLAx50p/rzaJWt9iiFmTGXqe5E1WfLejnZI9q1LgXA86Se5q6Y4G8A==", new DateTime(2023, 3, 9, 6, 44, 51, 120, DateTimeKind.Local).AddTicks(7445) });
        }
    }
}
