using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class Lodgers_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lodger",
                columns: table => new
                {
                    LodgerID = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    JoinDate = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lodger", x => x.LodgerID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
