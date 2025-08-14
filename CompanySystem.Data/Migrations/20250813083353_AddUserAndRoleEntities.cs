using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndRoleEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedBy", "CreatedDate", "Description", "IsActive", "IsDeleted", "RoleName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5616), "Full system access with all permissions", true, false, "Administrator", null, null },
                    { 2, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5618), "Human Resources management access", true, false, "HR Manager", null, null },
                    { 3, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5620), "Information Technology management access", true, false, "IT Manager", null, null },
                    { 4, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5621), "Standard employee access", true, false, "Employee", null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedBy", "CreatedDate", "DepartmentId", "Email", "FirstName", "IsActive", "IsDeleted", "LastLoginDate", "LastName", "PhoneNumber", "RoleId", "UpdatedBy", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5643), null, "admin@company.com", "System", true, false, new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5642), "Administrator", "1234567890", 1, null, null, "admin" },
                    { 2, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5646), 1, "hr@company.com", "HR", true, false, new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5645), "Manager", "1234567891", 2, null, null, "hr.manager" },
                    { 3, "System", new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5649), 2, "it@company.com", "IT", true, false, new DateTime(2025, 8, 13, 8, 33, 53, 143, DateTimeKind.Utc).AddTicks(5648), "Manager", "1234567892", 3, null, null, "it.manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 15, 59, 33, 19, DateTimeKind.Utc).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 15, 59, 33, 19, DateTimeKind.Utc).AddTicks(8539));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 15, 59, 33, 19, DateTimeKind.Utc).AddTicks(8631));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 15, 59, 33, 19, DateTimeKind.Utc).AddTicks(8633));

            migrationBuilder.UpdateData(
                table: "MainPageContents",
                keyColumn: "ContentId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 15, 59, 33, 19, DateTimeKind.Utc).AddTicks(8635));
        }
    }
}
