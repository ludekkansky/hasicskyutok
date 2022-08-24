using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hasickyutok.Migrations
{
    public partial class Aktualizace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorie",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Druzstva",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<string>(type: "TEXT", nullable: true),
                    StartovniCislo = table.Column<int>(type: "INTEGER", nullable: false),
                    KategorieID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Druzstva", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Druzstva_Kategorie_KategorieID",
                        column: x => x.KategorieID,
                        principalTable: "Kategorie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Druzstva_KategorieID",
                table: "Druzstva",
                column: "KategorieID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Druzstva");

            migrationBuilder.DropTable(
                name: "Kategorie");
        }
    }
}
