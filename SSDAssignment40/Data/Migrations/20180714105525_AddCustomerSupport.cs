using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class AddCustomerSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Replies",
                table: "CustomerSupport",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Replies",
                table: "CustomerSupport",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
