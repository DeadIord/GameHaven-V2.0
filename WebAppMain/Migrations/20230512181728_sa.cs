using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppMain.Migrations
{
    public partial class sa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                schema: "Identity",
                table: "Visiting",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCost",
                schema: "Identity",
                table: "Visiting");
        }
    }
}
