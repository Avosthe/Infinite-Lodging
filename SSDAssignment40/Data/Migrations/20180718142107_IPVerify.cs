using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class IPVerify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalVerificationSecret",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAdditionalVerification",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalVerificationSecret",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RequireAdditionalVerification",
                table: "AspNetUsers");
        }
    }
}
