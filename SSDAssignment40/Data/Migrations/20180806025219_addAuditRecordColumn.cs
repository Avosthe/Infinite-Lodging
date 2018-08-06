using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class addAuditRecordColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditRecordId",
                table: "RevertChanges",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RevertChanges_AuditRecordId",
                table: "RevertChanges",
                column: "AuditRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_RevertChanges_AuditRecords_AuditRecordId",
                table: "RevertChanges",
                column: "AuditRecordId",
                principalTable: "AuditRecords",
                principalColumn: "AuditRecordId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RevertChanges_AuditRecords_AuditRecordId",
                table: "RevertChanges");

            migrationBuilder.DropIndex(
                name: "IX_RevertChanges_AuditRecordId",
                table: "RevertChanges");

            migrationBuilder.DropColumn(
                name: "AuditRecordId",
                table: "RevertChanges");
        }
    }
}
