using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "MainPageContent",
                keyColumn: "ContentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MainPageContent",
                keyColumn: "ContentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MainPageContent",
                keyColumn: "ContentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Users",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                newName: "IX_Users_SerialNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Id",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                columns: new[] { "Id", "SerialNumber" });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 5, 19, 23, 23, 830, DateTimeKind.Utc).AddTicks(103));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 5, 19, 23, 23, 830, DateTimeKind.Utc).AddTicks(105));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 5, 19, 23, 23, 830, DateTimeKind.Utc).AddTicks(106));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 5, 19, 23, 23, 830, DateTimeKind.Utc).AddTicks(108));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Id",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Users",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SerialNumber",
                table: "Users",
                newName: "IX_Users_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3706));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3707));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3708));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "DepartmentId", "Email", "EmployeeId", "Experience", "FirstName", "HireDate", "IsActive", "LastName", "PasswordHash", "PhoneNumber", "ProfilePhoto", "RoleId", "Salary", "Skills", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3688), null, "admin@company.com", "ADMIN001", null, "System", new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3687), true, "Administrator", "AQAAAAEAACcQAAAAEBv+X3w5V5yh3Z8bX0cX8+9YzGK8h7K2L1M6N9o0P3q4R7s8T5u6V9w0X3y6Z9a2B5c8", null, null, 1, null, null, null });

            migrationBuilder.InsertData(
                table: "MainPageContent",
                columns: new[] { "ContentId", "Content", "CreatedDate", "SectionName", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Welcome to our company management system.", new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3722), 1, "Company Overview", 1, new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3723) },
                    { 2, "We are a leading company in our industry.", new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3725), 2, "About Us", 1, new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3726) },
                    { 3, "We provide comprehensive business solutions.", new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3728), 3, "Our Services", 1, new DateTime(2025, 7, 31, 22, 36, 58, 21, DateTimeKind.Utc).AddTicks(3728) }
                });
        }
    }
}
