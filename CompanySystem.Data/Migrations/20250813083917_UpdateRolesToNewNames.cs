using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRolesToNewNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedDate", "RoleName" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7588), "HR" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Description", "RoleName" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7589), "Team lead and project management access", "Lead" });

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
                columns: new[] { "CreatedDate", "LastLoginDate", "LastName", "Username" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7612), new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7611), "User", "hr.user" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Email", "FirstName", "LastLoginDate", "LastName", "Username" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7615), "lead@company.com", "Team", new DateTime(2025, 8, 13, 8, 39, 16, 976, DateTimeKind.Utc).AddTicks(7614), "Lead", "lead.user" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5540));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5542));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5662));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5664));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5666));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5616));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "RoleName" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5618), "HR Manager" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Description", "RoleName" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5620), "Information Technology management access", "IT Manager" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5621));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastLoginDate" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5643), new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5642) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastLoginDate", "LastName", "Username" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5646), new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5645), "Manager", "hr.manager" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Email", "FirstName", "LastLoginDate", "LastName", "Username" },
                values: new object[] { new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5649), "it@company.com", "IT", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5648), "Manager", "it.manager" });
        }
    }
}
