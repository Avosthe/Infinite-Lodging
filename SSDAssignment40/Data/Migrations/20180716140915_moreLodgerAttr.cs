using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class moreLodgerAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbsDown",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ThumbsUp",
                table: "AspNetUsers",
                newName: "Rating");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nric",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteLabel",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nric",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SiteLabel",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "AspNetUsers",
                newName: "ThumbsUp");

            migrationBuilder.AddColumn<int>(
                name: "ThumbsDown",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
