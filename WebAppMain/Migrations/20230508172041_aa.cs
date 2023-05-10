using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppMain.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visiting_Visitors_VisitorsId",
                schema: "Identity",
                table: "Visiting");

            migrationBuilder.DropIndex(
                name: "IX_Visiting_VisitorsId",
                schema: "Identity",
                table: "Visiting");

            migrationBuilder.AlterColumn<int>(
                name: "VisitorsId",
                schema: "Identity",
                table: "Visiting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Visiting_VisitorsId",
                schema: "Identity",
                table: "Visiting",
                column: "VisitorsId",
                unique: true,
                filter: "[VisitorsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Visiting_Visitors_VisitorsId",
                schema: "Identity",
                table: "Visiting",
                column: "VisitorsId",
                principalSchema: "Identity",
                principalTable: "Visitors",
                principalColumn: "VisitorsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visiting_Visitors_VisitorsId",
                schema: "Identity",
                table: "Visiting");

            migrationBuilder.DropIndex(
                name: "IX_Visiting_VisitorsId",
                schema: "Identity",
                table: "Visiting");

            migrationBuilder.AlterColumn<int>(
                name: "VisitorsId",
                schema: "Identity",
                table: "Visiting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visiting_VisitorsId",
                schema: "Identity",
                table: "Visiting",
                column: "VisitorsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Visiting_Visitors_VisitorsId",
                schema: "Identity",
                table: "Visiting",
                column: "VisitorsId",
                principalSchema: "Identity",
                principalTable: "Visitors",
                principalColumn: "VisitorsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
