using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class addUserRatingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Listing",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    UserRatingId = table.Column<string>(nullable: false),
                    RatedId = table.Column<string>(nullable: true),
                    RaterId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRating", x => x.UserRatingId);
                    table.ForeignKey(
                        name: "FK_UserRating_AspNetUsers_RatedId",
                        column: x => x.RatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRating_AspNetUsers_RaterId",
                        column: x => x.RaterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_RatedId",
                table: "UserRating",
                column: "RatedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_RaterId",
                table: "UserRating",
                column: "RaterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "UserRating");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Listing",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
