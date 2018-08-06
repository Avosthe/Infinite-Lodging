using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class adrattr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditRecordId",
                table: "UserReverts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReverts_AuditRecordId",
                table: "UserReverts",
                column: "AuditRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReverts_AuditRecords_AuditRecordId",
                table: "UserReverts",
                column: "AuditRecordId",
                principalTable: "AuditRecords",
                principalColumn: "AuditRecordId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReverts_AuditRecords_AuditRecordId",
                table: "UserReverts");

            migrationBuilder.DropIndex(
                name: "IX_UserReverts_AuditRecordId",
                table: "UserReverts");

            migrationBuilder.DropColumn(
                name: "AuditRecordId",
                table: "UserReverts");
        }
    }
}
