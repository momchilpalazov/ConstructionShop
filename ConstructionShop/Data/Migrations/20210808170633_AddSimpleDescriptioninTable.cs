using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionShop.Data.Migrations
{
    public partial class AddSimpleDescriptioninTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SimpleDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SimpleDescription",
                table: "Products");
        }
    }
}
