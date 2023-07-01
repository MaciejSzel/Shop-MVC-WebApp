using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sklep.Intranet.Migrations
{
    public partial class SklepContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RodzajId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rodzaj",
                columns: table => new
                {
                    IdRodzaju = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rodzaj", x => x.IdRodzaju);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_RodzajId",
                table: "Products",
                column: "RodzajId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Rodzaj_RodzajId",
                table: "Products",
                column: "RodzajId",
                principalTable: "Rodzaj",
                principalColumn: "IdRodzaju",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Rodzaj_RodzajId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Rodzaj");

            migrationBuilder.DropIndex(
                name: "IX_Products_RodzajId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RodzajId",
                table: "Products");
        }
    }
}
