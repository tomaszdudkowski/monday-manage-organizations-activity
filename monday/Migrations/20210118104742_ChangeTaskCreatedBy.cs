using Microsoft.EntityFrameworkCore.Migrations;

namespace mondayWebApp.Migrations
{
    public partial class ChangeTaskCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Employees_TaskCreatedByEmployeeID",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_TaskCreatedByEmployeeID",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "TaskCreatedByEmployeeID",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskCreatedBy",
                table: "ProjectTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskCreatedBy",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskCreatedByEmployeeID",
                table: "ProjectTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_TaskCreatedByEmployeeID",
                table: "ProjectTasks",
                column: "TaskCreatedByEmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Employees_TaskCreatedByEmployeeID",
                table: "ProjectTasks",
                column: "TaskCreatedByEmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
