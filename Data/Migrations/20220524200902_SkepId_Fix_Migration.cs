using Microsoft.EntityFrameworkCore.Migrations;

namespace asp_mvc_std_v2.Data.Migrations
{
    public partial class SkepId_Fix_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SklepId",
                table: "Produkt",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sklep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sklep", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produkt_SklepId",
                table: "Produkt",
                column: "SklepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produkt_Sklep_SklepId",
                table: "Produkt",
                column: "SklepId",
                principalTable: "Sklep",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produkt_Sklep_SklepId",
                table: "Produkt");

            migrationBuilder.DropTable(
                name: "Sklep");

            migrationBuilder.DropIndex(
                name: "IX_Produkt_SklepId",
                table: "Produkt");

            migrationBuilder.DropColumn(
                name: "SklepId",
                table: "Produkt");
        }
    }
}
