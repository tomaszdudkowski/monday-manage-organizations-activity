using Microsoft.EntityFrameworkCore.Migrations;

namespace mondayWebApp.Migrations
{
    public partial class asp_net_core_app_Monday_DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Employees_EmployeeID",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_EmployeeID",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "AspNetRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_EmployeeID",
                table: "AspNetRoles",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Employees_EmployeeID",
                table: "AspNetRoles",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
