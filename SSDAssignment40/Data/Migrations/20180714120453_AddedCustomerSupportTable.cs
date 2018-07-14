using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class AddedCustomerSupportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSupport",
                columns: table => new
                {
                    CustomerSupport_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    DateTimeStamp = table.Column<DateTime>(nullable: false),
                    Request = table.Column<string>(nullable: true),
                    NoReplies = table.Column<int>(nullable: false),
                    Replies = table.Column<string>(nullable: true),
                    LodgerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupport", x => x.CustomerSupport_ID);
                    table.ForeignKey(
                        name: "FK_CustomerSupport_AspNetUsers_LodgerId",
                        column: x => x.LodgerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupport_LodgerId",
                table: "CustomerSupport",
                column: "LodgerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSupport");
        }
    }
}
