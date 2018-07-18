using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class UpdateUserReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewFor",
                table: "UserReview",
                newName: "ReviewForId");

            migrationBuilder.RenameColumn(
                name: "ReviewBy",
                table: "UserReview",
                newName: "ReviewById");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewForId",
                table: "UserReview",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewById",
                table: "UserReview",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReview_ReviewById",
                table: "UserReview",
                column: "ReviewById");

            migrationBuilder.CreateIndex(
                name: "IX_UserReview_ReviewForId",
                table: "UserReview",
                column: "ReviewForId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReview_AspNetUsers_ReviewById",
                table: "UserReview",
                column: "ReviewById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReview_AspNetUsers_ReviewForId",
                table: "UserReview",
                column: "ReviewForId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReview_AspNetUsers_ReviewById",
                table: "UserReview");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReview_AspNetUsers_ReviewForId",
                table: "UserReview");

            migrationBuilder.DropIndex(
                name: "IX_UserReview_ReviewById",
                table: "UserReview");

            migrationBuilder.DropIndex(
                name: "IX_UserReview_ReviewForId",
                table: "UserReview");

            migrationBuilder.RenameColumn(
                name: "ReviewForId",
                table: "UserReview",
                newName: "ReviewFor");

            migrationBuilder.RenameColumn(
                name: "ReviewById",
                table: "UserReview",
                newName: "ReviewBy");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewFor",
                table: "UserReview",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewBy",
                table: "UserReview",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
