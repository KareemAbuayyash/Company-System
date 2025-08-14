using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3845));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3847));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3995));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3997));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3998));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3953));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3955));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3956));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3958));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastLoginDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3976), new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3975), "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastLoginDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3980), new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3979), "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastLoginDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3982), new DateTime(2025, 8, 13, 8, 59, 46, 956, DateTimeKind.Utc).AddTicks(3982), "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7489));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7630));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7632));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7585));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7588));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7589));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7591));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastLoginDate" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7608), new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastLoginDate" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7612), new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7611) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastLoginDate" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7615), new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7614) });
        }
    }
}
