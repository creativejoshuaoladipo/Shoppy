using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoppy.Migrations
{
    public partial class EditProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
