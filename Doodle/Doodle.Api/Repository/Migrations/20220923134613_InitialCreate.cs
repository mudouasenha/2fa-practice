using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doodle.Api.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false),
                    Username = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "varchar(62)", maxLength: 62, nullable: false),
                    Password = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "nci_Users_Email",
                table: "Users",
                column: "Email",
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "nci_Users_Username",
                table: "Users",
                column: "Username",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}