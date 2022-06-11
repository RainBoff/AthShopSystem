using Microsoft.EntityFrameworkCore.Migrations;

namespace asp_mvc_std_v2.Data.Migrations
{
    public partial class ProduktWSklepie_Update_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProduktWSklepieID",
                table: "Sklep");

            migrationBuilder.AddColumn<int>(
                name: "ProductName",
                table: "ProduktWSklepie",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ProduktWSklepie");

            migrationBuilder.AddColumn<int>(
                name: "ProduktWSklepieID",
                table: "Sklep",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
