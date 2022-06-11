using Microsoft.EntityFrameworkCore.Migrations;

namespace asp_mvc_std_v2.Data.Migrations
{
    public partial class MainAdmin_Area_Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produkt_Sklep_SklepId",
                table: "Produkt");

            migrationBuilder.DropIndex(
                name: "IX_Produkt_SklepId",
                table: "Produkt");

            migrationBuilder.DropColumn(
                name: "SklepId",
                table: "Produkt");

            migrationBuilder.AddColumn<int>(
                name: "ProduktWSklepieID",
                table: "Sklep",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProduktWSklepieID",
                table: "Sklep");

            migrationBuilder.AddColumn<int>(
                name: "SklepId",
                table: "Produkt",
                type: "int",
                nullable: true);

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
    }
}
