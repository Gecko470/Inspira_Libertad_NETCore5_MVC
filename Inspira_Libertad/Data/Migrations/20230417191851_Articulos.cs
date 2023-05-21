using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspira_Libertad.Data.Migrations
{
    public partial class Articulos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articulos",
                columns: table => new
                {
                    ArticuloId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precio = table.Column<float>(type: "real", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articulos", x => x.ArticuloId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articulos");
        }
    }
}
