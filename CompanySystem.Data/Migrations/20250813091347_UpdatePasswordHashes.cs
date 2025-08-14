using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1344));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1348));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1302));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1304));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1306));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastLoginDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1326), new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1325), "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastLoginDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1329), new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1328), "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastLoginDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1332), new DateTime(2025, 8, 13, 9, 13, 46, 902, DateTimeKind.Utc).AddTicks(1332), "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
