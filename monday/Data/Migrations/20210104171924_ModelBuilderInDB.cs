using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mondayWebApp.Migrations
{
    public partial class ModelBuilderInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(nullable: true),
                    DepartmentDesc = table.Column<string>(nullable: true),
                    DepartmentEstablishmentDate = table.Column<DateTime>(nullable: false),
                    IsEdited = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectDesc = table.Column<string>(nullable: true),
                    ProjectBrief = table.Column<string>(nullable: true),
                    ProjectDeadline = table.Column<DateTime>(nullable: false),
                    IsEdited = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(nullable: true),
                    EmployeeSurname = table.Column<string>(nullable: true),
                    EmployeeDateOfBirth = table.Column<DateTime>(nullable: false),
                    EmployeePhoneNumber = table.Column<string>(nullable: true),
                    EmployeeRole = table.Column<string>(nullable: true),
                    DepartmentID = table.Column<int>(nullable: true),
                    ProjectID = table.Column<int>(nullable: true),
                    IsEdited = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    DepartmentID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentID1",
                        column: x => x.DepartmentID1,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    ApartmentNumber = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: false),
                    IsEdited = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    EmployeeID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Addresses_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Employees_EmployeeID1",
                        column: x => x.EmployeeID1,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(nullable: true),
                    TaskDeadline = table.Column<DateTime>(nullable: false),
                    TaskCreatedByEmployeeID = table.Column<int>(nullable: true),
                    TaskEmployeeResponsibleForEmployeeID = table.Column<int>(nullable: true),
                    ProjectID = table.Column<int>(nullable: true),
                    IsEdited = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tasks_Employees_TaskCreatedByEmployeeID",
                        column: x => x.TaskCreatedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Employees_TaskEmployeeResponsibleForEmployeeID",
                        column: x => x.TaskEmployeeResponsibleForEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_EmployeeID",
                table: "Addresses",
                column: "EmployeeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_EmployeeID1",
                table: "Addresses",
                column: "EmployeeID1",
                unique: true,
                filter: "[EmployeeID1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID1",
                table: "Employees",
                column: "DepartmentID1",
                unique: true,
                filter: "[DepartmentID1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProjectID",
                table: "Employees",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectID",
                table: "Tasks",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCreatedByEmployeeID",
                table: "Tasks",
                column: "TaskCreatedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskEmployeeResponsibleForEmployeeID",
                table: "Tasks",
                column: "TaskEmployeeResponsibleForEmployeeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
