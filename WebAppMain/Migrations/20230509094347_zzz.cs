using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppMain.Migrations
{
    public partial class zzz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visiting_Computers_ComputerId",
                schema: "Identity",
                table: "Visiting");

            migrationBuilder.DropIndex(
                name: "IX_Visiting_ComputerId",
                schema: "Identity",
                table: "Visiting");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Visiting_ComputerId",
                schema: "Identity",
                table: "Visiting",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visiting_Computers_ComputerId",
                schema: "Identity",
                table: "Visiting",
                column: "ComputerId",
                principalSchema: "Identity",
                principalTable: "Computers",
                principalColumn: "ComputerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
