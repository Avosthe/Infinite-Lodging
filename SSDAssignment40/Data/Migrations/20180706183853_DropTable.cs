using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class DropTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name : "Lodger");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
