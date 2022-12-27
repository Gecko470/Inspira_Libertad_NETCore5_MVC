using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspira_Libertad.Data.Migrations
{
    public partial class ColumnImpTotalOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ImporteTotal",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImporteTotal",
                table: "Orders");
        }
    }
}
