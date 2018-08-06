using Microsoft.EntityFrameworkCore.Migrations;

namespace SSDAssignment40.Data.Migrations
{
    public partial class RevertUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReverts",
                columns: table => new
                {
                    UserRevertId = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    AlternateEmail = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Occupation = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    GovernmentID = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Biography = table.Column<string>(nullable: true),
                    Hobbies = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<string>(nullable: true),
                    is3AuthEnabled = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReverts", x => x.UserRevertId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReverts");
        }
    }
}
