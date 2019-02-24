using Microsoft.EntityFrameworkCore.Migrations;

namespace Pm.Data.Migrations
{
    public partial class thumbnailrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_PostId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ThumbnailImageId",
                table: "Posts",
                column: "ThumbnailImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Images_ThumbnailImageId",
                table: "Posts",
                column: "ThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Images_ThumbnailImageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ThumbnailImageId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageId",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Images",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_PostId",
                table: "Images",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
