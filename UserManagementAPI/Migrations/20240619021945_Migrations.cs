using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagementAPI.Migrations
{
    public partial class Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Domain");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Domain",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 200, nullable: false),
                    Email = table.Column<string>(maxLength: 1000, nullable: true),
                    Alias = table.Column<string>(maxLength: 1000, nullable: true),
                    FirstName = table.Column<string>(maxLength: 1000, nullable: true),
                    LastName = table.Column<string>(maxLength: 1000, nullable: true),
                    UserType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain.Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                schema: "Domain",
                columns: table => new
                {
                    ManagerId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain.Managers", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Domain.Managers_Users",
                        column: x => x.ManagerId,
                        principalSchema: "Domain",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "Domain",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    ManagerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain.Clients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Domain.Clients_Users",
                        column: x => x.ClientId,
                        principalSchema: "Domain",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Domain.Clients_Managers",
                        column: x => x.ManagerId,
                        principalSchema: "Domain",
                        principalTable: "Managers",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ManagerId",
                schema: "Domain",
                table: "Clients",
                column: "ManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Managers",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Domain");
        }
    }
}
