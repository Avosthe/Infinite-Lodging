using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class rmvRevertChangesClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevertChanges");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevertChanges",
                columns: table => new
                {
                    RevertChangesId = table.Column<string>(nullable: false),
                    AuditRecordId = table.Column<string>(nullable: true),
                    OldLodgerUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevertChanges", x => x.RevertChangesId);
                    table.ForeignKey(
                        name: "FK_RevertChanges_AuditRecords_AuditRecordId",
                        column: x => x.AuditRecordId,
                        principalTable: "AuditRecords",
                        principalColumn: "AuditRecordId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RevertChanges_AspNetUsers_OldLodgerUserId",
                        column: x => x.OldLodgerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevertChanges_AuditRecordId",
                table: "RevertChanges",
                column: "AuditRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RevertChanges_OldLodgerUserId",
                table: "RevertChanges",
                column: "OldLodgerUserId");
        }
    }
}
