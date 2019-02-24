using Microsoft.EntityFrameworkCore.Migrations;

namespace Pm.Data.Migrations
{
    public partial class image_mime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mime",
                table: "Images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mime",
                table: "Images");
        }
    }
}
