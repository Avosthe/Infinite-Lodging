using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class _3AuthProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "is3AuthEnabled",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "is3AuthPattern",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is3AuthEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "is3AuthPattern",
                table: "AspNetUsers");
        }
    }
}
