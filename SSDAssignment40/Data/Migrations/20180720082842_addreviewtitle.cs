using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class addreviewtitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewTitle",
                table: "Review",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewTitle",
                table: "Review");
        }
    }
}
