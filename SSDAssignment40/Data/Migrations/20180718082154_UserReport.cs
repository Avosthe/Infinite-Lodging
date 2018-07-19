using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class UserReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReport",
                columns: table => new
                {
                    UserReportId = table.Column<string>(nullable: false),
                    ReportedUserId = table.Column<string>(nullable: true),
                    ReportingUserId = table.Column<string>(nullable: true),
                    ReportedContent = table.Column<string>(nullable: true),
                    ReportFileEvidence = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    isReviewd = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReport", x => x.UserReportId);
                    table.ForeignKey(
                        name: "FK_UserReport_AspNetUsers_ReportedUserId",
                        column: x => x.ReportedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserReport_AspNetUsers_ReportingUserId",
                        column: x => x.ReportingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReport_ReportedUserId",
                table: "UserReport",
                column: "ReportedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReport_ReportingUserId",
                table: "UserReport",
                column: "ReportingUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReport");
        }
    }
}
