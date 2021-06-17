using Microsoft.EntityFrameworkCore.Migrations;

namespace shopapp.webui.Migrations
{
    public partial class Addinwallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "wallet",
                table: "AspNetUsers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "wallet",
                table: "AspNetUsers");
        }
    }
}
