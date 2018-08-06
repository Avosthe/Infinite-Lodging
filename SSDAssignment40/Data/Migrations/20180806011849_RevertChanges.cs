using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class RevertChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevertChanges",
                columns: table => new
                {
                    RevertChangesId = table.Column<string>(nullable: false),
                    OldLodgerUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevertChanges", x => x.RevertChangesId);
                    table.ForeignKey(
                        name: "FK_RevertChanges_AspNetUsers_OldLodgerUserId",
                        column: x => x.OldLodgerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevertChanges_OldLodgerUserId",
                table: "RevertChanges",
                column: "OldLodgerUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevertChanges");
        }
    }
}
