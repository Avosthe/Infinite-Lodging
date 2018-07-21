using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class DropAuditRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditRecords");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleDescription",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_CreatedById",
                table: "AspNetRoles",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_CreatedById",
                table: "AspNetRoles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_CreatedById",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_CreatedById",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "RoleDescription",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "AuditRecords",
                columns: table => new
                {
                    Audit_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuditActionType = table.Column<string>(nullable: true),
                    DateTimeStamp = table.Column<DateTime>(nullable: false),
                    LodgerId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRecords", x => x.Audit_ID);
                    table.ForeignKey(
                        name: "FK_AuditRecords_AspNetUsers_LodgerId",
                        column: x => x.LodgerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditRecords_LodgerId",
                table: "AuditRecords",
                column: "LodgerId");
        }
    }
}
