using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class reply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReviewTitle",
                table: "Review",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewDesc",
                table: "Review",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeStamp",
                table: "Reply",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Reply",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Listing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Listing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Desc",
                table: "Listing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "CreditCard",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeStamp",
                table: "Reply");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Reply");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewTitle",
                table: "Review",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ReviewDesc",
                table: "Review",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Listing",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Listing",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Desc",
                table: "Listing",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "CreditCard",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
