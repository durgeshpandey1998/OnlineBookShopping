using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineBookShopping.Migrations
{
    public partial class DocumentUpload1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DelayDay",
                table: "BookRent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIssued",
                table: "BookRent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturn",
                table: "BookRent",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelayDay",
                table: "BookRent");

            migrationBuilder.DropColumn(
                name: "IsIssued",
                table: "BookRent");

            migrationBuilder.DropColumn(
                name: "IsReturn",
                table: "BookRent");
        }
    }
}
