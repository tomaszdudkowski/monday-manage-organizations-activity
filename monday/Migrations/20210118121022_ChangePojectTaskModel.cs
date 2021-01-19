using Microsoft.EntityFrameworkCore.Migrations;

namespace mondayWebApp.Migrations
{
    public partial class ChangePojectTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnd",
                table: "ProjectTasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnd",
                table: "ProjectTasks");
        }
    }
}
