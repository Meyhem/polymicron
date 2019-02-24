using Microsoft.EntityFrameworkCore.Migrations;

namespace Pm.Data.Migrations
{
    public partial class rating_index_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_PostId_Token",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_PostId_Token",
                table: "Ratings",
                columns: new[] { "PostId", "Token" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_PostId_Token",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_PostId_Token",
                table: "Ratings",
                columns: new[] { "PostId", "Token" });
        }
    }
}
