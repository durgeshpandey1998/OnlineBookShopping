using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineBookShopping.Migrations
{
    public partial class AddQtySell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BookSell",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BookSell");
        }
    }
}
